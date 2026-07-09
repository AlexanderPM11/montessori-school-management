using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.DTOS.Attendance
{
    public class RequestAttendanceDTO
    {
        public int IdRoom { get; set; }
        public int IdSubject { get; set; }
        public string DayWeek { get; set; }
        public int IdStudent { get; set; }
        public string Status { get; set; } 
        public string? Observations { get; set; } 
    }
}
