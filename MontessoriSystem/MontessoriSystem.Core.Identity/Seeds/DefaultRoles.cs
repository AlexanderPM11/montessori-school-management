using Microsoft.AspNetCore.Identity;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Rector.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Cordinador.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Profesor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Secretario.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Padre_Tutor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basico.ToString()));
        }
    }
}
