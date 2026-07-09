using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.General;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;
using MontessoriSystem.Core.Domain.Entities;
using System.Net.Mime;

namespace SmartCampus.Controllers.v1
{
    public class AdjuntoController : BaseApiController
    {
        private readonly IAdjuntoServices _adjuntoServices;
        private readonly ICustAdjuntoService _adjuntoService;


        public AdjuntoController(IAdjuntoServices  adjuntoServices, ICustAdjuntoService adjuntoService)
        {
            _adjuntoServices = adjuntoServices;
            _adjuntoService = adjuntoService;
        }

        [Authorize(Roles = "SuperAdmin,Admin,Profesor")]
        [HttpGet("GetAdjunto/{idStudent}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<AdjuntoViewModel>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdjunto(int idStudent)
        {
            GeneralResponse<List<AdjuntoViewModel>> response = new();
            try
            {
                response = await _adjuntoService.GetAdjunto(idStudent);               

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
        public async Task<IActionResult> CreateOrUpdate([FromForm] SaveAdjuntoViewModel model)
        {
            GeneralResponse<int> response = new();
            try
            {
                response = await _adjuntoService.CreateOrUpdate(model);

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
        [HttpDelete("Delete/{idAdjunto}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int idAdjunto)
        {
            GeneralResponse<int> response = new();
            try
            {
                response= await _adjuntoService.Delete(idAdjunto);

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
