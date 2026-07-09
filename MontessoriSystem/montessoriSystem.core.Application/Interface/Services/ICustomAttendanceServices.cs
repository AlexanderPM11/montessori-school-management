using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Attendance;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ICustomAttendanceServices
    {
        Task<GeneralResponse<AttendanceSummaryViewModel>> AttendanceInfo(int IdRoom, string DayWeek);
        Task<GeneralResponse<string>> UpdateAttendance(RequestAttendanceDTO request);
        Task<GeneralResponse<string>> MakeAllPresent(RequestAttendanceDTO request);
        Task<GeneralResponse<string>> DeleteAttendance(int Id);
    }
}
