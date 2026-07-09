using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Helpers.ExcelManager
{
    public class StudentAndParentsViewModel
    { 
        // Estudiantes
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public DateTime? FechaNacimientoEst { get; set; }
        public string? Sexo { get; set; }
        public string? Telefono { get; set; }
        public string? NoFolio { get; set; }
        public string? NoLibro { get; set; }
        public string? Email { get; set; }
        public string? Nivel { get; set; }
        public string? Grupo { get; set; }
        public int IdGrado { get; set; }


        // Representante 1
        public string? RepNombre { get; set; }
        public string? RepApellido1 { get; set; }
        public string? RepApellido2 { get; set; }
        public string? RepTelefono { get; set; }
        public string? RepEmail { get; set; }
        public string? Sexo_rep1 { get; set; }
        public string? Cedula { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        // Representante 2
        public string? Rep2Nombre { get; set; }
        public string? Rep2Apellido1 { get; set; }
        public string? Rep2Apellido2 { get; set; }
        public string? Rep2Telefono { get; set; }
        public string? Rep2Email { get; set; }
        public string? SexoRep2 { get; set; }
        public string? CedulaRep2 { get; set; }
        public DateTime? FechaNacimientoRep2 { get; set; }
    }
}
