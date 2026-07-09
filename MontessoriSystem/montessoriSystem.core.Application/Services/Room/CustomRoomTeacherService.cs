using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Application.ViewModels.Room;
using MontessoriSystem.Core.Application.ViewModels.RoomSchedule;
using MontessoriSystem.Core.Application.ViewModels.RoomTeacher;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services.Room
{
    public class CustomRoomTeacherService: ICustomRoomTeacherService
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

        private string userName;

        public CustomRoomTeacherService(IRoomService roomService,
            IUserService userService, IMapper mapper, IFileServices fileServices,
            IWebHostEnvironment webHostEnvironment, ISujectService sujectService,
            IRoomScheduleService roomScheduleService, IValidateInfoDataBase validateInfoDataBase, IRoomTeacherService roomTeacherService,
            ITypeRegisterService typeRegisterService, IInitDataHelper initDataHelper, IHttpContextAccessor httpContextAccessor)
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
        }
        public async Task<GeneralResponse<List<RoomTeacherViewModel>>> GetAllRoomTeacher(int idRoom)
        {
            GeneralResponse<List<RoomTeacherViewModel>> response = new();

            try
            {

                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permiso para visualizar estos datos");
                    return response;
                }

                var room = await _roomService.GetByIdSaveViewModel((int)idRoom);

                var roomTeacherViewModels = await _roomTeacherService.GetBy(grade =>
                grade.IdRoom == idRoom);

                List<ClassSelected<string>> usersTeachers = await GetTeachers(idRoom);
                List<ClassSelected<int>> grades = await GetSujects();

                foreach (var teacher in roomTeacherViewModels)
                {
                    if (usersTeachers != null)
                    {
                        teacher.NameTeacher = usersTeachers.Where(userT => userT.Id == teacher.IdTeacher).FirstOrDefault()?.text;
                        teacher.NameGrade = grades.Where(grade => grade.Id == teacher.IdSuject).FirstOrDefault()?.text;
                    }
                }

                response.Data = roomTeacherViewModels;
                response.messages.Add("Consulta exitosa");
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
        public async Task<GeneralResponse<int>> CreateUpdate(RoomTeacherSaveViewModel model)
        {
            GeneralResponse<int> response = new();

            try
            {
                var userCurrent = await _userService.GetUserByName(userName);
                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permiso para crear/actualizar este servicio");
                    return response;
                }

                var roomTeacherViewModels = await _roomTeacherService.GetBy(grade =>
                    grade.IdSuject == model.IdSuject && grade.IdTeacher == model.IdTeacher && grade.IdRoom == model.IdRoom);
                
                if (roomTeacherViewModels.Count > 0)
                {
                    response.result = false;
                    response.messages.Add("La asignatura seleccionada ya está asignada a ese profesor en esta sala.");
                    return response;
                }

                if (model.Id > 0)
                {
                    await _roomTeacherService.Update(model, model.Id);
                    response.messages.Add("Actualización exitosa");
                }
                else
                {
                    model.IdRoom = model.IdRoom;
                    var grade = await _roomTeacherService.Add(model);
                    model.Id = grade.Id;
                    response.messages.Add("Creación exitosa");
                }

                response.Data = model.Id;
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
        public async Task<GeneralResponse<int>> Delete(int id)
        {
            GeneralResponse<int> response = new();

            try
            {
                if (id > 0)
                {
                    var userCurrent = await _userService.GetUserByName(userName);
                    if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                    {
                        response.result = false;
                        response.messages.Add("No tiene permiso para visualizar estos datos");
                        return response;
                    }

                    await _roomTeacherService.Delete(id);

                    response.Data = id;
                    response.messages.Add("Asignatura eliminada correctamente!");
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

            response.result = false;
            response.messages.Add("Asignatura no encontrada!");
            return response;

        }



        #region Private Methods
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
        private async Task<List<ClassSelected<int>>> GetSujects()
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
