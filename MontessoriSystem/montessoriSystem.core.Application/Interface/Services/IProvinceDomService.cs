

using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.ViewModels.Province;
using MontessoriSystem.Core.Domain.Entities;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface IProvinceDomService:IGenericService<ProvinceDomSaveViewModel, ProvinceDomViewModel, ProvinceDom>
    {
    }
}
