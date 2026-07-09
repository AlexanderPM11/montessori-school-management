

using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.Enums;

namespace crudSignalR.Core.Application.Interface.Services
{
    public interface IAccountService
    {

        Task<GeneralResponse<AuthenticationResponse>> AuthenticateAsyncWebApi(AuthenticationRequest request);
        Task<GeneralResponse<ResponseTokenDTO>> RefreshToken(GenerateTokenRequestDTO generateToken);
        Task<GeneralResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<GeneralResponse<string>> RegisterUserAsync(RegisterRequest request, List<string> roles, string origin, bool sendEmail = false, bool apiURl = false);
        Task<GeneralResponse<string>> UpdateUserAsync(RegisterRequest request, List<string> roles, string Id);
        Task<GeneralResponse<bool>> QuitStudentRoom(string idUser, int IdRoom);
        Task<GeneralResponse<bool>> AddStudentRoom(string idUser, int IdRoom);
        Task<GeneralResponse<string>> ConfirmEmailAsync(string userId, string token);
        Task<GeneralResponse<string>> ForgotPasswordAync(ForgotPassswordRequest request, string origin);
        Task<GeneralResponse<string>> ResetPasswordAsyn(ResetPasswordRequest request, string origin);
        Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllUser();
        Task<GeneralResponse<bool>> DeleteUser(string idUser);
        Task<GeneralResponse<AppliciationUserDTO>> GetUserById(string idUser);
        Task<GeneralResponse<AppliciationUserDTO>> GetUserByName(string name);
        Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllMyAdminUsers(string IdUserCreator);
        Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllMyUsers(string IdUserCreator);
        Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllUsersByIdIinstitutionPrincipal(int institutionIdPrincipal);
        Task<List<String>> GetAllRoles(List<string> rolesUser);
        Task<GeneralResponse<List<AppliciationUserDTO>>> GetUserByIdInstitu(int IdInstitu);
        Task SignOutAsync();
    }
}