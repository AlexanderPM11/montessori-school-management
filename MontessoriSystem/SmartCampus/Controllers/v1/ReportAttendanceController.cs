using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services.Reports;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using Rotativa.AspNetCore;
using System.Net;
using System.Net.Mime;

namespace SmartCampus.Controllers.v1
{
    [Authorize(Roles = "SuperAdmin,Admin,Profesor")]

    public class ReportAttendanceController : BaseApiController
    {
        private readonly IReportAttendance _reportAttendance;
        public ReportAttendanceController(IReportAttendance reportAttendance)
        {
            _reportAttendance = reportAttendance;
        }
        [HttpGet("GetPdf")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<string>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPdf([FromQuery] int IdRoom, [FromQuery] string Date)
        {
            GeneralResponse<string> response = new();
            try
            {
                var attendanceViewModelResponse = await _reportAttendance.GenerateReportAttendance(IdRoom, Date);

                var pdf = new ViewAsPdf("~/Views/ReportAttendance/ReportAttendance.cshtml", attendanceViewModelResponse)
                {
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                    PageSize = Rotativa.AspNetCore.Options.Size.A3
                };

                var pdfResult = pdf;

                if (pdfResult != null)
                {
                    var pdfBytes = await pdfResult.BuildFile(ControllerContext);
                    string base64Pdf = Convert.ToBase64String(pdfBytes);
                    response.Data = base64Pdf;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            } 

        }
        
        [HttpGet("GetPdfStudent")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<string>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPdfStudent([FromQuery] int IdStudent, [FromQuery] string Date)
        {
            GeneralResponse<string> response = new();
            try
            {
                var attendanceViewModelResponse = await _reportAttendance.GenerateStudentAttendanceReport(IdStudent, Date);

                var pdf = new ViewAsPdf("~/Views/ReportAttendance/ReportStudentAttendance.cshtml", attendanceViewModelResponse)
                {
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                    PageSize = Rotativa.AspNetCore.Options.Size.A3
                };

                var pdfResult = pdf;

                if (pdfResult != null)
                {
                    var pdfBytes = await pdfResult.BuildFile(ControllerContext);
                    string base64Pdf = Convert.ToBase64String(pdfBytes);
                    response.Data = base64Pdf;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            } 

        }

    }
}
