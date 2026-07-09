using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.General;
using MontessoriSystem.Core.Application.Specialization;
using MontessoriSystem.Core.Application.ViewModels.EducationalLevel;
using MontessoriSystem.Core.Application.ViewModels.MaritalStatus;
using MontessoriSystem.Core.Application.ViewModels.Nationality;
using MontessoriSystem.Core.Application.ViewModels.Professions;
using MontessoriSystem.Core.Application.ViewModels.RelationshipPerson;
using MontessoriSystem.Core.Application.ViewModels.TitlesAchieved;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ISelectionService
    {
       Task<List<ClassSelected<int>>> GetTitlesAchieved();
       Task<List<ClassSelected<int>>> GetSpecializations();
       Task<List<ClassSelected<int>>> GetEducationalLevel();
       Task<List<ClassSelected<int>>> GetProfessions();
       Task<List<ClassSelected<int>>> GetCivilStatus();
       Task<List<ClassSelected<int>>> GetNationality();
       Task<List<ClassSelected<int>>> GetRelationship();
       Task<List<ClassSelected<int>>> GetLevels();
       Task<GeneralResponse<GendersCivStatuNacLevEducRelati>> GendersCivStatuNacLevEducRelati();
       Task<List<ClassSelected<string>>> GetTeachers();
        Task<List<ClassSelected<int>>> GetProvices();
        Task<List<ClassSelected<string>>> GetTeachersByIdRoom(int idRoom);
        Task<List<ClassSelected<string>>> GetPeriods(bool isPrimaria = false);
        Task<Dictionary<string, List<ClassSelected<string>>>> GetGeneralsUserRoles();
        Task<GeneralResponse<List<string>>> GetDaysOfWeek();
    }
}
