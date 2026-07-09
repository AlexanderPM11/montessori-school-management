using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Suject;
using System.Net.Mime;

namespace SmartCampus.Controllers.v1
{
    [Authorize(Roles = "SuperAdmin,Admin,Profesor")]

    public class SubjectController : BaseApiController
    {
        private readonly ICustomSujectService _customSujectService;
        public SubjectController(ICustomSujectService customSujectService)
        {
            _customSujectService = customSujectService;
        }

        [HttpGet("GetAllSubject")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<SujectViewModel>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSubject()
        {
            GeneralResponse<List<SujectViewModel>> response = new();
            try
            {
                response = await _customSujectService.GetAllSubject();

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
        public async Task<IActionResult> CreateOrUpdate([FromForm] SujectSaveViewModel model)
        {
            GeneralResponse<int> response = new();
            try
            {
                response = await _customSujectService.CreateUpdate(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }
        [HttpDelete("Delete/{idSubject}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int idSubject)
        {
            GeneralResponse<int> response = new();
            try
            {
                response = await _customSujectService.Delete(idSubject);

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
