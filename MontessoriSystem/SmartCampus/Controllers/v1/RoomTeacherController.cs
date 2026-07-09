using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services.Room;
using MontessoriSystem.Core.Application.Services.Suject;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;
using MontessoriSystem.Core.Application.ViewModels.Room;
using MontessoriSystem.Core.Application.ViewModels.RoomTeacher;
using MontessoriSystem.Core.Application.ViewModels.Suject;
using System.Net.Mime;

namespace SmartCampus.Controllers.v1
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class RoomTeacherController : BaseApiController
    {
        private readonly ICustomRoomTeacherService _customRoomTeacherService;
        public RoomTeacherController(ICustomRoomTeacherService customRoomTeacherService)
        {
            _customRoomTeacherService = customRoomTeacherService;
        }
        [HttpGet("GetAllRoomTeacher/{idRoom}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<RoomTeacherViewModel>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRoomTeacher(int idRoom)
        {
            GeneralResponse<List<RoomTeacherViewModel>> response = new();
            try
            {
                response = await _customRoomTeacherService.GetAllRoomTeacher(idRoom);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }
        
        [HttpPost("CreateOrUpdate")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrUpdate([FromForm] RoomTeacherSaveViewModel model)
        {
            GeneralResponse<int> response = new();
            try
            {
                response = await _customRoomTeacherService.CreateUpdate(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpDelete("Delete/{idsubjectTeacher}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int idsubjectTeacher)
        {
            GeneralResponse<int> response = new();
            try
            {
                response = await _customRoomTeacherService.Delete(idsubjectTeacher);

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
