using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Grade;
using MontessoriSystem.Core.Application.ViewModels.InitData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence.Seed
{
 
    public static class GradeSeed
    {
        public static async Task SetGrade(IGradeService gradeService)
        {
            var gradeData = new Dictionary<string, string>
            {
                { "Infante", "Hasta 1 año y 11 meses" },
                //{ "Párvulo I 45 días - 6 Meses", "Niños y niñas de 45 días a 6 meses" },
                //{ "Párvulo I 6 meses - 1 año", "Niños y niñas de 6 meses - 1 año" },
                { "Párvulo I", "Niños y niñas de 6 meses a 1 año" },
                { "Párvulo II", "Niños y niñas de 1 año" },
                { "Párvulo III", "Niños y niñas de 2 año" },
                { "Pre-Kinder", "Hasta 3 año y 11 meses." },
                { "Kinder", "Hasta 4 año y 11 meses." },
                { "Preprimaria", "Hasta 5 año y 11 meses." },
                { "Primero", "Hasta 6 año y 11 meses." },
                { "Segundo", "Hasta 7 año y 11 meses." },
                { "Tercero", "Hasta 8 año y 11 meses." },
                { "Cuarto", "Hasta 9 año y 11 meses." },
                { "Quinto", "Hasta 10 año y 11 meses." },
                { "Sexto", "Hasta 11 año y 11 meses." },
                { "Primero Secundaria", "Hasta 12 año y 11 meses." },
                { "Segundo Secundaria", "Hasta 13 año y 11 meses." },
                { "Tercero Secundaria", "Hasta 14 año y 11 meses." },
                { "Cuarto Secundaria", "Hasta 15 año y 11 meses." },
                { "Quinto Secundaria", "Hasta 17 año y 11 meses." },
                { "Sexto Secundaria", "Hasta 18 año y 11 meses." },
            };

            foreach (var grade in gradeData)
            {
                var generalModel = new GradeSaveViewModel
                {
                    Name = grade.Key,
                    IdEducationalInsti = null,
                    Description = grade.Value,
                    CreatedBy = "Admin",
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    LastModifiedBy = "Admin"
                };
                await gradeService.Add(generalModel);
            }

        }
    }
}
