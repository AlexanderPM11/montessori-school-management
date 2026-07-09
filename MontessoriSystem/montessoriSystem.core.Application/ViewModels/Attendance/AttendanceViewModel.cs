using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Attendance
{
    public class AttendanceViewModel:AuditableBaseModel
    {
        public DateTime Date { get; set; }
        public string? Observation { get; set; }
        public bool? IsPresent { get; set; } = false;
        public bool? IsDelay { get; set; } = false;
        public bool? IsAbsent { get; set; } = false;
        public bool? IsExcuse { get; set; } = false;

        //Custom
        public string? FullNameStudent { get; set; }
        public int IdRoom { get; set; }
        public int IdStudent { get; set; }
        public int? IdSuject { get; set; }
        public int NumberList { get; set; }

    }
}
