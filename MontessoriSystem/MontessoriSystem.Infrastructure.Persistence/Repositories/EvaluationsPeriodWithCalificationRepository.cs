using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence.Repositories
{
    public class EvaluationsPeriodWithCalificationRepository:GenericRepository<EvaluationsPeriodWithCalification>, IEvaluationsPeriodWithCalificationRepository
    {
        private readonly ApplicationContext _dbContext;

        public EvaluationsPeriodWithCalificationRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            this._dbContext = applicationContext;
        }
    }
}
