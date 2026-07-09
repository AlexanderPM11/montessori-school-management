using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Attendance
{
    public class StudentAttendanceViewModelResponse
    {
        public string StudentName { get; set; }
        public string RoomName { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public int DayWorked { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalExcused { get; set; }
        public int TotalDelayed { get; set; }
        public double Percentage { get; set; }
        public List<WeekAttendanceData> Weeks { get; set; } = new List<WeekAttendanceData>();
    }

    public class WeekAttendanceData
    {
        public List<DayAttendance> Days { get; set; } = new List<DayAttendance>();
    }

    public class DayAttendance
    {
        public DateTime Date { get; set; }
        public string Status { get; set; } // P, A, E, T
    }
}
