using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class Adjunto: AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public byte bytesAdjunto { get; set; }

        //Navigation property
        public int IdStudent { get; set; }
        public int IdTipoAdjunto { get; set; }

        public Student Student { get; set; }
        public TipoAdjunto TipoAdjunto { get; set; }
    }
}
