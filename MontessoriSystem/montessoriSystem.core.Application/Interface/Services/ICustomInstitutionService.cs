using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ICustomInstitutionService
    {
        Task<GeneralResponse<EducationalInstitutionViewModel>> GetInstituCenter();
        Task<GeneralResponse<string>> CreateUpdate(SaveEducationalInstitutionViewModel model);
    }
}
