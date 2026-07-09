using MontessoriSystem.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Helpers
{
    public interface IValidateInfoDataBase
    {
        Task<bool> IsInstitutionIdValidAsync(int institutionId, SaveUserViewModel currentUser);
        Task<bool> IsIdUserValidAsync(string idUser, SaveUserViewModel currentUser);
    }
}
