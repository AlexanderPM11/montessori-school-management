using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.ViewModels.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ICustomRoomService
    {
        Task<GeneralResponse<List<RoomoViewModel>>> GetAllRooms();
        Task<GeneralResponse<int>> CreateOrUpdate(RoomSaveViewModel model);
        Task<GeneralResponse<int>> Delete(int idRoom);
    }
}
