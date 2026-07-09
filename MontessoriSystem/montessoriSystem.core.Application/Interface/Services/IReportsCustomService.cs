using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.ViewModels.AchievementIndicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface IReportsCustomService
    {
        Task<(GeneralResponse<AchievementIndicatorStatusViewModelResponse>, string view, string fullNameStudent)> Report(int IdStudent, string userName, bool preview = false, string period = "", int IdSubject = 0);
        Task<(GeneralResponse<AchievementIndicatorStatusViewModelResponse>, string view)> GetDataReport(int IdStudent, string period, int IdSubject = 0);
        Task<GeneralResponse<string>> UpdateEvaluationInitial(int idAchievementIndicator, string estado, int idStudent, string period);
        Task<GeneralResponse<string>> DeleteEvaluationInitial(int idAchievementIndicator, int idStudent, string period);
        Task<GeneralResponse<string>> SaveComments(int IdStudent, string comment1, string comment2, string period);
        Task<string> GetStudentGrade(int IdStudent);
        Task<List<ClassSelected<int>>> GetSubjectsByTeacher(string IdTeacher, int IdRoom);
        Task<GeneralResponse<string>> UpdateCalification(int achievementId, int score, string period, int idStudent, int selectedGradeId, bool isRp = false, int? rp = null);
        Task<GeneralResponse<string>> DeleteCalification(int achievementId, string period, int idStudent, int selectedGradeId);
    }
}
