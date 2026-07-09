using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Attendance
{
    public class ReportAttendance:AuditableBaseModel
    {     
        public  WeekDaysReportAttendance weekDaysReportAttendance { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAusent { get; set; }
        public double PercentAisstMonth { get; set; }
        public int NumberListStudent { get; set; }
        public string? Observations { get; set; }
    }
}
