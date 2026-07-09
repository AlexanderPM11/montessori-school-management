using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.User;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface IUserService
    {
        Task<GeneralResponse<LoginResponse>> AuthenticateAsyncWebApi(LoginViewModel request);
        Task<GeneralResponse<ResponseTokenDTO>> RefreshToken(GenerateTokenRequestDTO generateToken);
        Task<GeneralResponse<LoginResponse>> AuthenticateAsync(LoginViewModel request);
        Task<GeneralResponse<string>> RegisterUsserAsync(SaveUserViewModel request, List<string> roles, string origin, bool sendEmail = false, bool apiURl = false);
        Task<GeneralResponse<string>> UpdateUserAsync(SaveUserViewModel request, List<string> roles, string Id);
        Task<GeneralResponse<string>> ConfirmEmailAsync(string userId, string token);
        Task<GeneralResponse<string>> ForgotPasswordAync(ForgotPassWordViewModel request, string origin);
        Task<GeneralResponse<string>> ResetPasswordAsyn(ResetPasswordViewModel request, string origin);
        Task<GeneralResponse<bool>> QuitStudentTeacher(string idUser, int IdRoom);
        Task<GeneralResponse<bool>> AddStudentTeacher(string idUser, int IdRoom);
        Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllUser();
        Task<GeneralResponse<bool>> DeleteUser(string idUser);
        Task<SaveUserViewModel> GetUserById(string idUser);
        Task<SaveUserViewModel> GetUserByName(string name);
        Task<List<UserViewModel>> GetAllMyAdminUsers(string IdUserCreator);
        Task<List<UserViewModel>> GetAllMyUsers(string IdUserCreator);
        Task<List<UserViewModel>> GetUserByIdInstitu(int IdInstitu);
        Task<List<UserViewModel>> GetAllUsersByIdIinstitutionPrincipal(int institutionIdPrincipal);
        Task<List<String>> GetAllRoles(List<string> rolesUser);
        Task SignOutAsync();
    }
}
