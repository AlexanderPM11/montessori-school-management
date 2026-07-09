using Microsoft.EntityFrameworkCore;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationContext _applicationContext;
        public GenericRepository( ApplicationContext applicationContext) 
        {
            this._applicationContext = applicationContext;
        }

        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            await _applicationContext.Set<Entity>().AddAsync(entity);
            await _applicationContext.SaveChangesAsync();           
            return entity;
        }
        public virtual async Task UpdateAsync(Entity entity, int id)
        {
            try
            {
                var entry = await _applicationContext.Set<Entity>().FindAsync(id);
                _applicationContext.Entry(entry!).CurrentValues.SetValues(entity);
                await _applicationContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        public virtual async Task DeleteAsync(Entity entity)
        {
            _applicationContext.Set<Entity>().Remove(entity);
            await _applicationContext.SaveChangesAsync();
        }
        public virtual async Task<List<Entity>> GetAllAsync()
        {
            return await _applicationContext.Set<Entity>().ToListAsync();
        }
        public virtual async Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties, 
            Expression<Func<Entity, bool>>? queryFilter
            = null)
        {
            var query = _applicationContext.Set<Entity>().AsQueryable();

            foreach (string property in properties)
            {
                if(queryFilter != null)
                {
                    query = query.Include(property).Where(queryFilter);
                }
                else
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }
        public virtual async Task<Entity?> GetByIdAsync(int id)
        {
            return await _applicationContext.Set<Entity>().FindAsync(id);
        }
        public virtual async Task<List<Entity>?> GetBy(Expression<Func<Entity, bool>> query)
        {
            return await _applicationContext.Set<Entity>().Where(query).ToListAsync();
        }
    }
}
