using Microsoft.EntityFrameworkCore;
using MontessoriSystem.Core.Domain.Common;

namespace MontessoriSystem.Infrastructure.Persistence.Contexts
{
    public class SecurityContext : DbContext
    {
        public SecurityContext(DbContextOptions<SecurityContext> options) : base(options) { }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken= new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FLUENT API

            #region Tables

            #endregion

            #region Primary keys

           

            #endregion

            #region Relationships


            #endregion

        }



    }
}
