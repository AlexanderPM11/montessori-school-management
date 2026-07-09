using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface IUserManagementService
    {
        Task<GeneralResponse<LoginResponse>> Login(LoginViewModel model);
        Task<(string message, bool result)> CreateOrUpdateUserAsync(SaveUserViewModel vm, int? idInstitu, int? institutionIdPrincipal, string origin);
        Task<(List<AppliciationUserDTO> data, string message, bool result)> GetUsersAsync(List<string>? selectedRole = null);
        Task<GeneralResponse<string>> ActiveInactive(string idUser);
        Task<GeneralResponse<ParentsResponseDTO>> GetFathersAndMothers();
        Task<GeneralResponse<List<string>>> GetAllRoles();
        Task<GeneralResponse<List<AppliciationUserDTO>>> GetTeacherRoom(int idRoom);
        Task<GeneralResponse<string>> QuitTeacherRoom(QuitAddTeacherRoomDTO vm);
        Task<GeneralResponse<string>> AddTeacherRoom(QuitAddTeacherRoomDTO vm);
        Task<GeneralResponse<List<AppliciationUserDTO>>> GetTeacherToAddRoom(int? IdRoom);

    }
}
