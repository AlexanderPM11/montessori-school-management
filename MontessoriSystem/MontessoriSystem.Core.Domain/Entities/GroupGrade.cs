using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class GroupGrade:AuditableBaseEntity
    {
        public int GroupId { get; set; }
        public GroupCenter GroupCenter { get; set; }

        public int GradeId { get; set; }
        public Grade Grade { get; set; }
    }
}
