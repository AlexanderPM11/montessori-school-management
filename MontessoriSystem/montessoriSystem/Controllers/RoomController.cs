using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers.Date;
using MontessoriSystem.Core.Application.Helpers.InitData;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.Room;
using MontessoriSystem.Core.Application.ViewModels.RoomSchedule;
using MontessoriSystem.Core.Application.ViewModels.TeachingPeriods;
using MontessoriSystem.Core.Application.ViewModels.TypeRegister;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Infrastructure.Identity.Seeds;
using System.Net;

namespace montessoriSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly ISujectService _sujectService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileServices _fileServices;
        private readonly IRoomScheduleService _roomScheduleService;
        private readonly string _wwwrootPath;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IRoomTeacherService _roomTeacherService;
        private readonly ITypeRegisterService _typeRegisterService;
        private readonly IInitDataHelper _initDataHelper;

        public RoomController(IRoomService roomService,
            IUserService userService, IMapper mapper, IFileServices fileServices, 
            IWebHostEnvironment webHostEnvironment, ISujectService sujectService, 
            IRoomScheduleService roomScheduleService, IValidateInfoDataBase validateInfoDataBase, IRoomTeacherService roomTeacherService,
            ITypeRegisterService typeRegisterService, IInitDataHelper initDataHelper)
        {
            _roomService = roomService;
            _userService = userService;
            _mapper = mapper;
            _fileServices = fileServices;
            _wwwrootPath = webHostEnvironment.WebRootPath;
            _sujectService = sujectService;
            _roomScheduleService = roomScheduleService;
            _validateInfoDataBase = validateInfoDataBase;
            _roomTeacherService = roomTeacherService;
            _typeRegisterService = typeRegisterService;
            _initDataHelper = initDataHelper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //(string action, string controller) = await _initDataHelper.ValidatedInitData();

            //if (action != null)
            //{
            //    return RedirectToAction(action, controller);
            //}
            var userName = User.Identity.Name;
            var user = await _userService.GetUserByName(userName);

            ViewBag.idInstitu = user.InstitutionId  ?? 0;
            return View();

        }

        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? id, int idInstitu)
        {
            var userName = User.Identity.Name;
            var user = await _userService.GetUserByName(userName);

            
            List<ClassSelected<string>> users = await GetTeachers();
            var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();
            var targetLevels = GetLevels(typeRegisterViewModels);

            ViewBag.TeacherUsers = users;
            ViewBag.Levels = targetLevels;

            if (id != null && id > 0)
            {
                var room = await _roomService.GetByIdSaveViewModel((int)id);

                if (!user.Roles.Contains(Roles.Admin.ToString()))
                {
                     return PartialView("");
                }

                var roomoViewModels = await _roomService.GetBy(room =>
                 room.Id == id);

                if (roomoViewModels != null && roomoViewModels.Count > 0)
                {
                    var model = _mapper.Map<RoomSaveViewModel>(roomoViewModels.FirstOrDefault());
                    if(model.IdTypeRegisters != null)
                    {
                        var idSelects = model.IdTypeRegisters.Split(",");
                        foreach (var idSelect in idSelects)
                        {
                            model.SelectLevels.Add(int.Parse(idSelect));
                        }
                    }
                    

                    return PartialView(model);
                }
            }

            return PartialView(new RoomSaveViewModel());

        }       

        [HttpPost]
        public async Task<JsonResult> CreateUpdatePost(RoomSaveViewModel model, int idInstitu, string[] IdTypeRegisters)
        {
            try
            {
                ModelState.Remove("SelectLevels");

                if (ModelState.IsValid)
                {
                    if (IdTypeRegisters != null)
                    {
                        // Filtra los valores vacíos y convierte a un string separado por comas
                        model.IdTypeRegisters = string.Join(",", IdTypeRegisters.Where(id => !string.IsNullOrEmpty(id)));
                    }

                    var userName = User.Identity.Name;
                    var user = await _userService.GetUserByName(userName);

                    if (!user.Roles.Contains(Roles.Admin.ToString()))
                    {
                        return Json(new { });
                    }

                    bool deleteImg = false;
                    
                    var exitRegister = await _roomService.GetBy(room => room.Id == model.Id &&
                    room.IdEducationalInsti == idInstitu);

                    if (model.File != null && model.File.Length > 0)
                    {
                        var pathNewFolder = $"{_wwwrootPath}/FileUser/Room";
                        GeneralResponse<string> reponse = await _fileServices.CreateOrUpdateFile(model.File, pathNewFolder);

                        model.ImageUrl = reponse.Data;
                        deleteImg = true;
                    }
                    var DBModel = exitRegister.FirstOrDefault();

                    if (deleteImg && exitRegister.Count > 0)
                    {
                        string rutaArchivo = $"FileUser/Room/{DBModel.ImageUrl}";
                        var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
                        await _fileServices.DeleteFile(filePath);
                    }

                    
                    model.IdEducationalInsti = idInstitu;
                    if (model.Id > 0)
                    {
                        if (!deleteImg  && DBModel != null)
                        {
                            model.ImageUrl = DBModel.ImageUrl;
                        }

                        await _roomService.Update(model, model.Id);
                    }
                    else
                    {
                        var teachingPeriods = await _roomService.Add(model);
                    }

                    return Json(new { data = "Información actualizada correctamente!", result = true });
                }

                return Json(new { data = "Datos sin validar!", result = false });
            }
            catch (Exception ex)
            {
                return Json(new { data = "Ocurrio un error!.. " + ex.Message, result = false });
            }


        }

        [HttpGet]
        public async Task<PartialViewResult> Rooms(int? idInstitu)
        {
            ViewBag.idInstitu = idInstitu;
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> GetRoomsJson(int? idInstitu)
        {
            try
            {
                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Profesor.ToString()))
                {
                    return Json(new { });
                }


                List<RoomoViewModel> roomoViewModelsTarget = new();
                List<RoomoViewModel> roomoViewModels = await _roomService.GetAllViewModel();

                var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();

                List<ClassSelected<string>> usersTeachers = await GetTeachers();

                if(userCurrent.Roles.Count == 1 && userCurrent.Roles.Contains(Roles.Profesor.ToString()))
                {
                    if (!string.IsNullOrEmpty(userCurrent.IdsRoom))
                    {
                        var idsRoomTeacher = userCurrent.IdsRoom.Split(",");

                        List<int> IdsRoom = new();

                        foreach (var idRoom in idsRoomTeacher)
                        {
                            var room = roomoViewModels.Where(x => x.Id == int.Parse(idRoom)).FirstOrDefault();
                            roomoViewModelsTarget.Add(room);
                        }
                    } 

                    List<RoomScheduleViewModel> roomScheduleViewModels = new List<RoomScheduleViewModel>();
                    
                    var roomTeacherViewModels = await _roomTeacherService.GetBy(grade =>
                    grade.IdTeacher == userCurrent.Id);

                    var distinctroomTeacherViewModels = roomTeacherViewModels
                       .GroupBy(r => r.IdRoom)
                       .Select(g => g.First())
                       .ToList();

                    foreach (var item in distinctroomTeacherViewModels)
                    {
                        var roomSearch = await _roomService.GetBy(room =>
                        room.Id == item.IdRoom);
                        var room = roomSearch.FirstOrDefault();
                        if (room != null)
                        {
                            roomoViewModels.Add(room);
                        }
                            
                    }

                    roomScheduleViewModels = await _roomScheduleService.GetBy(rooms =>
                        rooms.IdTeacher == userCurrent.Id);

                    // Eliminar IdTeacher repetidos
                    var distinctRoomSchedules = roomScheduleViewModels
                        .GroupBy(r => r.IdRoom)
                        .Select(g => g.First())
                        .ToList();
                    
                    foreach (var item in distinctRoomSchedules)
                    {
                        var roomSearch = await _roomService.GetBy(room =>
                        room.Id == item.IdRoom);
                        var roomFilter = roomSearch.FirstOrDefault();
                        if (roomFilter != null)
                        {
                            bool constainsRoom = roomoViewModels.Any(x => x.Id == roomFilter.Id);
                            if (!constainsRoom)
                            {
                                roomoViewModels.Add(roomFilter);
                            }
                        }
                    }
                }
                
                else
                {
                    roomoViewModelsTarget = roomoViewModels;
                }

                foreach (var user in roomoViewModels)
                {
                    if(usersTeachers.Count > 0)
                    {
                        if(user != null)
                        {
                            var targetResult = usersTeachers.Where(userT => userT.Id == user.IdTeacherLead).FirstOrDefault();
                            if (targetResult != null)
                            {
                                user.TeacherFullName = targetResult.text;
                            }
                        }                      
                        
                    }                    
                }

                if (roomoViewModelsTarget != null && roomoViewModelsTarget.Count > 0)
                {
                    var model = _mapper.Map<List<RoomSaveViewModel>>(roomoViewModelsTarget);
                    foreach (var item in model)
                    {
                        if (item != null)
                        {
                            if (item.IdTypeRegisters != null)
                            {
                                var idSelects = item.IdTypeRegisters.Split(",");
                                foreach (var idSelect in idSelects)
                                {
                                    var levelSelect = typeRegisterViewModels.FirstOrDefault(stu => stu.Id == int.Parse(idSelect));
                                    
                                    if (levelSelect != null)
                                    {
                                        if (!string.IsNullOrEmpty(item.Level))
                                        {
                                            item.Level += ", ";
                                        }
                                        // Agrega el nombre actual
                                        item.Level += levelSelect.Name;
                                    }
                                }
                                

                            }
                        }                        
                    }
                    model =  model.OrderBy(x => x.Id).ToList();
                    return Json(model);
                }

                else
                {
                    return Json(new List<RoomSaveViewModel>());
                }

            }


            catch (Exception ex)
            {
                return Json("");
            }

        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            return Json(
                new
                {
                    data = "No disponible por el momento!",
                    result = false
                }
             );
            var room = await _roomService.GetByIdSaveViewModel(id);

            var userName = User.Identity.Name;
            var userCurrent = await _userService.GetUserByName(userName);

            bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(room.IdEducationalInsti ?? 0, userCurrent);

            if (!isAuthorized)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(new { message = "" });
            }

            var roomoViewModels = await _roomService.GetBy(rooms =>
                rooms.Id == id);

            if (id > 0)
            {
                if (roomoViewModels.Count > 0)
                {
                    string rutaArchivo = $"FileUser/Room/{roomoViewModels.FirstOrDefault().ImageUrl}";
                    var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
                    await _fileServices.DeleteFile(filePath);
                }
                await _roomService.Delete(id);
                return Json(
                new
                {
                    data = "Salón eliminado correctamente!",
                    result = true
                }
             );
            }

            return Json(
                new
                {
                    data = "Salón no encontrado!",
                    result = false
                }
             );
        }

        [HttpGet]
        public async Task<PartialViewResult> PartialViewSchedule(int? id)
        {           

            return PartialView();

        }

        [HttpGet]
        public async Task<JsonResult> JsonSchedule(int? idRoomSelect)
        {
            var room = await _roomService.GetByIdSaveViewModel(idRoomSelect ?? 0);

            var userName = User.Identity.Name;
            var userCurrent = await _userService.GetUserByName(userName);

            if (!userCurrent.Roles.Contains(Roles.Profesor.ToString()))
            {
                return Json(new { });
            }

            List<RoomScheduleViewModel> roomoViewModels = new List<RoomScheduleViewModel>();

            if (userCurrent.Roles.Count == 1 && userCurrent.Roles.Contains(Roles.Profesor.ToString()))
            {
                var roomTeacherViewModels = await _roomTeacherService.GetBy(grade =>
                   grade.IdTeacher == userCurrent.Id);

                var distinctroomTeacherViewModels = roomTeacherViewModels
                   .GroupBy(r => r.IdRoom)
                   .Select(g => g.First())
                   .ToList();

                foreach (var item in distinctroomTeacherViewModels)
                {
                    var roomSearch = await _roomScheduleService.GetBy(room =>
                    room.Id == item.IdSuject  && room.IdRoom == idRoomSelect);
                    var roomFilter = roomSearch.FirstOrDefault();

                    if (roomFilter != null)
                    {
                        roomoViewModels.Add(roomFilter);
                    }

                }

                List<RoomScheduleViewModel> roomScheduleViewModels = new List<RoomScheduleViewModel>();

                roomScheduleViewModels = await _roomScheduleService.GetBy(rooms =>
                       rooms.IdTeacher == userCurrent.Id);

                foreach (var item in roomScheduleViewModels)
                {
                    var roomSearch = await _roomScheduleService.GetBy(room =>
                    room.Id == item.Id  && room.IdRoom == idRoomSelect);
                    var roomFilter = roomSearch.FirstOrDefault();

                    if (roomFilter != null)
                    {
                        bool constainsRoom = roomoViewModels.Any(x => x.Id == roomFilter.Id);
                        if (!constainsRoom)
                        {
                            roomoViewModels.Add(roomFilter);
                        }
                    }
                }

            }
            else
            {
                roomoViewModels = await _roomScheduleService.GetBy(rooms =>
                   rooms.IdRoom == idRoomSelect);
            }

            if (roomoViewModels.Count == 0)
            {
                return Json(roomoViewModels);
            }

            var teacherUsers = _userService.GetAllUser().Result.Data;

            teacherUsers = teacherUsers.Where(userTeacher =>
                userTeacher.Roles.Contains(Roles.Profesor.ToString())
                && userTeacher.Statu ).ToList();           

            var subjectViewModels = await _roomTeacherService.GetAllWithIncludeViewModel(new List<string> { "Suject" }, suject =>
                suject.IdRoom == idRoomSelect);


            foreach (var item in roomoViewModels)
            {
                var subjectView = subjectViewModels.Where(suject => suject.IdSuject == item.IdSubject).FirstOrDefault();

                var teacher = teacherUsers.Where (userTeacher =>  userTeacher.Id  == item.IdTeacher).FirstOrDefault();                

                if(teacher != null)
                {
                    item.TeacherName = $"{teacher.FirstName} {teacher.LastName}";
                }
               
                if (subjectView != null)
                {
                    item.SubjectName = subjectView.Suject.Name;
                }
                
            }

            return Json(roomoViewModels);

        }

        [HttpGet]
        public async Task<PartialViewResult> SetScheduleInfo( int? id,int? IdRoom, int idInstitu, string reqDataStart, string reqDataEnd, string TypeRegisterSelect)
        {
            var userName = User.Identity.Name;
            var userCurrent = await _userService.GetUserByName(userName);

            if (IdRoom!= null)
            {
                var room = await _roomService.GetByIdSaveViewModel((int)IdRoom);
                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return PartialView("");
                }
            }
            

            List<ClassSelected<string>> users = await GetTeachers();
            List<ClassSelected<int>> Sujects =await GetSuject((int)IdRoom);

            ViewBag.TeacherUsers = users;
            ViewBag.Sujects = Sujects;
            ViewBag.TypeRegisterSelect = TypeRegisterSelect;
            if (idInstitu > 0 && IdRoom!= null && id !=null)
            {
                var roomoViewModels = await _roomScheduleService.GetBy(rooms =>
               rooms.IdRoom == IdRoom &&  rooms.Id == id);

                if(roomoViewModels.Count > 0 )
                {
                    var ScheduleRoom = roomoViewModels.FirstOrDefault();

                    var targetResult = _mapper.Map<SaveViewModelRoomSchedule>(ScheduleRoom);
                    targetResult.InitDate = reqDataStart;
                    targetResult.FinishDate = reqDataEnd;
                    return PartialView(targetResult);
                }

            }


            return PartialView(new SaveViewModelRoomSchedule { InitDate  = reqDataStart, FinishDate  = reqDataEnd }  );

        }
        [HttpPost]
        public async Task<JsonResult> SaveScheduleInfo(SaveViewModelRoomSchedule model  , int idRoomSelect,string level)
        {
            if(ModelState.IsValid)
            {
                if (level != TypeRegisterEnum.Inicial.ToString()
                   && level != TypeRegisterEnum.Secundaria.ToString()
                   && level != TypeRegisterEnum.Primaria.ToString())
                {
                    return Json(new { data = "Nivel del salón incorrecto", result = false });
                }


                var room = await _roomService.GetByIdSaveViewModel(idRoomSelect);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);
                
                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { message = "" });
                }          

                if (model.Id > 0)
                {                    
                    model.IdRoom = idRoomSelect;
                    await _roomScheduleService.Update(model, model.Id);
                    return Json(new { data = "Actualizado con éxito!", result = true, Id = model.Id });
                }
                else
                {
                    if (level != TypeRegisterEnum.Inicial.ToString())
                    {
                        var roomoViewModels = await _roomScheduleService.GetBy(rooms =>
                                          rooms.InitDate.Contains(model.InitDate.Substring(0, 3)) && rooms.IdSubject == model.IdSubject && rooms.IdRoom == idRoomSelect);

                        if (roomoViewModels.Count > 0)
                        {
                            return Json(new { data = "Esta materia ya está planificada para la fecha seleccionada.", result = false });
                        }
                    }
                       
                    model.IdRoom  = idRoomSelect;
                    var resutlAdded = await _roomScheduleService.Add(model);
                    return Json(new { data = "Agregado con éxito!", result = true, Id= resutlAdded.Id });
                }
                
            }


            return Json(new { data = "Datos sin validar!", result = false });

        }
         [HttpPost]
        public async Task<JsonResult> SaveEventDropScheduleInfo(int id,string reqDataStart, string reqDataEnd, int IdRoom, string level)
        {
            var userName = User.Identity.Name;
            var userCurrent = await _userService.GetUserByName(userName);

            if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(new { message = "" });
            }

            if (level != TypeRegisterEnum.Inicial.ToString() 
                && level != TypeRegisterEnum.Secundaria.ToString() 
                && level != TypeRegisterEnum.Primaria.ToString())
            {
                return Json(new { data = "Nivel del salón incorrecto", result = false });
            }

            var eventDrop = await _roomScheduleService.GetByIdSaveViewModel(id);

            if(eventDrop != null)
            {
                if (level != TypeRegisterEnum.Inicial.ToString())
                {
                    var roomoViewModels = await _roomScheduleService.GetBy(rooms =>
                                    rooms.InitDate.Contains(reqDataStart.Substring(0, 3))
                                    && rooms.IdSubject == eventDrop.IdSubject
                                    && rooms.Id != id
                                    && rooms.IdRoom == IdRoom
                                    );

                    if (roomoViewModels.Count > 0)
                    {
                        return Json(new { data = "Esta materia ya está planificada para la fecha seleccionada.", result = false });
                    }
                }  
                eventDrop.InitDate = reqDataStart;
                eventDrop.FinishDate = reqDataEnd;
                await _roomScheduleService.Update(eventDrop, eventDrop.Id);
            }

            return Json(new { data = "", result = true });

        }

        [HttpPost]
        public async Task<JsonResult> DeleteScheduleInfo(int id, int idRoomSelect)
        {
            var roomSchedule = await _roomScheduleService.GetByIdSaveViewModel(id);

            var room = await _roomService.GetByIdSaveViewModel(roomSchedule.IdRoom);

            var userName = User.Identity.Name;
            var userCurrent = await _userService.GetUserByName(userName);

            if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(new { message = "" });
            }

            await _roomScheduleService.Delete(id);

            return Json( new { }  );

        }


        #region Private Method
        private List<ClassSelected<int>> GetLevels(List<TypeRegisterViewModel>? typeRegisters)
        {

            List<ClassSelected<int>> levels = new List<ClassSelected<int>>();

            foreach (var item in typeRegisters)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                levels.Add(classSelected);

            }
            return levels;
        }
        private async  Task<List<ClassSelected<string>>> GetTeachers()
        {

            var teacherUsers = _userService.GetAllUser().Result.Data;

            teacherUsers = teacherUsers.Where(userTeacher =>
                userTeacher.Roles.Contains(Roles.Profesor.ToString())
                && userTeacher.Statu ).ToList();

            List<ClassSelected<string>> users = new List<ClassSelected<string>>();

            foreach (var adminUser in teacherUsers)
            {
                ClassSelected<string> classSelected = new ClassSelected<string>
                {
                    Id = adminUser.Id,
                    text = $"{adminUser.FirstName} {adminUser.LastName}",
                };

                users.Add(classSelected);
            }
            return users;
        }
        private async   Task<List<ClassSelected<int>>> GetSuject(int IdRoom)
        {
            var subjectViewModels = await _roomTeacherService.GetAllWithIncludeViewModel(new List<string> { "Suject" }, suject =>
                suject.IdRoom == IdRoom);

            List<ClassSelected<int>> sujects = new List<ClassSelected<int>>();

            foreach (var suject in subjectViewModels)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = suject.Suject.Id,
                    text = suject.Suject.Name,
                };

                sujects.Add(classSelected);
            }
            return sujects;
        }

        #endregion
    
    
    }
}
