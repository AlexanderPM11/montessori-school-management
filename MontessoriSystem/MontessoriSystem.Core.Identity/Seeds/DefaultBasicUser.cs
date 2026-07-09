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
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = "basicuser";
            applicationUser.FirstName = "Pedro";
            applicationUser.LastName = "Martínez";
            applicationUser.Email = "basic@democampus.com";
            applicationUser.EmailConfirmed = true;
            applicationUser.PhoneNumberConfirmed = true;
            applicationUser.PhoneNumber = "809-555-0101";
            applicationUser.EmailConfirmed = true;
            applicationUser.Addres = "Santiago";
            applicationUser.Gender = 1;
            applicationUser.Statu =true;
            applicationUser.IdentificationId = "000-000000-0";

            if (userManager.Users.All(u => u.Id != applicationUser.Id))
            {
                var user = await userManager.FindByEmailAsync(applicationUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(applicationUser,"123Pass$$word!");
                    await userManager.AddToRoleAsync(applicationUser,Roles.Basico.ToString());
                }
            } 
        }
    }
}
