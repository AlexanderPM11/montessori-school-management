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
    public class SpecializationRepository:GenericRepository<Specializations>, ISpecializationRepository
    {
        private readonly ApplicationContext _dbContext;

        public SpecializationRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            this._dbContext = applicationContext;
        }
    }
}
