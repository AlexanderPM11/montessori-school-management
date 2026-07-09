using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;
using MontessoriSystem.Core.Application.ViewModels.Room;
using MontessoriSystem.Core.Application.ViewModels.RoomSchedule;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Domain.Entities;
using System.Linq;
using System.Security.Claims;


namespace MontessoriSystem.Core.Application.Services.Room
{
    public class CustomRoomService: ICustomRoomService
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStudentServices _studentServices;

        private string userName;

        public CustomRoomService(IRoomService roomService,
            IUserService userService, IMapper mapper, IFileServices fileServices,
            IWebHostEnvironment webHostEnvironment, ISujectService sujectService,
            IRoomScheduleService roomScheduleService, IValidateInfoDataBase validateInfoDataBase, IRoomTeacherService roomTeacherService,
            ITypeRegisterService typeRegisterService, IInitDataHelper initDataHelper, IHttpContextAccessor httpContextAccessor, IStudentServices studentServices)
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
            _httpContextAccessor = httpContextAccessor;

            _initDataHelper = initDataHelper;
            userName = _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
            _studentServices = studentServices;
        }
        public async Task<GeneralResponse<List<RoomoViewModel>>> GetAllRooms()
        {
            GeneralResponse<List<RoomoViewModel>> response = new();

            try
            {
                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Profesor.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permiso para visualizar estos datos");
                    return response;
                }


                List<RoomoViewModel> roomoViewModelsTarget = new();
                List<RoomoViewModel> roomoViewModels = await _roomService.GetAllViewModel();

                var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();

                List<ClassSelected<string>> usersTeachers = await GetTeachers();

                if (userCurrent.Roles.Count == 1 && userCurrent.Roles.Contains(Roles.Profesor.ToString()))
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
                    if (usersTeachers.Count > 0)
                    {
                        if (user != null)
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
                    var model = roomoViewModelsTarget;
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
                            if (item.ImageUrl != null)
                            {
                                var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/Room/{item.ImageUrl}");
                                item.ImageUrl = responseBase64.Data;
                            }
                        }
                    }
                    model = model.OrderBy(x => x.Id).ToList();
                    response.Data = model;
                    return response;
                }

                else
                {
                    return response;
                }

            }
            catch (Exception ex)
            {
                var error = $"Ocurrió un error: {ex.Message}";
                response.result = false;
                response.messages.Add(error);

                return response;
            }
        }
        public async Task<GeneralResponse<int>> CreateOrUpdate(RoomSaveViewModel model)
        {
            GeneralResponse<int> response = new();

            try
            {
                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permiso para visualizar estos datos");
                    return response;
                }

                bool deleteImg = false;

                var exitRegister = await _roomService.GetBy(room => room.Id == model.Id);
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                if (model.File != null && model.File.Length > 0)
                {
                    var pathNewFolder = $"FileUser/Room";
                    var filePath = Path.Combine(uploadsPath, pathNewFolder);

                    GeneralResponse<string> reponse = await _fileServices.CreateOrUpdateFile(model.File, filePath);

                    model.ImageUrl = reponse.Data;
                    deleteImg = true;
                }
                var DBModel = exitRegister.FirstOrDefault();

                if (deleteImg && exitRegister.Count > 0)
                {
                    string rutaArchivo = $"FileUser/Room/{DBModel.ImageUrl}";
                    var filePath = Path.Combine(uploadsPath, rutaArchivo);

                    await _fileServices.DeleteFile(filePath);
                }

                if (model.Id > 0)
                {
                    if (!deleteImg && DBModel != null)
                    {
                        model.ImageUrl = DBModel.ImageUrl;
                    }
                    response.Data = model.Id;

                    await DeleteStudentFromRoom(model);
                    await _roomService.Update(model, model.Id);
                }
                else
                {
                    var teachingPeriods = await _roomService.Add(model);
                    response.Data = teachingPeriods.Id;

                }
                response.messages.Add("Información actualizada correctamente!");

                return response;              

            }
            catch (Exception ex)
            {
                var error = $"Ocurrió un error: {ex.Message}";
                response.result = false;
                response.messages.Add(error);

                return response;
            }
        }
        public async Task<GeneralResponse<int>> Delete(int idRoom)
        {
            GeneralResponse<int> response = new();
            response.result = false;
            response.messages.Add("Accion no permitida");
            
            return response;

            try
            {
                var room = await _roomService.GetBy(adj => adj.Id.Equals(idRoom));

                if (room != null)
                {
                    var roomViewModel = _mapper.Map<RoomSaveViewModel>(room.FirstOrDefault());

                    await DeleteStudentFromRoom(roomViewModel, true);

                    await _roomService.Delete(idRoom);

                    string rutaArchivo = $"FileUser/Room/{room?.FirstOrDefault()?.ImageUrl}";
                    await _fileServices.DeleteFile(rutaArchivo);

                    response.messages.Add("Registro eliminado con exito!");
                    response.Data = idRoom;
                }
                else
                {
                    response.result = false;
                    response.messages.Add("No se encontró el registro.");
                }

                return response;
            }
            catch (Exception ex)
            {
                var error = $"Ocurrió un error: {ex.Message}";
                response.result = false;
                response.messages.Add(error);
                return response;
            }

        }


        #region Private methods
        private async Task<List<ClassSelected<string>>> GetTeachers()
        {

            var teacherUsers = _userService.GetAllUser().Result.Data;

            teacherUsers = teacherUsers.Where(userTeacher =>
                userTeacher.Roles.Contains(Roles.Profesor.ToString())
                && userTeacher.Statu).ToList();

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
        private async Task<bool> DeleteStudentFromRoom(RoomSaveViewModel roomModel, bool isDeleting = false)
        {
            if (roomModel == null || string.IsNullOrWhiteSpace(roomModel.IdTypeRegisters))
                return false;

            var idTypeRegisterList = roomModel.IdTypeRegisters
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.Parse(id))
                .ToList();

            var students = await _studentServices.GetBy(student => student.IdRoom == roomModel.Id);
            var studentsToRemove = new List<StudentViewModel>();

            if (isDeleting)
            {
                studentsToRemove = students
               .Where(student => idTypeRegisterList.Contains(student.IdTypeRegister ?? 0))
               .ToList();
            }
            else
            {
                studentsToRemove = students
               .Where(student => !idTypeRegisterList.Contains(student.IdTypeRegister ?? 0))
               .ToList();
            }           

            foreach (var item in studentsToRemove)
            {
                var studentSave = _mapper.Map<StudentSaveViewModel>(item);
                studentSave.IdRoom = null;

                await _studentServices.Update(studentSave, item.Id);
            }           

            return true;
        }



        #endregion


    }
}
