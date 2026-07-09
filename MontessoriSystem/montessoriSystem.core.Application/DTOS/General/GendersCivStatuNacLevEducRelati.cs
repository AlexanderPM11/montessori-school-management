using MontessoriSystem.Core.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.DTOS.General
{
    public class GendersCivStatuNacLevEducRelati
    {
        public List<ClassSelected<int>> CivilStatus { get; set; }
        public List<ClassSelected<int>> Nationality { get; set; }
        public List<ClassSelected<int>> EducationalLevel { get; set; }
        public List<ClassSelected<int>> Relationship { get; set; }
        public List<ClassSelected<int>> Professions { get; set; }
        public List<ClassSelected<int>> Grades { get; set; }
        public List<ClassSelected<int>> Levels { get; set; }

        // Constructor para inicializar las listas vacías
        public GendersCivStatuNacLevEducRelati()
        {
            CivilStatus = new List<ClassSelected<int>>();
            Nationality = new List<ClassSelected<int>>();
            EducationalLevel = new List<ClassSelected<int>>();
            Relationship = new List<ClassSelected<int>>();
            Professions = new List<ClassSelected<int>>();
            Grades = new List<ClassSelected<int>>();
            Levels = new List<ClassSelected<int>>();
        }
    }
}
