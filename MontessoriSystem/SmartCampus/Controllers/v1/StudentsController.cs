using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.General;
using MontessoriSystem.Core.Application.DTOS.Student;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Application.ViewModels.User;
using MontessoriSystem.Core.Domain.Entities;
using System.Net.Mime;

namespace SmartCampus.Controllers.v1
{
    [Authorize(Roles = "SuperAdmin,Admin,Profesor")]

    public class StudentsController : BaseApiController
    {
        private readonly ISelectionService _selectionService;
        private readonly IStudentManagementService _studentManagementService;
        private readonly IExcelManager _excelManager;
        private readonly IStudentServices _studentServices;
        public StudentsController(ISelectionService selectionService, IStudentManagementService studentManagementService, IExcelManager excelManager, IStudentServices studentServices)
        {
            _selectionService = selectionService;
            _studentManagementService = studentManagementService;
            _excelManager = excelManager;
            _studentServices = studentServices;
        }

        [HttpGet("GetAllStudents")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<List<StudentViewModel>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllStudents()
        {
            GeneralResponse<List<StudentViewModel>> response = new();

            try
            {
                response = await _studentManagementService.GetAllStudents();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }
        [HttpGet("GetDatosEstudianteByParents/{IdParent}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<List<StudentViewModel>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDatosEstudianteByParents(string IdParent)
        {
            GeneralResponse<List<StudentViewModel>> response = new();

            try
            {
                response = await _studentManagementService.GetDatosEstudianteByParents(IdParent);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("ImportStudent")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportStudent(IFormFile file)
        {
            GeneralResponse<bool> response = new();

            try
            {
                var userName = User.Identity.Name;
                var origin = "";

                var targetResult = await _excelManager.StudentAndParentsImport(file);

                response.Data = targetResult.result;
                response.result = targetResult.result;
                response.messages = targetResult.messages;

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
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrUpdate([FromForm] StudentSaveViewModel vm)
        {
            GeneralResponse<bool> response = new();
            try
            {
                response = await _studentManagementService.CreateUpdateStudent(vm);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }

        }

        [HttpPost("ActiveInactiveStudent/{idStudent}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActiveInactiveStudent(int idStudent)
        {
            var response = new GeneralResponse<bool>();

            // Validación básica del ID
            if (idStudent <= 0)
            {
                response.messages.Add("ID de estudiante inválido");
                return BadRequest(response);
            }

            try
            {
                await _studentServices.ActiveInactiveStudent(idStudent);

                response.messages.Add("Estado del estudiante actualizado correctamente");
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add("Ocurrió un error al procesar la solicitud");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        #region Students Room

        [HttpGet("GetStudentsRoom/{IdRoom}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<List<StudentViewModel>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudentsRoom(int IdRoom)
        {
            GeneralResponse<List<StudentViewModel>> response = new();

            try
            {
                response = await _studentManagementService.GetStudentsRoom(IdRoom);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpGet("GetStudentsToAddRoom/{IdRoom}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<List<StudentViewModel>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudentsToAddRoom(int IdRoom)
        {
            GeneralResponse<List<StudentViewModel>> response = new();

            try
            {
                var userName = User.Identity.Name;

                response = await _studentManagementService.GetStudentsToAddRoom(IdRoom);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("AddStudentRoom")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<List<StudentViewModel>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddStudentRoom(QuitAddStudentRoomDTO vm)
        {
            GeneralResponse<int> response = new();

            try
            {
                response = await _studentManagementService.AddStudentRoom(vm);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("QuitStudentRoom/{IdStudent}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(Task<GeneralResponse<List<StudentViewModel>>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> QuitStudentRoom(int IdStudent)
        {
            GeneralResponse<int> response = new();

            try
            {
                response = await _studentManagementService.QuitStudentRoom(IdStudent);

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
