using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Infrastructure.Persistence.Contexts;


namespace MontessoriSystem.Infrastructure.Persistence.Repositories
{
    public class EvaluationsPeriodRepository:GenericRepository<EvaluationsPeriod>, IEvaluationsPeriodRepository
    {
        private readonly ApplicationContext _dbContext;

        public EvaluationsPeriodRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            this._dbContext = applicationContext;
        }

    }
}
