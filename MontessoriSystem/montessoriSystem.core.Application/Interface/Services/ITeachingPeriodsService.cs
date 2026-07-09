using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using MontessoriSystem.Core.Application.ViewModels.TeachingPeriods;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ITeachingPeriodsService: IGenericService<TeachingPeriodsSaveViewModel, TeachingPeriodsViewModel, TeachingPeriods>
    {
    }
}
