using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class Section: AuditableBaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Estado { get; set; } = "Activo";

        //Navigation property
        //public IEnumerable<Student> Students { get; set; }
    }
}
