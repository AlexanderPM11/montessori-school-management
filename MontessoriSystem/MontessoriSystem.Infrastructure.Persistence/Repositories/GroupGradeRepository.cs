using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Infrastructure.Persistence.Contexts;

namespace MontessoriSystem.Infrastructure.Persistence.Repositories
{
    public class GroupGradeRepository: GenericRepository<GroupGrade>, IGroupGradeRepository
    {
        private readonly ApplicationContext _dbContext;

        public GroupGradeRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            this._dbContext = applicationContext;
        }
    }
}
