using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Identity.Contexts
{
    public class IdentityContext: IdentityDbContext<ApplicationUser>
    {
      public  IdentityContext(DbContextOptions<IdentityContext> options):base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Tables

            //modelBuilder.HasDefaultSchema("Identity");

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("users");

            modelBuilder.Entity<IdentityRole>()
                .ToTable("roles");

            modelBuilder.Entity<IdentityUserRole<string>>()
                .ToTable("userroles");

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .ToTable("userlogin");

            #endregion

        }


    }

}
