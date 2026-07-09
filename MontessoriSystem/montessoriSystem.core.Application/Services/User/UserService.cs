using AutoMapper;
using crudSignalR.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.User;

namespace MontessoriSystem.Core.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<List<UserViewModel>> GetAllMyAdminUsers(string IdUserCreator)
        {
            try
            {
                var response = await _accountService.GetAllMyAdminUsers(IdUserCreator);
                List<UserViewModel> userViews = _mapper.Map<List<UserViewModel>>(response.Data);
                return userViews;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<UserViewModel>> GetAllMyUsers(string IdUserCreator)
        {
            try
            {
                var response = await _accountService.GetAllMyUsers(IdUserCreator);
                List<UserViewModel> userViews = _mapper.Map<List<UserViewModel>>(response.Data);
                return userViews;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<UserViewModel>> GetAllUsersByIdIinstitutionPrincipal(int institutionIdPrincipal)
        {
            try
            {
                var response = await _accountService.GetAllUsersByIdIinstitutionPrincipal(institutionIdPrincipal);
                List<UserViewModel> userViews = _mapper.Map<List<UserViewModel>>(response.Data);
                return userViews;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<SaveUserViewModel> GetUserById(string idUser)
        {
            var user = await _accountService.GetUserById(idUser);
            if (user.result)
            {
                SaveUserViewModel saveUserViewModel = _mapper.Map<SaveUserViewModel>(user.Data);

                return saveUserViewModel;
            }
            else
            {
                return new SaveUserViewModel();
            }

        }
        public async Task<List<UserViewModel>> GetUserByIdInstitu(int IdInstitu)
        {
            var user = await _accountService.GetUserByIdInstitu(IdInstitu);
            if (user.result)
            {
                List<UserViewModel> saveUserViewModel = _mapper.Map<List<UserViewModel>>(user.Data);

                return saveUserViewModel;
            }
            else
            {
                return new List<UserViewModel>();
            }

        }
        public async Task<SaveUserViewModel> GetUserByName(string name)
        {
            var user = await _accountService.GetUserByName(name);
            if (user.result)
            {
                SaveUserViewModel saveUserViewModel = _mapper.Map<SaveUserViewModel>(user.Data);

                return saveUserViewModel;
            }
            else
            {
                return new SaveUserViewModel();
            }

        }

         public async Task<GeneralResponse<bool>> QuitStudentTeacher(string idUser, int IdRoom)
         {
             return await _accountService.QuitStudentRoom(idUser, IdRoom);            

         }

         public async Task<GeneralResponse<bool>> AddStudentTeacher(string idUser, int IdRoom)
         {
            return await _accountService.AddStudentRoom(idUser, IdRoom);           

         }
        

        public Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllUser()
        {
            return _accountService.GetAllUser();
        }
        public async Task<GeneralResponse<LoginResponse>> AuthenticateAsync(LoginViewModel request)
        {
            GeneralResponse<LoginResponse> targetResponse = new();
            AuthenticationRequest authenticationRequest = _mapper.Map<AuthenticationRequest>(request);
            var response = await _accountService.AuthenticateAsync(authenticationRequest);

            targetResponse.Data = _mapper.Map<LoginResponse>(response.Data);
            targetResponse.result = response.result;
            targetResponse.messages = response.messages;
            return targetResponse;
        }
        public async Task<GeneralResponse<LoginResponse>> AuthenticateAsyncWebApi(LoginViewModel request)
        {
            GeneralResponse<LoginResponse> targetResponse = new();
            AuthenticationRequest authenticationRequest = _mapper.Map<AuthenticationRequest>(request);
            var response = await _accountService.AuthenticateAsyncWebApi(authenticationRequest);

            targetResponse.Data = _mapper.Map<LoginResponse>(response.Data);
            targetResponse.result = response.result;
            targetResponse.messages = response.messages;
            return targetResponse;
        }
        public async Task<GeneralResponse<ResponseTokenDTO>> RefreshToken(GenerateTokenRequestDTO generateToken)
        {
            GeneralResponse<ResponseTokenDTO> targetResponse = new();
            targetResponse = await _accountService.RefreshToken(generateToken);

            return targetResponse;
        }

        public async Task<GeneralResponse<string>> ConfirmEmailAsync(string userId, string token)
        {
            GeneralResponse<string> response = await _accountService.ConfirmEmailAsync(userId, token);
            return response;
        }

        public async Task<GeneralResponse<string>> ForgotPasswordAync(ForgotPassWordViewModel request, string origin)
        {
            ForgotPassswordRequest forgotPassswordRequest = _mapper.Map<ForgotPassswordRequest>(request);
            GeneralResponse<string> response = await _accountService.ForgotPasswordAync(forgotPassswordRequest, origin);
            return response;
        }

        public async Task<GeneralResponse<string>> RegisterUsserAsync(SaveUserViewModel request, List<string> roles, string origin, bool sendEmail = false, bool apiURl = false)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(request);
            GeneralResponse<string> response = await _accountService.RegisterUserAsync(registerRequest, roles, origin);
            return response;
        }
        public async Task<GeneralResponse<string>> UpdateUserAsync(SaveUserViewModel request, List<string> roles, string Id)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(request);
            GeneralResponse<string> response = await _accountService.UpdateUserAsync(registerRequest, roles, Id);
            return response;
        }

        public async Task<GeneralResponse<string>> ResetPasswordAsyn(ResetPasswordViewModel request, string origin)
        {
            ResetPasswordRequest resetPasswordRequest = _mapper.Map<ResetPasswordRequest>(request);
            GeneralResponse<string> response = await _accountService.ResetPasswordAsyn(resetPasswordRequest, origin);
            return response;
        }

        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }

        public Task<List<string>> GetAllRoles(List<string> rolesUser)
        {
            var roles = _accountService.GetAllRoles(rolesUser);
            return roles;
        }

        public async Task<GeneralResponse<bool>> DeleteUser(string idUser)
        {
            GeneralResponse<bool> response = await _accountService.DeleteUser(idUser);
            return response;
        }
    }
}
