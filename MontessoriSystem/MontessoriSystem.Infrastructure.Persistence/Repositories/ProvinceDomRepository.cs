using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Infrastructure.Persistence.Contexts;

namespace MontessoriSystem.Infrastructure.Persistence.Repositories
{
    public class ProvinceDomRepository: GenericRepository<ProvinceDom>, IProvinceDomRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProvinceDomRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            this._dbContext = applicationContext;
        }
    }
}
