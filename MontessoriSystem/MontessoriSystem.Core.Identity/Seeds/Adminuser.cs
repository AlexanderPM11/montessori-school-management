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
    public static class AdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = "AdminUser";
            applicationUser.FirstName = "Juan";
            applicationUser.LastName = "Pérez Gómez";
            applicationUser.Email = "admin@democampus.com";
            applicationUser.EmailConfirmed = true;
            applicationUser.PhoneNumberConfirmed = true;
            applicationUser.PhoneNumber = "809-555-0100";
            applicationUser.EmailConfirmed = true;
            applicationUser.Addres = "Santo Domingo";
            applicationUser.Statu = true;
            applicationUser.Gender = 1;
            applicationUser.IdentificationId = "000-000000-0";

            if (userManager.Users.All(u => u.Id != applicationUser.Id))
            {
                var user = await userManager.FindByEmailAsync(applicationUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(applicationUser, "123Pass$$word!");
                    await userManager.AddToRoleAsync(applicationUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Basico.ToString());
                }
            }
        }
    }
}
