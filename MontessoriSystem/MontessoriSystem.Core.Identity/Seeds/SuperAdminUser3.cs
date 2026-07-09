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
    public static class SuperAdminUser3
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {

            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = "oficina";
            applicationUser.FirstName = "Ana";
            applicationUser.LastName = "Del Rosario Mercedes";
            applicationUser.Email = "oficina@democampus.com";
            applicationUser.EmailConfirmed = true;
            applicationUser.PhoneNumberConfirmed = true;
            applicationUser.PhoneNumber = "809-555-0104";
            applicationUser.Addres = "Bávaro";
            applicationUser.Statu = true;
            applicationUser.Gender = 0;
            applicationUser.IdentificationId = "000-000000-0";

            if (userManager.Users.All(u => u.Id != applicationUser.Id))
            {
                var user = await userManager.FindByEmailAsync(applicationUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(applicationUser, "123Pass$$word!");
                    await userManager.AddToRoleAsync(applicationUser, Roles.SuperAdmin.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Rector.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Profesor.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Cordinador.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Secretario.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Padre_Tutor.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Basico.ToString());
                }
            }
        }
    }
}
