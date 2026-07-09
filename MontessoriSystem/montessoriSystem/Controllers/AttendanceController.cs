using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers.InitData;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using System;
using System.Net;

namespace montessoriSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Teacher")]

    public class AttendanceController : Controller
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


        public AttendanceController(IAttendanceService attendanceService,
            IRoomService roomService,  IStudentServices studentService, 
            IUserService userService, IValidateInfoDataBase validateInfoDataBase,  IMapper mapper, ISujectService sujectService, 
            IDateAndTimeManage dateAndTimeManage, IRoomTeacherService roomTeacherService, ITypeRegisterService typeRegisterService,IInitDataHelper initDataHelper
) { 
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
        }

        [HttpGet]
        public async Task<PartialViewResult> Index(int IdInstitu, int IdRoom, string TypeRegisterSelect)
        {
            try
            {
                (string action, string controller) = await _initDataHelper.ValidatedInitData();

                if (action != null)
                {
                    return PartialView(action, controller);
                }

                List<ClassSelected<int>> Sujects = await GetSuject(IdRoom);
                List<string> weekDays = _dateAndTimeManage.GetCurrentOrPreviousWeekDays();
                ViewBag.Sujects = Sujects;
                ViewBag.WeekDays = weekDays;
                ViewBag.TypeRegisterSelect = TypeRegisterSelect;

                return PartialView();
            }
            catch (Exception ex)
            {
                return PartialView("");
            }

        }       
        
        [HttpGet]
        public async Task<PartialViewResult> AttendanceInfo(int IdInstitu, int IdRoom, int IdSuject, string DayWeek, int idRegisterSelect)
        {
            try
            {                

                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    return PartialView("");
                }


                var dateAttendance = _dateAndTimeManage.GetDateFromDayOfWeek(DayWeek);

                var typeRegister = await _typeRegisterService.GetAllViewModel();

                //int idTargetRegister = typeRegister.FirstOrDefault(reg => reg.Id == idRegisterSelect).Id;

                int? targetIdSuject = IdSuject == 0 ? null : IdSuject;
                var attendancesVm = await _attendanceService.GetBy(atten =>
                    atten.IdRoom == IdRoom &&
                    atten.IdSuject == targetIdSuject &&
                    atten.Date.Date == dateAttendance.Date 
                    );


                List<AttendanceViewModel> attendancesVmTarget = new List<AttendanceViewModel>();

                //var studentsVm = await _studentService.GetBy(student => student.IdRoom == IdRoom && student.IdTypeRegister == idTargetRegister);
                var studentsVm = await _studentService.GetBy(student => student.IdRoom == IdRoom );

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
                            Date = dateAttendance,
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

                return PartialView(model);
            }
            catch (Exception ex)
            {
                return PartialView("");
            }

        }                     
        
        [HttpPost]
        public async Task<JsonResult> UpdateAttendance(int IdInstitu, int IdRoom, int IdStudent, string status, string? observations, int IdSuject, string DayWeek)
        {
            try
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                var room =await _roomService.GetByIdSaveViewModel(IdRoom);

                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    return Json(new { });
                }


                //Validar estado
                if (status != "presente" && status != "excusa" && status != "ausente" && status != "tardanza") 
                {
                    return Json(new { });
                }
                var dateAttendance = _dateAndTimeManage.GetDateFromDayOfWeek(DayWeek);

                int? targetIdSuject = IdSuject == 0 ? null : IdSuject;
                var attendancesVm = await _attendanceService.GetBy(
                    atten =>
                    atten.IdRoom == IdRoom &&
                    atten.IdStudent == IdStudent &&
                    atten.IdSuject == targetIdSuject &&
                    atten.Date.Date == dateAttendance.Date
                    );

                var attendance = attendancesVm.FirstOrDefault();

                if (attendance == null)
                {
                    AttendanceSaveViewModel attendanceViewModel = new AttendanceSaveViewModel
                    {
                        Date = dateAttendance,
                        Observation = observations,
                        IsExcuse = status == "excusa",
                        IsAbsent = status == "ausente",
                        IsDelay = status == "tardanza",
                        IsPresent = status == "presente",
                        IdRoom = IdRoom,
                        IdStudent = IdStudent,
                        IdSuject = targetIdSuject,
                    };
                    var attendancesAdd = await _attendanceService.Add(attendanceViewModel);
                    
                }
                else
                {       
                    var attendanceViewModel = _mapper.Map<AttendanceSaveViewModel>(attendance);

                    attendanceViewModel.IsExcuse = status == "excusa";
                    attendanceViewModel.IsAbsent = status == "ausente";
                    attendanceViewModel.IsDelay = status == "tardanza";
                    attendanceViewModel.IsPresent = status == "presente";
                    attendanceViewModel.Date = dateAttendance;
                    attendanceViewModel.Observation = observations;

                    await _attendanceService.Update(attendanceViewModel, attendance.Id);
                }

                return Json(new { data = "Información actualizada correctamente!", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = "Ocurrio un error!.. " + ex.Message, result = false });
            }

        }

        [HttpPost]
        public async Task<JsonResult> MakeAllPresent(int IdInstitu, int IdRoom, int IdSuject, string DayWeek)
        {
            try
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { });
                }
                var dateAttendance = _dateAndTimeManage.GetDateFromDayOfWeek(DayWeek);

                var attendancesVm = await _attendanceService.GetBy(atten =>
                    atten.IdRoom == IdRoom &&
                    atten.IdSuject == IdSuject &&
                    atten.Date.Date == dateAttendance.Date);
                List<AttendanceSaveViewModel> attendancesVmTarget = new List<AttendanceSaveViewModel>();

                var studentsVm = await _studentService.GetBy(student => student.IdRoom == IdRoom);

                studentsVm = studentsVm.OrderBy(student => student.Lastname).ToList();

                foreach (var student in studentsVm)
                {
                    var studentAttendance = attendancesVm.FirstOrDefault(atte => atte.IdStudent == student.Id);

                    AttendanceSaveViewModel attendanceViewModel = new AttendanceSaveViewModel
                    {
                        Date = dateAttendance,
                        IdStudent = student.Id,
                        IdSuject = IdSuject,
                        IdRoom = IdRoom,
                        Id = studentAttendance != null ? studentAttendance.Id : 0,
                    };
                    attendancesVmTarget.Add(attendanceViewModel);
                }

                var result = await _attendanceService.MakeAllPresent(attendancesVmTarget);

                if (!result.result)
                {
                    return Json(new { data = result.messages.FirstOrDefault(), result = false });
                }
                return Json(new { data = "Información actualizada correctamente!", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = "Ocurrio un error!.. " + ex.Message, result = false });
            }

        }

        #region Private
        private async Task<List<ClassSelected<int>>> GetSuject(int IdRoom)
        {

            var subjectViewModels = await _roomTeacherService.GetAllWithIncludeViewModel(new List<string> { "Suject" },suject =>
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
