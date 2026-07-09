using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class TeachingPeriods : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateInit { get; set; }
        public DateTime DateFinish { get; set; }

        //Navigation property

        public EducationalInstitution EducationalInstitution { get; set; }
        public int IdEducationalInsti { get; set; }
    }
}
