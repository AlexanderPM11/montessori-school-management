using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Helpers.DataBase;
using MontessoriSystem.Core.Application.Helpers.Date;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using Rotativa.AspNetCore;
using System.Net;
using System;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using MontessoriSystem.Core.Application.Helpers.InitData;

namespace montessoriSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Teacher")]

    public class ReportAttendanceController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IAttendanceService _attendanceService;
        private readonly IStudentServices _studentService;
        private readonly IReportAttendance _reportAttendance;
        private readonly ITypeRegisterService _typeRegisterService;
        private readonly IInitDataHelper _initDataHelper;
        private readonly IDateAndTimeManage _dateAndTimeManage;
        public ReportAttendanceController(IDateAndTimeManage dateAndTimeManage, 
            IUserService userService, IValidateInfoDataBase validateInfoDataBase, 
            IAttendanceService attendanceService, IStudentServices studentServices, 
            IReportAttendance reportAttendance, ITypeRegisterService typeRegisterService, IInitDataHelper initDataHelper)
        {
            _dateAndTimeManage = dateAndTimeManage;
            _userService = userService;
            _validateInfoDataBase = validateInfoDataBase;
            _attendanceService = attendanceService;
            _studentService = studentServices;
            _reportAttendance = reportAttendance;
            _typeRegisterService = typeRegisterService;
            _initDataHelper = initDataHelper;
        }
        public async Task<PartialViewResult> Index()
        {
            (string action, string controller) = await _initDataHelper.ValidatedInitData();

            if (action != null)
            {
                return PartialView(action, controller);
            }
            var models = await _typeRegisterService.GetAllViewModel();
            List<string> months = _dateAndTimeManage.GetMonthsFromJanuaryToCurrent();
            months.Reverse();
            ViewBag.Months = months;
            return PartialView();
        }
        public  async Task<IActionResult> ReportAttendance(int IdInstitu, int IdRoom, int? IdSuject, string Moth, int idRegisterSelect)
        {
            AttendanceViewModelResponse  attendanceViewModelResponse = new AttendanceViewModelResponse();
            try
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);
                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { });
                }

                var typeRegister = await _typeRegisterService.GetAllViewModel();

                //int idTargetRegister = typeRegister.FirstOrDefault(reg => reg.Id == idRegisterSelect).Id;

                attendanceViewModelResponse = await _reportAttendance.GenerateReportAttendance(IdRoom,Moth.ToString());
            }
            catch (Exception ex)
            {
                return PartialView("");
            }

            return new ViewAsPdf("ReportAttend", attendanceViewModelResponse)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A3                
            };

        }
        public async Task<IActionResult> GetPdf(int IdInstitu, int IdRoom, int? IdSuject, string Moth, int idRegisterSelect)
        {
            var pdf = await ReportAttendance(IdInstitu, IdRoom, IdSuject, Moth, idRegisterSelect);
            var pdfResult = pdf as ViewAsPdf;

            if (pdfResult != null)
            {
                var pdfBytes = await pdfResult.BuildFile(ControllerContext);
                return File(pdfBytes, "application/pdf");
            }

            return NotFound();
        }
    }
}
