using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.RoomTeacher;
using MontessoriSystem.Core.Domain.Entities;
using System.Net;

namespace montessoriSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Teacher")]

    public class RoomTeacherController : Controller
    {
         private readonly IRoomService _roomService;
         private readonly IGradeService _gradeService;
         private readonly ISujectService _sujectService;
         private readonly IRoomTeacherService _roomTeacherService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidateInfoDataBase _validateInfoDataBase;

        public RoomTeacherController(IRoomService roomService, IRoomTeacherService roomTeacherService, IUserService userService, 
            IMapper mapper, IGradeService gradeService, IValidateInfoDataBase validateInfoDataBase, ISujectService sujectService)
        {
            _roomService = roomService;
            _roomTeacherService = roomTeacherService;
            _userService = userService;
            _mapper = mapper;
            _gradeService = gradeService;
            _userService = userService;
            _validateInfoDataBase = validateInfoDataBase;
            _sujectService = sujectService;
        }

        [HttpGet]
        public async Task<PartialViewResult> RoomTeachers(int? idRoom)
        {
            ViewBag.IdRoom = idRoom;
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> GetRoomTeachersJson(int? idRoom, int idInstitu)
        {
            try
            {
                var room = await _roomService.GetByIdSaveViewModel((int)idRoom);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);
                
                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    return Json("");
                }               

                ViewBag.Teachers = await GetTeachers(idRoom ?? 0);
                ViewBag.IdRoom = idRoom;
                var roomTeacherViewModels = await _roomTeacherService.GetBy(grade =>
                grade.IdRoom == idRoom);

                List<ClassSelected<string>> usersTeachers = await GetTeachers(idRoom ?? 0);
                List<ClassSelected<int>> grades = await GetSujects(idInstitu);

                foreach (var teacher in roomTeacherViewModels)
                {
                    if (usersTeachers != null)
                    {
                        teacher.NameTeacher = usersTeachers.Where(userT => userT.Id == teacher.IdTeacher).FirstOrDefault().text;
                        teacher.NameGrade = grades.Where(grade => grade.Id == teacher.IdSuject).FirstOrDefault().text;
                    }                   

                }
                if (roomTeacherViewModels != null && roomTeacherViewModels.Count > 0)
                {
                    var model = _mapper.Map<List<RoomTeacherSaveViewModel>>(roomTeacherViewModels);

                    return Json(model);
                }

                else
                {
                    return Json(new List<RoomTeacherSaveViewModel>());
                }
            }

            catch (Exception ex)
            {
                return Json("");
            }

        }

        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? idRoom, int idInstitu, int? idRoomTeacher)
        {
            var userName = User.Identity.Name;
            var userCurrent = await _userService.GetUserByName(userName);

            var room = await _roomService.GetByIdSaveViewModel((int)idRoom);

            if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
            {
                return PartialView("");
            }

            ViewBag.Teachers = await GetTeachers(idRoom ?? 0);
            ViewBag.Sujects = await GetSujects(idInstitu); ;

            if (idRoomTeacher != null && idRoomTeacher > 0)
            {
                var gradeViewModels = await _roomTeacherService.GetBy(grade =>
                 grade.Id == idRoomTeacher);

                if (gradeViewModels != null && gradeViewModels.Count > 0)
                {
                    var model = _mapper.Map<RoomTeacherSaveViewModel>(gradeViewModels.FirstOrDefault());

                    return PartialView(model);
                }
            }

            return PartialView(new RoomTeacherSaveViewModel());

        }

        [HttpPost]
        public async Task<JsonResult> CreateUpdatePost(RoomTeacherSaveViewModel model, int idRoom)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var room = await _roomService.GetByIdSaveViewModel(idRoom);

                    var userName = User.Identity.Name;
                    var userCurrent = await _userService.GetUserByName(userName);

                    if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Json(new { message = "Usuarios no encontrados" });
                    }
                    var roomTeacherViewModels = await _roomTeacherService.GetBy(grade =>
                        grade.IdSuject == model.IdSuject && grade.IdTeacher  == model.IdTeacher && grade.IdRoom == idRoom);
                    if(roomTeacherViewModels.Count > 0)
                    {
                        return Json(new { data = "La asignatura seleccionada ya está asignada a ese profesor.", result = false });
                    }

                    if (model.Id > 0)
                    {
                        await _roomTeacherService.Update(model, model.Id);
                    }
                    else
                    {
                        model.IdRoom = idRoom;
                        var grade = await _roomTeacherService.Add(model);
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

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            return Json(
                new
                {
                    data = "Accion no permitida!",
                    result = true
                });

            if (id > 0)
            {
                var roomTeacher = await _roomTeacherService.GetByIdSaveViewModel(id);

                var room = await _roomService.GetByIdSaveViewModel(roomTeacher.IdRoom);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(room.IdEducationalInsti ?? 0, userCurrent);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { message = "Usuarios no encontrados" });
                }
                var roomTeacherViewModels = await _roomTeacherService.GetBy(grade =>
               grade.Id == id);

                await _roomTeacherService.Delete(id);
                return Json(
                new
                {
                    data = "Registro eliminado correctamente!",
                    result = true
                }
             );
            }

            return Json(
                new
                {
                    data = "Registro no encontrado!",
                    result = false
                }
             );
        }


        #region  Private Methos
        private async Task<List<ClassSelected<string>>> GetTeachers(int idRoom)
        {

            var teacherUsers = _userService.GetAllUser().Result.Data;

            // Filtrar solo profesores y aquellos que tienen asignado el IdRoom
            teacherUsers = teacherUsers
                .Where(u => u.Roles.Contains(Roles.Profesor.ToString())
                            && !string.IsNullOrEmpty(u.IdsRoom)
                            && u.IdsRoom.Split(',').Contains(idRoom.ToString()))
                .ToList();

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
        private async Task<List<ClassSelected<int>>> GetSujects(int idInstitu)
        {

            var grades = await _sujectService.GetAllViewModel();
          

            List<ClassSelected<int>> targetResult = new List<ClassSelected<int>>();

            foreach (var grade in grades)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = grade.Id,
                    text = $"{grade.Name}",
                };

                targetResult.Add(classSelected);
            }
            return targetResult;
        }
        #endregion
    }
}
