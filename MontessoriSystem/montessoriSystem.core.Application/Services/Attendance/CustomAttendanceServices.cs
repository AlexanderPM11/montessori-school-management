using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Attendance;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services.Attendance
{
    public class CustomAttendanceServices: ICustomAttendanceServices
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IRoomService _roomService;
        private readonly IStudentServices _studentService;
        private readonly IUserService _userService;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IMapper _mapper;
        private readonly ISujectService _sujectService;
        private readonly IDateAndTimeManage _dateAndTimeManage;
        private readonly IRoomTeacherService _roomTeacherService;
        private readonly ITypeRegisterService _typeRegisterService;
        private readonly IInitDataHelper _initDataHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string userName;


        public CustomAttendanceServices(IAttendanceService attendanceService,
            IRoomService roomService, IStudentServices studentService,
            IUserService userService, IValidateInfoDataBase validateInfoDataBase, IMapper mapper, ISujectService sujectService,
            IDateAndTimeManage dateAndTimeManage, IRoomTeacherService roomTeacherService,
            ITypeRegisterService typeRegisterService, IInitDataHelper initDataHelper, IHttpContextAccessor httpContextAccessor
)
        {
            _attendanceService = attendanceService;
            _roomService = roomService;
            _studentService = studentService;
            _userService = userService;
            _validateInfoDataBase = validateInfoDataBase;
            _mapper = mapper;
            _sujectService = sujectService;
            _dateAndTimeManage = dateAndTimeManage;
            _roomTeacherService = roomTeacherService;
            _typeRegisterService = typeRegisterService;
            _initDataHelper = initDataHelper;
            _httpContextAccessor = httpContextAccessor;

            userName = _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        }
        public async Task<GeneralResponse<AttendanceSummaryViewModel>> AttendanceInfo(int IdRoom, string date)
        {
            GeneralResponse<AttendanceSummaryViewModel> response = new();

            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                var attendancesVm = await _attendanceService.GetBy(atten =>
                    atten.IdRoom == IdRoom &&
                    atten.Date.Date == DateTime.Parse(date)
                    );


                List<AttendanceViewModel> attendancesVmTarget = new List<AttendanceViewModel>();

                var studentsVm = await _studentService.GetBy(student => student.IdRoom == IdRoom);

                studentsVm = studentsVm.OrderBy(student => student.Lastname).ToList();


                int number = 1;

                foreach (var student in studentsVm)
                {
                    var studentAttendance = attendancesVm.FirstOrDefault(atte => atte.IdStudent == student.Id);

                    if (studentAttendance != null)
                    {
                        studentAttendance.NumberList = number;
                        studentAttendance.FullNameStudent = $"{student.Name} {student.Lastname}";
                        attendancesVmTarget.Add(studentAttendance);
                    }
                    else
                    {
                        AttendanceViewModel attendanceViewModel = new AttendanceViewModel
                        {
                            Date = DateTime.Parse(date),
                            Observation = "",
                            FullNameStudent = $"{student.Name} {student.Lastname}",
                            IdStudent = student.Id,
                            NumberList = number,
                        };
                        attendancesVmTarget.Add(attendanceViewModel);

                    }
                    number++;
                }

                attendancesVmTarget = attendancesVmTarget.OrderBy(student => student.NumberList).ToList();

                var model = new AttendanceSummaryViewModel
                {
                    Attendances = attendancesVmTarget,
                    PresentCount = attendancesVmTarget.Count(a => a.IsPresent == true),
                    DelayCount = attendancesVmTarget.Count(a => a.IsDelay == true),
                    AbsentCount = attendancesVmTarget.Count(a => a.IsAbsent == true),
                    ExcuseCount = attendancesVmTarget.Count(a => a.IsExcuse == true),
                    TotalCount = attendancesVmTarget.Count
                };
                response.result = true;
                response.Data = model;
                response.messages.Add("Asistencia obtenida correctamente");

                return response;
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add("Error al obtener la asistencia" + ex.Message);

                return response;
            }

        }
        public async Task<GeneralResponse<string>> UpdateAttendance(RequestAttendanceDTO request)
        {
            GeneralResponse<string> response = new();
            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                //Validar estado
                if (request.Status != "isExcuse" && request.Status != "isAbsent" && request.Status != "isDelay" && request.Status != "isPresent")
                {
                    
                    response.result = false;
                    response.messages.Add("El estado de asistencia no es válido. Debe ser 'presente', 'excusa', 'ausente' o 'tardanza'.");
                    return response;
                }

                var attendancesVm = await _attendanceService.GetBy(
                    atten =>
                    atten.IdRoom == request.IdRoom &&
                    atten.IdStudent == request.IdStudent &&
                    atten.Date.Date == DateTime.Parse(request.DayWeek)
                    );

                var attendance = attendancesVm.FirstOrDefault();

                if (attendance == null)
                {
                    AttendanceSaveViewModel attendanceViewModel = new AttendanceSaveViewModel
                    {
                        Date = DateTime.Parse(request.DayWeek),
                        Observation = request.Observations,
                        IsExcuse = request.Status == "isExcuse",
                        IsAbsent = request.Status == "isAbsent",
                        IsDelay = request.Status == "isDelay",
                        IsPresent = request.Status == "isPresent",
                        IdRoom = request.IdRoom,
                        IdStudent = request.IdStudent,
                    };
                    var attendancesAdd = await _attendanceService.Add(attendanceViewModel);
                }
                else
                {
                    var attendanceViewModel = _mapper.Map<AttendanceSaveViewModel>(attendance);

                    attendanceViewModel.IsExcuse = request.Status == "isExcuse";
                    attendanceViewModel.IsAbsent = request.Status == "isAbsent";
                    attendanceViewModel.IsDelay = request.Status == "isDelay";
                    attendanceViewModel.IsPresent = request.Status == "isPresent";
                    attendanceViewModel.Date = DateTime.Parse(request.DayWeek);
                    attendanceViewModel.Observation = request.Observations;

                    await _attendanceService.Update(attendanceViewModel, attendance.Id);
                }
                response.result = true;
                response.messages.Add("Asistencia actualizada correctamente");

                return response;

            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add("Ocurrio un error!.. " + ex.Message);

                return response;
            }

        }
        public async Task<GeneralResponse<string>> DeleteAttendance(int Id)
        {
            GeneralResponse<string> response = new();
            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");
                    return response;
                }

                await _attendanceService.Delete(Id);

                response.result = true;
                response.messages.Add("Asistencia eliminada correctamente");

                return response;

            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add("Ocurrio un error!.. " + ex.Message);

                return response;
            }

        }
        public async Task<GeneralResponse<string>> MakeAllPresent(RequestAttendanceDTO request )
        {
            GeneralResponse<string> response = new();
            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                var attendancesVm = await _attendanceService.GetBy(atten =>
                    atten.IdRoom == request.IdRoom &&
                    atten.Date.Date == DateTime.Parse(request.DayWeek));

                List<AttendanceSaveViewModel> attendancesVmTarget = new List<AttendanceSaveViewModel>();

                var studentsVm = await _studentService.GetBy(student => student.IdRoom == request.IdRoom);

                studentsVm = studentsVm.OrderBy(student => student.Lastname).ToList();

                foreach (var student in studentsVm)
                {
                    var studentAttendance = attendancesVm.FirstOrDefault(atte => atte.IdStudent == student.Id);

                    AttendanceSaveViewModel attendanceViewModel = new AttendanceSaveViewModel
                    {
                        Date = DateTime.Parse(request.DayWeek),
                        IdStudent = student.Id,
                        IdSuject = request.IdSubject,
                        IdRoom = request.IdRoom,
                        Id = studentAttendance != null ? studentAttendance.Id : 0,
                    };
                    attendancesVmTarget.Add(attendanceViewModel);
                }

                var result = await _attendanceService.MakeAllPresent(attendancesVmTarget);

                if (!result.result)
                {
                    response.result = false;
                    response.messages.Add(result.messages.FirstOrDefault() ?? "Error al marca todos los estudiantes presentes.");
                    return response;
                }

                response.result = true;
                response.messages.Add("Todos los estudiantes han sido marcados como presentes.");

                return response;
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add("Ocurrio un error!.. " + ex.Message);

                return response;
            }

        }



    }
}
