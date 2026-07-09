using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class GroupCenter:AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }


        //Navigation property
        public Room Room { get; set; }
        public int IdRoom { get; set; }
        public IEnumerable<GroupGrade> GroupGrade { get; set; }


    }
}
