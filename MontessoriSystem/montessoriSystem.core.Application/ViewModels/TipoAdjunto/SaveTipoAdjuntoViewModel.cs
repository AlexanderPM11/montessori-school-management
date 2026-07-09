using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.TipoAdjunto
{
    public class SaveTipoAdjuntoViewModel
    {
        public string Description { get; set; }

        //Navigation property
        public IEnumerable<MontessoriSystem.Core.Domain.Entities.Adjunto> Adjuntos { get; set; }
    }
}
