using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers.Date;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Helpers.NewFolder
{
    public class ReportAttendanceHelper: IReportAttendance
    {
        private readonly IUserService _userService;
        private readonly IAttendanceService _attendanceService;
        private readonly IStudentServices _studentService;
        private readonly IDateAndTimeManage _dateAndTimeManage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string userName;
        public ReportAttendanceHelper(IDateAndTimeManage dateAndTimeManage,
            IUserService userService, IValidateInfoDataBase validateInfoDataBase,
            IAttendanceService attendanceService, IStudentServices studentServices, IHttpContextAccessor httpContextAccessor)
        {
            _dateAndTimeManage = dateAndTimeManage;
            _userService = userService;
            _attendanceService = attendanceService;
            _studentService = studentServices;
            _httpContextAccessor = httpContextAccessor;
            userName = _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        }
        public async Task<AttendanceViewModelResponse> GenerateReportAttendance(int IdRoom, string Date)
        {
            List<ReportAttendance> reportAttendances = new List<ReportAttendance>();
            AttendanceViewModelResponse attendanceViewModelResponse = new AttendanceViewModelResponse();
            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    throw new Exception("No tiene permisos para realizar esta acción");
                }

                #region Reporte asisstencia Registro Inicial

                var dateTimeTarget = DateTime.Parse(Date);

                var attendancesVm = await _attendanceService.GetBy(atten => atten.IdRoom == IdRoom &&
                    atten.Date.Date.Year == dateTimeTarget.Year && atten.Date.Date.Month == dateTimeTarget.Month);

                List<AttendanceViewModel> attendancesVmTarget = new List<AttendanceViewModel>();

                //var studentsVm = await _studentService.GetBy(student => student.IdRoom == IdRoom && student.IdTypeRegister  == idTargetRegister);
                var studentsVm = await _studentService.GetBy(student => student.IdRoom == IdRoom );

                studentsVm = studentsVm.OrderBy(student => student.Lastname).ToList();
                int number = 1;
                int totalPresent = 0;
                int totalAusent = 0;
                int dayWorked = 0;

                int indexFirtFiveWeek = 0;
                int indexSecodFiveWeek = 0;
                int indexThirdFiveWeek = 0;
                int indexFourtFiveWeek = 0;
                int indexFiveWeek = 0;

                var month = dateTimeTarget.Month;
                var year = dateTimeTarget.Year;

                foreach (var student in studentsVm)
                {
                    #region Day week status, P,A,E,T

                    WeekDaysReportAttendance weekDaysReportAttendance = new WeekDaysReportAttendance();

                    #region FirstWeekDetails

                    var firstFiveWeekdays = _dateAndTimeManage.GetWeekdaysOfWeek(year, month, 1);

                    foreach (var day in firstFiveWeekdays)
                    {

                        var studentAttendance = attendancesVm.FirstOrDefault(
                            atte => atte.Date.Date == day.Date
                        );

                        if (studentAttendance != null)
                        {
                            var studentAsist = attendancesVm.FirstOrDefault(
                            atte => atte.IdStudent == student.Id && atte.Date.Date == day.Date
                            );

                            dayWorked++;

                            if (studentAsist != null)
                            {

                                if ((bool)studentAsist.IsPresent!)
                                {

                                    if (int.Parse(student.Sexo) == 1)
                                    {
                                        if (attendanceViewModelResponse.FirstWeekmale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.FirstWeekmale[indexFirtFiveWeek];
                                            attendanceViewModelResponse.FirstWeekmale[indexFirtFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            attendanceViewModelResponse.FirstWeekmale.Add(1);
                                            if (attendanceViewModelResponse.FirstWeekFemale.Count != 5)
                                            {
                                                attendanceViewModelResponse.FirstWeekFemale.Add(0);
                                            }
                                               
                                        }

                                    }
                                    else
                                    {
                                        if (attendanceViewModelResponse.FirstWeekFemale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.FirstWeekFemale[indexFirtFiveWeek];
                                            attendanceViewModelResponse.FirstWeekFemale[indexFirtFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            if (attendanceViewModelResponse.FirstWeekmale.Count != 5)
                                            {
                                                attendanceViewModelResponse.FirstWeekmale.Add(0);
                                            }
                                            attendanceViewModelResponse.FirstWeekFemale.Add(1);
                                        }
                                    }

                                    weekDaysReportAttendance.FirstWeekDetails.Add("P");
                                    totalPresent++;

                                }
                                else
                                {
                                    if (attendanceViewModelResponse.FirstWeekmale.Count < 5)
                                    {
                                        attendanceViewModelResponse.FirstWeekmale.Add(0);
                                    }
                                    if (attendanceViewModelResponse.FirstWeekFemale.Count < 5)
                                    {
                                        attendanceViewModelResponse.FirstWeekFemale.Add(0);
                                    }

                                }

                                if ((bool)studentAsist.IsAbsent!)
                                {
                                    weekDaysReportAttendance.FirstWeekDetails.Add("A");
                                    totalAusent++;

                                }
                                if ((bool)studentAsist.IsDelay!)
                                {
                                    weekDaysReportAttendance.FirstWeekDetails.Add("T");

                                }
                                if ((bool)studentAsist.IsExcuse!)
                                {
                                    weekDaysReportAttendance.FirstWeekDetails.Add("E");

                                }
                            }
                            else
                            {
                                if (attendanceViewModelResponse.FirstWeekmale.Count < 5)
                                {
                                    attendanceViewModelResponse.FirstWeekmale.Add(0);
                                }
                                if (attendanceViewModelResponse.FirstWeekFemale.Count < 5)
                                {
                                    attendanceViewModelResponse.FirstWeekFemale.Add(0);
                                }

                                totalAusent++;
                                weekDaysReportAttendance.FirstWeekDetails.Add("A");
                            }
                        }
                        else
                        {
                            weekDaysReportAttendance.FirstWeekDetails.Add("-");
                            if (attendanceViewModelResponse.FirstWeekmale.Count < 5)
                            {
                                attendanceViewModelResponse.FirstWeekmale.Add(0);
                            }
                            if (attendanceViewModelResponse.FirstWeekFemale.Count < 5)
                            {
                                attendanceViewModelResponse.FirstWeekFemale.Add(0);
                            }

                        }
                        indexFirtFiveWeek++;
                    }
                    indexFirtFiveWeek = 0;

                    #endregion

                    #region SecondFiveWeekdays
                    var SecondFiveWeekdays = _dateAndTimeManage.GetWeekdaysOfWeek(year, month, 2);

                    foreach (var day in SecondFiveWeekdays)
                    {
                        var studentAttendance = attendancesVm.FirstOrDefault(
                            atte => atte.Date.Date == day.Date
                        );

                        if (studentAttendance != null)
                        {
                            dayWorked++;
                            var studentAsist = attendancesVm.FirstOrDefault(
                            atte => atte.IdStudent == student.Id && atte.Date.Date == day.Date
                            );

                            if (studentAsist != null)
                            {

                                if ((bool)studentAsist.IsPresent!)
                                {
                                    if (int.Parse(student.Sexo) == 1)
                                    {
                                        if (attendanceViewModelResponse.SecondWeekmale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.SecondWeekmale[indexSecodFiveWeek];
                                            attendanceViewModelResponse.SecondWeekmale[indexSecodFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            attendanceViewModelResponse.SecondWeekmale.Add(1);
                                            if (attendanceViewModelResponse.SecondWeekFemale.Count != 5)
                                            {
                                                attendanceViewModelResponse.SecondWeekFemale.Add(0);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (attendanceViewModelResponse.SecondWeekFemale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.SecondWeekFemale[indexSecodFiveWeek];
                                            attendanceViewModelResponse.SecondWeekFemale[indexSecodFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            if (attendanceViewModelResponse.SecondWeekmale.Count != 5)
                                            {
                                                attendanceViewModelResponse.SecondWeekmale.Add(0);
                                            }
                                            attendanceViewModelResponse.SecondWeekFemale.Add(1);
                                        }
                                    }
                                    weekDaysReportAttendance.SecondWeekDetails.Add("P");
                                    totalPresent++;

                                }
                                else
                                {
                                    if (attendanceViewModelResponse.SecondWeekmale.Count < 5)
                                    {
                                        attendanceViewModelResponse.SecondWeekmale.Add(0);
                                    }
                                    if (attendanceViewModelResponse.SecondWeekFemale.Count < 5)
                                    {
                                        attendanceViewModelResponse.SecondWeekFemale.Add(0);
                                    }
                                }
                                if ((bool)studentAsist.IsAbsent!)
                                {
                                    weekDaysReportAttendance.SecondWeekDetails.Add("A");
                                    totalAusent++;

                                }
                                if ((bool)studentAsist.IsDelay!)
                                {
                                    weekDaysReportAttendance.SecondWeekDetails.Add("T");

                                }
                                if ((bool)studentAsist.IsExcuse!)
                                {
                                    weekDaysReportAttendance.SecondWeekDetails.Add("E");

                                }
                            }

                            else
                            {
                                totalAusent++;
                                weekDaysReportAttendance.SecondWeekDetails.Add("A");
                                if (attendanceViewModelResponse.SecondWeekmale.Count < 5)
                                {
                                    attendanceViewModelResponse.SecondWeekmale.Add(0);
                                }
                                if (attendanceViewModelResponse.SecondWeekFemale.Count < 5)
                                {
                                    attendanceViewModelResponse.SecondWeekFemale.Add(0);
                                }
                            }

                        }

                        else
                        {
                            weekDaysReportAttendance.SecondWeekDetails.Add("");
                            if (attendanceViewModelResponse.SecondWeekmale.Count < 5)
                            {
                                attendanceViewModelResponse.SecondWeekmale.Add(0);
                            }
                            if (attendanceViewModelResponse.SecondWeekFemale.Count < 5)
                            {
                                attendanceViewModelResponse.SecondWeekFemale.Add(0);
                            }
                        }

                        indexSecodFiveWeek++;
                    }
                    indexSecodFiveWeek = 0;
                    #endregion

                    #region ThirdFiveWeekdays

                    var ThirdFiveWeekdays = _dateAndTimeManage.GetWeekdaysOfWeek(year, month, 3);

                    foreach (var day in ThirdFiveWeekdays)
                    {
                        var studentAttendance = attendancesVm.FirstOrDefault(
                            atte => atte.Date.Date == day.Date
                        );

                        if (studentAttendance != null)
                        {
                            var studentAsist = attendancesVm.FirstOrDefault(
                            atte => atte.IdStudent == student.Id && atte.Date.Date == day.Date
                            );
                            dayWorked++;
                            if (studentAsist != null)
                            {
                                if ((bool)studentAsist.IsPresent!)
                                {
                                    if (int.Parse(student.Sexo) == 1)
                                    {
                                        if (attendanceViewModelResponse.ThirdWeekmale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.ThirdWeekmale[indexThirdFiveWeek];
                                            attendanceViewModelResponse.ThirdWeekmale[indexThirdFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            if (attendanceViewModelResponse.ThirdWeekFemale.Count != 5)
                                            {
                                                attendanceViewModelResponse.ThirdWeekFemale.Add(0);
                                            }
                                            attendanceViewModelResponse.ThirdWeekmale.Add(1);
                                        }

                                    }
                                    else
                                    {
                                        if (attendanceViewModelResponse.ThirdWeekFemale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.ThirdWeekFemale[indexThirdFiveWeek];
                                            attendanceViewModelResponse.ThirdWeekFemale[indexThirdFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            if (attendanceViewModelResponse.ThirdWeekmale.Count != 5)
                                            {
                                                attendanceViewModelResponse.ThirdWeekmale.Add(0);
                                            }
                                            attendanceViewModelResponse.ThirdWeekFemale.Add(1);
                                        }
                                    }
                                    weekDaysReportAttendance.ThirdWeekDetails.Add("P");
                                    totalPresent++;

                                }
                                else
                                {
                                    if (attendanceViewModelResponse.ThirdWeekFemale.Count < 5)
                                    {
                                        attendanceViewModelResponse.ThirdWeekFemale.Add(0);
                                    }
                                    if (attendanceViewModelResponse.ThirdWeekmale.Count < 5)
                                    {
                                        attendanceViewModelResponse.ThirdWeekmale.Add(0);
                                    }
                                }
                                if ((bool)studentAsist.IsAbsent!)
                                {
                                    weekDaysReportAttendance.ThirdWeekDetails.Add("A");
                                    totalAusent++;

                                }
                                if ((bool)studentAsist.IsDelay!)
                                {
                                    weekDaysReportAttendance.ThirdWeekDetails.Add("T");

                                }
                                if ((bool)studentAsist.IsExcuse!)
                                {
                                    weekDaysReportAttendance.ThirdWeekDetails.Add("E");

                                }
                            }
                            else
                            {
                                if (attendanceViewModelResponse.ThirdWeekFemale.Count < 5)
                                {
                                    attendanceViewModelResponse.ThirdWeekFemale.Add(0);
                                }
                                if (attendanceViewModelResponse.ThirdWeekmale.Count < 5)
                                {
                                    attendanceViewModelResponse.ThirdWeekmale.Add(0);
                                }
                                weekDaysReportAttendance.ThirdWeekDetails.Add("A");
                                totalAusent++;
                            }

                        }
                        else
                        {
                            weekDaysReportAttendance.ThirdWeekDetails.Add("");
                            if (attendanceViewModelResponse.ThirdWeekFemale.Count < 5)
                            {
                                attendanceViewModelResponse.ThirdWeekFemale.Add(0);
                            }
                            if (attendanceViewModelResponse.ThirdWeekmale.Count < 5)
                            {
                                attendanceViewModelResponse.ThirdWeekmale.Add(0);
                            }
                        }
                        indexThirdFiveWeek++;
                    }
                    indexThirdFiveWeek = 0;
                    #endregion

                    #region FourFiveWeekdays

                    var FourFiveWeekdays = _dateAndTimeManage.GetWeekdaysOfWeek(year, month, 4);

                    foreach (var day in FourFiveWeekdays)
                    {
                        var studentAttendance = attendancesVm.FirstOrDefault(
                            atte => atte.Date.Date == day.Date
                        );

                        if (studentAttendance != null)
                        {
                            var studentAsist = attendancesVm.FirstOrDefault(
                            atte => atte.IdStudent == student.Id && atte.Date.Date == day.Date
                            );
                            dayWorked++;
                            if (studentAsist != null)
                            {
                                if ((bool)studentAsist.IsPresent!)
                                {
                                    if (int.Parse(student.Sexo) == 1)
                                    {
                                        if (attendanceViewModelResponse.FourWeekmale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.FourWeekmale[indexFourtFiveWeek];
                                            attendanceViewModelResponse.FourWeekmale[indexFourtFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            if (attendanceViewModelResponse.FourWeekFemale.Count != 5)
                                            {
                                                attendanceViewModelResponse.FourWeekFemale.Add(0);
                                            }
                                            attendanceViewModelResponse.FourWeekmale.Add(1);
                                        }

                                    }
                                    else
                                    {
                                        if (attendanceViewModelResponse.FourWeekFemale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.FourWeekFemale[indexFourtFiveWeek];
                                            attendanceViewModelResponse.FourWeekFemale[indexFourtFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            if (attendanceViewModelResponse.FourWeekmale.Count != 5)
                                            {
                                                attendanceViewModelResponse.FourWeekmale.Add(0);
                                            }
                                            attendanceViewModelResponse.FourWeekFemale.Add(1);
                                        }
                                    }
                                    weekDaysReportAttendance.FourWeekDetails.Add("P");
                                    totalPresent++;

                                }

                                else
                                {
                                    if (attendanceViewModelResponse.FourWeekFemale.Count < 5)
                                    {
                                        attendanceViewModelResponse.FourWeekFemale.Add(0);
                                    }
                                    if (attendanceViewModelResponse.FourWeekmale.Count < 5)
                                    {
                                        attendanceViewModelResponse.FourWeekmale.Add(0);
                                    }
                                }

                                if ((bool)studentAsist.IsAbsent!)
                                {
                                    weekDaysReportAttendance.FourWeekDetails.Add("A");
                                    totalAusent++;
                                }
                                if ((bool)studentAsist.IsDelay!)
                                {
                                    weekDaysReportAttendance.FourWeekDetails.Add("T");
                                }
                                if ((bool)studentAsist.IsExcuse!)
                                {
                                    weekDaysReportAttendance.FourWeekDetails.Add("E");

                                }
                            }
                            else
                            {
                                if (attendanceViewModelResponse.FourWeekFemale.Count < 5)
                                {
                                    attendanceViewModelResponse.FourWeekFemale.Add(0);
                                }
                                if (attendanceViewModelResponse.FourWeekmale.Count < 5)
                                {
                                    attendanceViewModelResponse.FourWeekmale.Add(0);
                                }
                                weekDaysReportAttendance.FourWeekDetails.Add("A");
                                totalAusent++;
                            }

                        }

                        else
                        {
                            if (attendanceViewModelResponse.FourWeekFemale.Count < 5)
                            {
                                attendanceViewModelResponse.FourWeekFemale.Add(0);
                            }
                            if (attendanceViewModelResponse.FourWeekmale.Count < 5)
                            {
                                attendanceViewModelResponse.FourWeekmale.Add(0);
                            }
                            weekDaysReportAttendance.FourWeekDetails.Add("");
                        }
                        indexFourtFiveWeek++;
                    }
                    indexFourtFiveWeek = 0;
                    #endregion

                    #region FiveWeekdays
                    var FiveWeekdays = _dateAndTimeManage.GetWeekdaysOfWeek(year, month, 5);

                    foreach (var day in FiveWeekdays)
                    {
                        var studentAttendance = attendancesVm.FirstOrDefault(
                            atte => atte.Date.Date == day.Date
                        );

                        if (studentAttendance != null)
                        {

                            var studentAsist = attendancesVm.FirstOrDefault(
                            atte => atte.IdStudent == student.Id && atte.Date.Date == day.Date
                            );
                            dayWorked++;

                            if (studentAsist != null)
                            {
                                if ((bool)studentAsist.IsPresent!)
                                {
                                    if (int.Parse(student.Sexo) == 1)
                                    {
                                        if (attendanceViewModelResponse.FiveWeekmale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.FiveWeekmale[indexFiveWeek];
                                            attendanceViewModelResponse.FiveWeekmale[indexFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            attendanceViewModelResponse.FiveWeekmale.Add(1);
                                            if (attendanceViewModelResponse.FiveWeekFemale.Count != 5)
                                            {
                                                attendanceViewModelResponse.FiveWeekFemale.Add(0);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (attendanceViewModelResponse.FiveWeekFemale.Count == 5)
                                        {
                                            var valueindex = attendanceViewModelResponse.FiveWeekFemale[indexFiveWeek];
                                            attendanceViewModelResponse.FiveWeekFemale[indexFiveWeek] = valueindex + 1;
                                        }
                                        else
                                        {
                                            if (attendanceViewModelResponse.FiveWeekmale.Count != 5)
                                            {
                                                attendanceViewModelResponse.FiveWeekmale.Add(0);
                                            }
                                            attendanceViewModelResponse.FiveWeekFemale.Add(1);
                                        }
                                    }
                                    weekDaysReportAttendance.FiveWeekDetails.Add("P");
                                    totalPresent++;

                                }
                                else
                                {
                                    if (attendanceViewModelResponse.FiveWeekmale.Count < 5)
                                    {
                                        attendanceViewModelResponse.FiveWeekmale.Add(0);
                                    }
                                    if (attendanceViewModelResponse.FiveWeekFemale.Count < 5)
                                    {
                                        attendanceViewModelResponse.FiveWeekFemale.Add(0);
                                    }
                                }
                                if ((bool)studentAsist.IsAbsent!)
                                {
                                    weekDaysReportAttendance.FiveWeekDetails.Add("A");
                                    totalAusent++;

                                }
                                if ((bool)studentAsist.IsDelay!)
                                {
                                    weekDaysReportAttendance.FiveWeekDetails.Add("T");

                                }
                                if ((bool)studentAsist.IsExcuse!)
                                {
                                    weekDaysReportAttendance.FiveWeekDetails.Add("E");

                                }
                            }
                            else
                            {
                                if (attendanceViewModelResponse.FiveWeekmale.Count < 5)
                                {
                                    attendanceViewModelResponse.FiveWeekmale.Add(0);
                                }
                                if (attendanceViewModelResponse.FiveWeekFemale.Count < 5)
                                {
                                    attendanceViewModelResponse.FiveWeekFemale.Add(0);
                                }
                                totalAusent++;
                                weekDaysReportAttendance.FiveWeekDetails.Add("A");
                            }

                        }

                        else
                        {
                            if (attendanceViewModelResponse.FiveWeekmale.Count < 5)
                            {
                                attendanceViewModelResponse.FiveWeekmale.Add(0);
                            }
                            if (attendanceViewModelResponse.FiveWeekFemale.Count < 5)
                            {
                                attendanceViewModelResponse.FiveWeekFemale.Add(0);
                            }
                            weekDaysReportAttendance.FiveWeekDetails.Add("");
                        }
                        indexFiveWeek++;
                    }

                    indexFiveWeek = 0;

                    #endregion

                    #region Assigment and reset values
                    
                    reportAttendances.Add(
                        new ReportAttendance
                        {
                            weekDaysReportAttendance = weekDaysReportAttendance,
                            NumberListStudent = number,
                            TotalAusent = totalAusent,
                            TotalPresent = totalPresent,
                            PercentAisstMonth =  (totalPresent > 0)  ? ((double)totalPresent / dayWorked * 100) : 0,
                        }
                    );

                    attendanceViewModelResponse.DayWorked = dayWorked;

                    // Reset VALUE
                    totalPresent = 0;
                    totalAusent = 0;
                    dayWorked = 0;
                    weekDaysReportAttendance = new WeekDaysReportAttendance();
                   
                    number++;

                    #endregion

                    #endregion
                }

                #region Add dummy data if reportAttendances has less than 30 entries
                while (reportAttendances.Count < 30)
                {
                    WeekDaysReportAttendance dummy = new WeekDaysReportAttendance();
                    // Initialize each list with empty strings
                    for (int i = 0; i < 5; i++)
                    {
                        dummy.FirstWeekDetails.Add("");
                        dummy.SecondWeekDetails.Add("");
                        dummy.ThirdWeekDetails.Add("");
                        dummy.FourWeekDetails.Add("");
                        dummy.FiveWeekDetails.Add("");
                    }
                    reportAttendances.Add(new ReportAttendance
                    {
                        weekDaysReportAttendance = dummy,
                        NumberListStudent = reportAttendances.Count + 1,
                        TotalAusent = 0,
                        TotalPresent = 0,
                    });
                }
                #endregion

                attendanceViewModelResponse.attendanceViews = reportAttendances;
                attendanceViewModelResponse.Year = DateTime.Now.Year;
                attendanceViewModelResponse.Month = dateTimeTarget.ToString("MMMM", 
                    new System.Globalization.CultureInfo("es-ES"));

                #endregion
            }
            catch (Exception)
            {
                throw;
            }
            return attendanceViewModelResponse;
        }

        public async Task<StudentAttendanceViewModelResponse> GenerateStudentAttendanceReport(int studentId, string date)
        {
            StudentAttendanceViewModelResponse response = new StudentAttendanceViewModelResponse();
            
            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    throw new Exception("No tiene permisos para realizar esta acción");
                }

                var dateTimeTarget = DateTime.Parse(date);
                var month = dateTimeTarget.Month;
                var year = dateTimeTarget.Year;

                // Obtener datos del estudiante
                var student = await _studentService.GetByIdSaveViewModel(studentId);
                if (student == null)
                {
                    throw new Exception("Estudiante no encontrado");
                }

                response.StudentName = $"{student.Lastname} {student.Name}";
                response.RoomName = "N/A";
                response.Month = dateTimeTarget.ToString("MMMM",
                    new System.Globalization.CultureInfo("es-ES"));
                response.Year = year;

                // Obtener asistencias del estudiante para el mes
                var attendances = await _attendanceService.GetBy(a =>
                    a.IdStudent == studentId &&
                    a.Date.Year == year &&
                    a.Date.Month == month);

                int totalPresent = 0;
                int totalAbsent = 0;
                int totalExcused = 0;
                int totalDelayed = 0;
                int dayWorked = 0;

                // Procesar cada semana del mes
                for (int week = 1; week <= 5; week++)
                {
                    var weekDays = _dateAndTimeManage.GetWeekdaysOfWeek(year, month, week);
                    var weekData = new WeekAttendanceData();

                    foreach (var day in weekDays)
                    {
                        var attendance = attendances.FirstOrDefault(a => a.Date.Date == day.Date);

                        if (attendance != null)
                        {
                            dayWorked++;
                            string status = "";

                            if ((bool)attendance.IsPresent!)
                            {
                                status = "P";
                                totalPresent++;
                            }
                            else if ((bool)attendance.IsExcuse!)
                            {
                                status = "E";
                                totalExcused++;
                            }
                            else if ((bool)attendance.IsDelay!)
                            {
                                status = "T";
                                totalDelayed++;
                            }
                            else if ((bool)attendance.IsAbsent!)
                            {
                                status = "A";
                                totalAbsent++;
                            }

                            weekData.Days.Add(new DayAttendance
                            {
                                Date = day,
                                Status = status
                            });
                        }
                        else
                        {
                            // Día sin registro de asistencia
                            weekData.Days.Add(new DayAttendance
                            {
                                Date = day,
                                Status = ""
                            });
                        }
                    }

                    response.Weeks.Add(weekData);
                }

                response.DayWorked = dayWorked;
                response.TotalPresent = totalPresent;
                response.TotalAbsent = totalAbsent;
                response.TotalExcused = totalExcused;
                response.TotalDelayed = totalDelayed;
                response.Percentage = dayWorked > 0 ? (double)totalPresent / dayWorked * 100 : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar reporte de asistencia del estudiante", ex);
            }

            return response;
        }
    }
}
