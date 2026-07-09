using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.ViewModels.RoomTeacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ICustomRoomTeacherService
    {
        Task<GeneralResponse<List<RoomTeacherViewModel>>> GetAllRoomTeacher(int idRoom);
        Task<GeneralResponse<int>> CreateUpdate(RoomTeacherSaveViewModel model);
        Task<GeneralResponse<int>> Delete(int id);
    }
}
