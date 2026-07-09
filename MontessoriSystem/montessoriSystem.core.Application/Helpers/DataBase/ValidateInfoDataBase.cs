using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Helpers.DataBase
{
    public class ValidateInfoDataBase: IValidateInfoDataBase
    {
        private readonly IUserService _userService;
        private readonly IEducationalInstitutionService _educationalInstitutionServices;

        public ValidateInfoDataBase(IUserService userService, IEducationalInstitutionService
            educationalInstitutionServices) 
        { 
            _educationalInstitutionServices = educationalInstitutionServices;
            _userService = userService;
        }
        public async Task<bool> IsInstitutionIdValidAsync(int institutionId, SaveUserViewModel currentUser)
        {
            if (institutionId == 0)
            {
                return false;
            }

            var educationCenter = await _educationalInstitutionServices.GetByIdSaveViewModel(institutionId);
            
            if (educationCenter == null)
            {
                return false;
            }

            var institutionCreatorId = educationCenter.IdUser;  
           
            if ((currentUser.Roles.Any(role => role == Roles.SuperAdmin.ToString())))
            {
                if (currentUser.Id == institutionCreatorId)
                {
                    return true;
                }
                return false;
            }
            string idSuperAdmin = currentUser.IdUserCreator;


            var adminUsers = await _userService.GetAllMyUsers(idSuperAdmin);
            adminUsers = adminUsers.Where(urs => urs.InstitutionId == institutionId).ToList();

            foreach (var userAdmin in adminUsers)
            {
                if (userAdmin.Id == currentUser.Id)
                {
                    return true;
                }                
            }
            return false;
        }  
        public async Task<bool> IsIdUserValidAsync(string idUser, SaveUserViewModel currentUser)
        {
            
            if (currentUser.Id == idUser)
            {
                return true;
            }

            if ( !(currentUser.Roles.Any(role=> role == Roles.SuperAdmin.ToString() || role == Roles.Admin.ToString())) )
            {
                if (currentUser.Id == idUser)
                {
                    return true;
                }
                return false;
            }

            string idSuperAdmin = "";
            if ((currentUser.Roles.Any(role => role == Roles.SuperAdmin.ToString())))
            {
                idSuperAdmin = currentUser.Id;
            }
            else
            {
                idSuperAdmin= currentUser.IdUserCreator;
            }

            var adminUsers = await _userService.GetAllMyUsers(idSuperAdmin);

            foreach (var item in adminUsers)
            {

                if (item.Id == idUser)
                {
                    return true;
                }

                if (item.Roles.Contains(Roles.Admin.ToString()))
                {
                    var allUsers = await _userService.GetAllMyUsers(item.Id);
                    foreach (var user in allUsers)
                    {
                        if (user.Id == idUser)
                        {
                            return true;
                        }
                    }
                }
                
            }



            return false;

        }      
       
    }
}
