using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Adjunto
{
    public class SaveAdjuntoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe de especificar un nommbre para el archivo.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Debe de especificar una descripcion.")]
        public string Description { get; set; }
        public string? Path { get; set; }
        public byte? bytesAdjunto { get; set; }

        //[Required(ErrorMessage = "Debe de especificar un archivo.")]
        public IFormFile? Archivo { get; set; }


        //Navigation property
        public int? IdStudent { get; set; }
        public int? IdTipoAdjunto { get; set; }

        public MontessoriSystem.Core.Domain.Entities.Student? Student { get; set; }
        public MontessoriSystem.Core.Domain.Entities.TipoAdjunto? TipoAdjunto { get; set; }
    }
}
