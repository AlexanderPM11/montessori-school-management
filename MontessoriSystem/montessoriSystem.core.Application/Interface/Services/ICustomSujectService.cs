using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.ViewModels.Suject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ICustomSujectService
    {
        Task<GeneralResponse<List<SujectViewModel>>> GetAllSubject();
        Task<GeneralResponse<int>> CreateUpdate(SujectSaveViewModel model);
        Task<GeneralResponse<int>> Delete(int id);
    }
}
