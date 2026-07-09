using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Reports;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.AchievementIndicators;
using MontessoriSystem.Core.Application.ViewModels.Suject;
using MontessoriSystem.Core.Domain.Entities;
using Rotativa;
using Rotativa.AspNetCore;
using System.Net.Mime;

namespace SmartCampus.Controllers.v1
{
    public class ReportsController : BaseApiController
    {
        private readonly IReportsCustomService _reportsCustomService;
        public ReportsController(IReportsCustomService iReportsCustomService)
        {
            _reportsCustomService = iReportsCustomService;
        }

        #region Get Methods

        [HttpGet("GetDataReport")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<AchievementIndicatorStatusViewModelResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDataReport([FromQuery] int IdStudent, [FromQuery] string Period, [FromQuery] int IdSubject)
        {
            GeneralResponse<AchievementIndicatorStatusViewModelResponse> response = new();
            try
            {
                (var data, string view) = await _reportsCustomService.GetDataReport(IdStudent, Period, IdSubject);

                response = data;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpGet("GetReportPDF")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<AchievementIndicatorStatusViewModelResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReportPDF([FromQuery] int IdStudent, [FromQuery] string Period, [FromQuery] int IdSubject, [FromQuery] bool Preview = true)
        {
            try
            {
               var response = await GeneratePdfBase64Async( IdStudent, Period, IdSubject, Preview);
               return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("GetSubjectsByTeacher")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<SujectViewModel>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubjectsByTeacher([FromQuery] string IdTeacher, [FromQuery] int IdRoom)
        {
            GeneralResponse<List<ClassSelected<int>>> response = new();
            try
            {
                var subjects = await _reportsCustomService.GetSubjectsByTeacher(IdTeacher, IdRoom);

                response.Data = subjects;
                response.messages.Add("Subjects retrieved successfully.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        #endregion

        #region Post Methods

        [HttpPost("UpdateEvaluation")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<SujectViewModel>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEvaluation(RequestReportDTO request)
        {
            GeneralResponse<string> response = new();
            try
            {
                var data = await _reportsCustomService.UpdateEvaluationInitial(request.IdAchievementIndicator, request.Estado,
                    request.IdStudent, request.Period);

                response = data;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("UpdateCalification")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<SujectViewModel>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCalification(RequestReportDTO request)
        {
            GeneralResponse<string> response = new();
            try
            {
                var data = await _reportsCustomService.UpdateCalification(request.IdAchievementIndicator, request.Score,
                    request.Period, request.IdStudent, request.IdSubject, request.isRp, request.rp);

                response = data;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("SaveComments")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveComments(SaveCommentsRequestDTO request)
        {
            GeneralResponse<string> response = new();
            try
            {
                response = await _reportsCustomService.SaveComments(request.IdStudent, request.Comment1,
                    request.Comment2, request.Period);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        #endregion

        #region Delete Methods
        
        [HttpDelete("DeleteCalification")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCalification([FromQuery] int idAchievementIndicator, [FromQuery] int idStudent, [FromQuery] int selectedGradeId,
        [FromQuery] string period)
        {
            GeneralResponse<string> response = new();
            try
            {
                response = await _reportsCustomService.DeleteCalification(idAchievementIndicator, period, idStudent, selectedGradeId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete("DeleteEvaluation")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEvaluation([FromQuery] int idAchievementIndicator, [FromQuery] int idStudent, [FromQuery] string period)
        {
            GeneralResponse<string> response = new();
            try
            {
                var data = await _reportsCustomService.DeleteEvaluationInitial(
                    idAchievementIndicator,
                    idStudent,
                    period
                );

                response = data;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        #endregion

        #region Private Methods
        private async Task<GeneralResponse<AchievementIndicatorStatusViewModelResponse>> GeneratePdfBase64Async(int IdStudent,string Period,int IdSubject, bool Preview = true)
        {
            GeneralResponse<AchievementIndicatorStatusViewModelResponse> response = new();
            try
            {
                (response, string view, string fullNameStudent) = await _reportsCustomService.Report(
                    IdStudent, null, preview: Preview, period: Period, IdSubject: IdSubject);

                var templateView = $"~/Views/{view}.cshtml";

                // Crear el PDF
                var viewAsPdf = new ViewAsPdf(templateView, response.Data)
                {
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0),
                    CustomSwitches = "--disable-smart-shrinking"
                };

                // Generar el PDF como byte[]
                var pdfBytes = await viewAsPdf.BuildFile(ControllerContext);
                string base64Pdf = Convert.ToBase64String(pdfBytes);

                // Asignar el Base64 al modelo de respuesta
                response.Data.Base64Preview = base64Pdf;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating PDF: " + ex.Message);
            }
        }
        
        #endregion

    }
}
