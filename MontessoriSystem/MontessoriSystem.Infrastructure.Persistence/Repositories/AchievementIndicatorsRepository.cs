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
    internal class AchievementIndicatorsRepository: GenericRepository<AchievementIndicators>, IAchievementIndicatorsRepository
    {
        private readonly ApplicationContext _dbContext;

    public AchievementIndicatorsRepository(ApplicationContext applicationContext) : base(applicationContext)
    {
        this._dbContext = applicationContext;
    }
}
}
