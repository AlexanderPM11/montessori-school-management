using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Application.ViewModels.AchievementIndicators;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Core.Domain.Settings;
using MontessoriSystem.Core.Domain.Settings.Grades;
using Rotativa.AspNetCore;
using System;

namespace montessoriSystem.Controllers
{
    public class ReportsController : Controller
    {
       
        private readonly IReportsCustomService _reportsCustomService;
        private readonly IStudentServices _studentServices;
        private readonly EducationalPeriod _educationalPeriod;
        private readonly IUserService _userService;
        public ReportsController(IReportsCustomService reportsCustomService, IOptions<EducationalPeriod> educationalPeriod, IStudentServices studentServices, IUserService userService)
        {
            _reportsCustomService = reportsCustomService;
            _educationalPeriod = educationalPeriod.Value;
            _studentServices = studentServices;
            _userService = userService;
        }

        public async Task< IActionResult> Index()
        {
            var userName = User.Identity.Name;

            var (response, view, fullNameStudent) = await _reportsCustomService.Report(1, userName);

            return new ViewAsPdf("Secundaria_Primero/SecundariaPrimeroTemplate", response.Data)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.Letter, // Tamaño Letter 8.5 x 11 in
                PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0), // Eliminar márgenes
                CustomSwitches = "--disable-smart-shrinking"
            };
        }

        public async Task<IActionResult> ReportsView(int IdStudent, string Period,int IdSubject)
        {
            try
            {
                var userName = User.Identity.Name;

                var (response,view, fullNameStudent) = await _reportsCustomService.Report(IdStudent, userName, period: Period,IdSubject: IdSubject);

                if (!response.result)
                {
                    return Json(new { success = false, message = response.messages.FirstOrDefault() });
                }

                ViewBag.FullNameStudent = fullNameStudent;

                var pdfResult = new ViewAsPdf(view, response.Data)
                {
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter, // Tamaño Letter 8.5 x 11 in
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0), // Eliminar márgenes
                    CustomSwitches = "--disable-smart-shrinking"
                };

                var pdfBytes = await pdfResult.BuildFile(ControllerContext);
                return File(pdfBytes, "application/pdf");

            }
            catch (Exception ex)
            {
                throw;
            }
           
        }
        public async Task<IActionResult> PreviewReport(int IdStudent, string Period, int IdSubject)
        {
            try
            {
                var userName = User.Identity.Name;

                (var response, string view, string fullNameStudent) = await _reportsCustomService.Report(IdStudent, userName, preview:true, period: Period, IdSubject: IdSubject);

                if (!response.result)
                {
                    return Json(new { success = false, message = response.messages.FirstOrDefault() });
                }

                // Generar PDF con tamaño 8.50 x 11.00 in (Carta), orientación vertical y sin márgenes
                return new ViewAsPdf(view, response.Data)
                {
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter, // Tamaño Letter 8.5 x 11 in
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0), // Eliminar márgenes
                    CustomSwitches = "--disable-smart-shrinking"
                };

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #region Evaluation

        [HttpGet]
        public async Task<IActionResult> GetDataReport(int IdStudent, string period = "1ro. Agosto - Diciembre", int IdSubject = 0)
        {
            (var response, string view) = await _reportsCustomService.GetDataReport(IdStudent, period, IdSubject);
            if (!response.result)
            {
                return Json(new { success = false, message = response.messages.FirstOrDefault() });
            }


            return PartialView(view, response.Data);
        }
        [HttpGet]
        public async Task<PartialViewResult> Evaluation(int IdStudent, int IdRoom)
        {
            var grade = await _reportsCustomService.GetStudentGrade(IdStudent);
            
            if (new HashSet<string> { "Segundo", "Tercero", "Cuarto", "Quinto", "Sexto" }.Contains(grade))
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                var subjects = await _reportsCustomService.GetSubjectsByTeacher(currentUser.Id, IdRoom);
                
                ViewBag.Grade = subjects;
                return PartialView("SecondGrade/Evaluation");
            }

            if (new HashSet<string> { "Primero Secundaria", "Segundo Secundaria", "Tercero Secundaria", "Cuarto Secundaria", "Quinto Secundaria" , "Sexto Secundaria" }.Contains(grade))
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                var subjects = await _reportsCustomService.GetSubjectsByTeacher(currentUser.Id, IdRoom);

                ViewBag.Grade = subjects;
                return PartialView("Secundaria_Primero/Evaluation");
            }

            return PartialView();

        }

        [HttpPost]
        public async Task<IActionResult> UpdateEvaluationInitial(int idAchievementIndicator, string estado, int idStudent, string period)
        {
            try
            {
                var response = await _reportsCustomService.UpdateEvaluationInitial(idAchievementIndicator, estado, idStudent, period);

                return Json(new { success = response.result, message = response.messages.FirstOrDefault() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }  
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvaluationInitial(int idAchievementIndicator, int idStudent, string period)
        {
            try
            {
                var response = await _reportsCustomService.DeleteEvaluationInitial(idAchievementIndicator, idStudent, period);
                return Json(new { success = response.result, message = response.messages.FirstOrDefault() });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }


        }

        [HttpPost]
        public async Task<IActionResult> SaveComments( int IdStudent,string comment1, string comment2, string period)
        {
            try
            {
                var response = await _reportsCustomService.SaveComments(IdStudent, comment1, comment2, period);
                return Json(new { success = response.result, message = response.messages.FirstOrDefault() });               

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCalification(int achievementId, int score, string period, int idStudent, int selectedGradeId, bool isRp = false, int? rp = null)
        {
            try
            {
                var response = await _reportsCustomService.UpdateCalification( achievementId, score, period, idStudent, selectedGradeId, isRp, rp);
                return Json(new { success = response.result, message = response.messages.FirstOrDefault() });               

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCalification( int achievementId,string period, int idStudent, int selectedGradeId)
        {
            try
            {
                var response = await _reportsCustomService.DeleteCalification( achievementId, period, idStudent, selectedGradeId);
                return Json(new { success = response.result, message = response.messages.FirstOrDefault() });              

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion      
    }
}
