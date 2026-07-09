using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.AchievementIndicators;
using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using System.Net.Mime;


namespace SmartCampus.Controllers.v1
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EducationalInstituController : BaseApiController
    {
        private readonly ICustomInstitutionService _customInstitutionService;
        public EducationalInstituController(ICustomInstitutionService customInstitutionService)
        {
            _customInstitutionService = customInstitutionService;
        }

        #region Get Methods

        [HttpGet("GetInstituCenter")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<EducationalInstitutionViewModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInstituCenter()
        {
            GeneralResponse<EducationalInstitutionViewModel> response = new();
            try
            {
                response = await _customInstitutionService.GetInstituCenter();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("CreateUpdate")]
        [Consumes("multipart/form-data")] 
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        public async Task<IActionResult> CreateUpdate([FromForm] SaveEducationalInstitutionViewModel vm)

        {
            GeneralResponse<string> response = new();
            try
            {
                response = await _customInstitutionService.CreateUpdate(vm);

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




    }
}
