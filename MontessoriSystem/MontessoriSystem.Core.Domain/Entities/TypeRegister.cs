using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class TypeRegister:AuditableBaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }

        //Navigation property
        public virtual IEnumerable<Student>? Student { get; set; }
        public IEnumerable<Room> Room { get; set; }
    }
}
