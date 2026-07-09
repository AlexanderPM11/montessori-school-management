using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.General;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Application.ViewModels.User;
using System.Net.Mime;

namespace SmartCampus.Controllers.v1
{
    [Authorize]
    public class GeneralDataController : BaseApiController
    {
        private readonly ISelectionService _selectionService;

        public GeneralDataController(ISelectionService selectionService)
        {
            _selectionService = selectionService;
        }

        [HttpGet("GetGendersCivStatuNacLevEducRelati")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<GendersCivStatuNacLevEducRelati>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGendersCivStatuNacLevEducRelati()
        {
            GeneralResponse<GendersCivStatuNacLevEducRelati> response = new();
            try
            {
                response = await _selectionService.GendersCivStatuNacLevEducRelati();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }
        [HttpGet("GetProvices")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<ClassSelected<int>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProvices()
        {
            GeneralResponse<List<ClassSelected<int>>> response = new();
            try
            {
                response.Data = await _selectionService.GetProvices();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpGet("GetCivilStatus")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<ClassSelected<int>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCivilStatus()
        {
            GeneralResponse<List<ClassSelected<int>>> response = new();
            try
            {
                response.Data = await _selectionService.GetCivilStatus();
                response.result = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpGet("GetTeachers")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<ClassSelected<string>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTeachers()
        {
            GeneralResponse<List<ClassSelected<string>>> response = new();
            try
            {
                response.Data = await _selectionService.GetTeachers();
                response.result = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        } 
        
        [HttpGet("GetGeneralsUserRoles")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<Dictionary<string, List<ClassSelected<string>>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGeneralsUserRoles()
        {
            var response = new GeneralResponse<Dictionary<string, List<ClassSelected<string>>>>();

            try
            {
                response.Data = await _selectionService.GetGeneralsUserRoles();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("GetTeachersByIdRoom/{IdRoom}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<ClassSelected<string>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTeachers(int IdRoom)
        {
            GeneralResponse<List<ClassSelected<string>>> response = new();
            try
            {
                response.Data = await _selectionService.GetTeachersByIdRoom(IdRoom);
                response.result = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpGet("GetLevels")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<ClassSelected<int>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLevels()
        {
            GeneralResponse<List<ClassSelected<int>>> response = new();
            try
            {
                response.Data = await _selectionService.GetLevels();
                response.result = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpGet("GetPeriods/{isPrimaria}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<ClassSelected<string>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPeriods(bool isPrimaria = false)
        {
            GeneralResponse<List<ClassSelected<string>>> response = new();
            try
            {
                response.Data = await _selectionService.GetPeriods(isPrimaria);
                response.result = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }
    
        [HttpGet("GetDaysOfWeek")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDaysOfWeek()
        {
            GeneralResponse<List<string>> response = new();
            try
            {
                response = await _selectionService.GetDaysOfWeek();
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
