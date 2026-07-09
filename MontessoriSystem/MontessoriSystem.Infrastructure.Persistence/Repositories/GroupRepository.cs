using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence.Repositories
{
    public class GroupRepository: GenericRepository<GroupCenter>, IGroupRepository
    {
        private readonly ApplicationContext _dbContext;

        public GroupRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            this._dbContext = applicationContext;
        }
    }
}
