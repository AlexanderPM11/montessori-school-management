using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Attendance
{
    public class WeekDaysReportAttendance:AuditableBaseEntity
    {
        public List<string> FirstWeekDetails { get; set; } = new List<string>();    

        public List<string> SecondWeekDetails { get; set; } = new List<string>();
      

        public List<string> ThirdWeekDetails { get; set; } = new List<string>();
       

        public List<string> FourWeekDetails { get; set; } = new List<string>();
       

        public List<string> FiveWeekDetails { get; set; } = new List<string>();
    


    }
}
