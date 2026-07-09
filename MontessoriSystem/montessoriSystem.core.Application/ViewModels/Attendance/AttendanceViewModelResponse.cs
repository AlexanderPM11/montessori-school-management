using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Attendance
{
    public class AttendanceViewModelResponse
    {
        public List<ReportAttendance> attendanceViews { get; set; } = new List<ReportAttendance>();

        public int DayWorked { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }


        //

        public List<int> FirstWeekFemale { get; set; } = new List<int>();
        public List<int> FirstWeekmale { get; set; } = new List<int>();

        public List<int> SecondWeekFemale { get; set; } = new List<int>();
        public List<int> SecondWeekmale { get; set; } = new List<int>();

        public List<int> ThirdWeekFemale { get; set; } = new List<int>();
        public List<int> ThirdWeekmale { get; set; } = new List<int>();

        public List<int> FourWeekFemale { get; set; } = new List<int>();
        public List<int> FourWeekmale { get; set; } = new List<int>();

        public List<int> FiveWeekFemale { get; set; } = new List<int>();
        public List<int> FiveWeekmale { get; set; } = new List<int>();
    }
}
