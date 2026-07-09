using MontessoriSystem.Core.Application.ViewModels.AchievementIndicators;
using MontessoriSystem.Core.Application.ViewModels.CivilStatus;
using MontessoriSystem.Core.Domain.Entities;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ICivilStatusService: IGenericService<SaveCivilStatusViewModel, CivilStatusViewModel, MaritalStatus>
    {
    }
}
