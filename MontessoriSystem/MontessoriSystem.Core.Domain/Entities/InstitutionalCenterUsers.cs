using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class InstitutionalCenterUsers
    {
        public int CenterId { get; set; }
        public EducationalInstitution EducationalInstitution { get; set; }
        public string UserId { get; set; }
    }
}
