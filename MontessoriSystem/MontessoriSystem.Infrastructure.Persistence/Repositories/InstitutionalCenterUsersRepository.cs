using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Infrastructure.Persistence.Contexts;

namespace MontessoriSystem.Infrastructure.Persistence.Repositories
{
    public class InstitutionalCenterUsersRepository: GenericRepository<InstitutionalCenterUsers>, IInstitutionalCenterUsersRepository
    {
        private readonly ApplicationContext _dbContext;

        public InstitutionalCenterUsersRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            this._dbContext = applicationContext;
        }
    }
}
