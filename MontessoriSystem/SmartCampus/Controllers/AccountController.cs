using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.User;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;


namespace SmartCampus.Controllers
{
   
    [SwaggerTag(description: "Este controlador nos permite realizar el login y demas operaciones relacionadas a los usuarios.")]
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IUserManagementService _userManagementService;
        private readonly IExcelManager _excelManager;
        public AccountController(IUserService userService, IUserManagementService userManagementService, IExcelManager excelManager)
        {
            _userManagementService = userManagementService;
            _userService = userService;
            _excelManager = excelManager;
        }

        #region Authentication and User Management

        [HttpPost("Login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<LoginResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            GeneralResponse<LoginResponse> response = new();
            try
            {
                response = await _userManagementService.Login(model);

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
        public async Task<IActionResult> CreateOrUpdate([FromForm] SaveUserViewModel vm)
        {
            GeneralResponse<bool> response = new();

            try
            {
                var origin = "";

                var targetResult = await _userManagementService.CreateOrUpdateUserAsync(vm, null, null, origin);

                response.Data = targetResult.result;
                response.result = targetResult.result;
                response.messages.Add(targetResult.message);

                return Ok(response);
            }            
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }

        }

        [HttpPost("ImportUsers")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportUsers( IFormFile file)
        {
            GeneralResponse<bool> response = new();

            try
            {
                var userName = User.Identity.Name;
                var origin = "";

                var targetResult = await _excelManager.AdministrativeImport(file, origin);

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

        [HttpPost("Delete/{idUser}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string idUser)
        {
            GeneralResponse<bool> response = new();
            try
            {
                response = await _userService.DeleteUser(idUser);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }            
        }

        [HttpPost("ActiveInactive/{idUser}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActiveInactive(string idUser)
        {
            GeneralResponse<string> response = new();
            try
            {
                response = await _userManagementService.ActiveInactive(idUser);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpGet ("GetAllUsers")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<AppliciationUserDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            GeneralResponse<List<AppliciationUserDTO>> response = new();

            try
            {            

                var targetResponse = await _userManagementService.GetUsersAsync( new List<string> { });

                response.Data = targetResponse.data;
                response.messages.Add(targetResponse.message);
                response.result = targetResponse.result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }

        }
        
        [HttpGet("GetFathersAndMothers")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<ParentsResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFathersAndMothers()
        {
            GeneralResponse<ParentsResponseDTO> response = new();

            try
            {

                response = await _userManagementService.GetFathersAndMothers();               

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }

        }

        [HttpGet("GetAllRoles")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRoles()
        {
            GeneralResponse<List<string>> response = new();

            try
            {
                response = await _userManagementService.GetAllRoles();
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

        #region Teacher Room Management

        [HttpGet("GetTeacherToAddRoom/{idRoom}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<AppliciationUserDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTeacherToAddRoom(int idRoom)
        {
            GeneralResponse<List<AppliciationUserDTO>> response = new();
            try
            {
                response = await _userManagementService.GetTeacherToAddRoom(idRoom);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpGet("GetTeacherRoom/{idRoom}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<AppliciationUserDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTeacherRoom(int idRoom)
        {
            GeneralResponse<List<AppliciationUserDTO>> response = new();
            try
            {
                response = await _userManagementService.GetTeacherRoom(idRoom);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }

        }

        [HttpPost("QuitTeacherRoom")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> QuitTeacherRoom(QuitAddTeacherRoomDTO vm)
        {
            GeneralResponse<string> response = new();

            try
            {
                response = await _userManagementService.QuitTeacherRoom(vm);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }

        }

        [HttpPost("AddTeacherRoom")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddTeacherRoom(QuitAddTeacherRoomDTO vm)
        {
            GeneralResponse<string> response = new();

            try
            {
                response = await _userManagementService.AddTeacherRoom(vm);

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

        #region Email Confirmation and Password Management

        [HttpPost("ConfirmEmail")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmail model)
        {           
            GeneralResponse<string> response = new();
            try
            {
                response = await _userService.ConfirmEmailAsync(model.userId, model.Token);
                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }
        
        [HttpPost("RefreshToken")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<LoginResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] GenerateTokenRequestDTO model)
        {
            GeneralResponse<ResponseTokenDTO> response = new();
            try
            {
                response = await _userService.RefreshToken(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("ForgotPassword")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassWordViewModel forgotPassWord)
        {
            GeneralResponse<string> response = new();
            try
            {
                string origin = "";
                response = await _userService.ForgotPasswordAync(forgotPassWord, origin);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message + StatusCodes.Status500InternalServerError);
                return new JsonResult(response);
            }
        }

        [HttpPost("ChangePassword")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GeneralResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ResetPasswordViewModel forgotPassWord)
        {
            GeneralResponse<string> response = new();
            try
            {
                string origin = "";
                response = await _userService.ResetPasswordAsyn(forgotPassWord, origin);
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
