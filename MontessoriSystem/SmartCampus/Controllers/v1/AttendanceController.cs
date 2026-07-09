using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Attendance;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using MontessoriSystem.Core.Application.ViewModels.Student;
using System.Net.Mime;

namespace SmartCampus.Controllers.v1
{
    [Authorize(Roles = "SuperAdmin,Admin,Profesor")]
    public class AttendanceController : BaseApiController
    {
        ICustomAttendanceServices _customAttendanceServices;
        public AttendanceController(ICustomAttendanceServices customAttendanceServices)
        {
            _customAttendanceServices = customAttendanceServices;
        }

        [HttpGet("AttendanceInfo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<AttendanceSummaryViewModel>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AttendanceInfo([FromQuery] int IdRoom, [FromQuery] string DayWeek)
        {
            GeneralResponse<AttendanceSummaryViewModel> response = new();

            try
            {
                response = await _customAttendanceServices.AttendanceInfo(IdRoom,DayWeek);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("UpdateAttendance")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<string>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAttendance(RequestAttendanceDTO request)
        {
            GeneralResponse<string> response = new();

            try
            {
                response = await _customAttendanceServices.UpdateAttendance(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }
        [HttpDelete("DeleteAttendance/{IdAttendance}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<string>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAttendance(int IdAttendance)
        {
            GeneralResponse<string> response = new();

            try
            {
                response = await _customAttendanceServices.DeleteAttendance(IdAttendance);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("MakeAllPresent")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<string>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MakeAllPresent(RequestAttendanceDTO request)
        {
            GeneralResponse<string> response = new();

            try
            {
                response = await _customAttendanceServices.MakeAllPresent(request);

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
