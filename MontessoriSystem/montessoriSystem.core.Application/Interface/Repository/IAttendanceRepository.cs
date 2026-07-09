using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Repository
{
    public interface IAttendanceRepository:IGenericRepository<Attendance>
    {
        Task<GeneralResponse<bool>> MakeAllPresent(List<AttendanceSaveViewModel> entity);
    }
}
