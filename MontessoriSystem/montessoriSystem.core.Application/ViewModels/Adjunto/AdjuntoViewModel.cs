using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Adjunto
{
    public class AdjuntoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Base64 { get; set; }
        public byte bytesAdjunto { get; set; }

        //Navigation property
        public int IdStudent { get; set; }
        public int IdTipoAdjunto { get; set; }

        public MontessoriSystem.Core.Domain.Entities.Student Student { get; set; }
        public MontessoriSystem.Core.Domain.Entities.TipoAdjunto TipoAdjunto { get; set; }
    }
}
