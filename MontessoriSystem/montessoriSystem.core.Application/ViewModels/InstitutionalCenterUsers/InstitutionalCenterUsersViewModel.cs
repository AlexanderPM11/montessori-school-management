using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.InstitutionalCenterUsers
{
    public class InstitutionalCenterUsersViewModel
    {
        public int CenterId { get; set; }
        public MontessoriSystem.Core.Domain.Entities.EducationalInstitution EducationalInstitution { get; set; }
        public string UserId { get; set; }
    }
}
