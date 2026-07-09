using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;
using MontessoriSystem.Core.Application.ViewModels.Room;
using System.Net.Mime;

namespace SmartCampus.Controllers.v1
{
    [Authorize(Roles = "SuperAdmin,Admin,Profesor")]

    public class RoomController : BaseApiController
    {
        private readonly ICustomRoomService _customRoomService;

        public RoomController(ICustomRoomService customRoomService)
        {
            _customRoomService = customRoomService;
        }

        [HttpGet("GetAllRooms")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<AdjuntoViewModel>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRooms()
        {
            GeneralResponse<List<RoomoViewModel>> response = new();
            try
            {
                response = await _customRoomService.GetAllRooms();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("CreateOrUpdate")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrUpdate([FromForm] RoomSaveViewModel vm)
        {
            GeneralResponse<int> response = new();
            try
            {
                response = await _customRoomService.CreateOrUpdate(vm);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }

        }

        [HttpDelete("Delete/{idRoom}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int idRoom)
        {
            GeneralResponse<int> response = new();
            try
            {
                response = await _customRoomService.Delete(idRoom);

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
