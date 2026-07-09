using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Section
{
    public class SectionSaveViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe de especificar un nombre.")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
