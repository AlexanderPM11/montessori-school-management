using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class TipoAdjunto: AuditableBaseEntity
    {
        public string Description { get; set; }

        //Navigation property
        public IEnumerable<Adjunto> Adjuntos { get; set; }

    }
}
