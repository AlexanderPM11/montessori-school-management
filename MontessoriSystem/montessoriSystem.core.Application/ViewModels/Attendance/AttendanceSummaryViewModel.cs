using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Attendance
{
    public class AttendanceSummaryViewModel:AuditableBaseModel
    {
        public List<AttendanceViewModel> Attendances { get; set; }
        public int PresentCount { get; set; }
        public int DelayCount { get; set; }
        public int AbsentCount { get; set; }
        public int ExcuseCount { get; set; }
        public int TotalCount { get; set; }
    }
}
