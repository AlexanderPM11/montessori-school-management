using MontessoriSystem.Core.Application.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Helpers
{
    public interface IReportAttendance
    {
        Task<AttendanceViewModelResponse> GenerateReportAttendance(int IdRoom, string Date);
        Task<StudentAttendanceViewModelResponse> GenerateStudentAttendanceReport(int studentId, string date);
    }
}
