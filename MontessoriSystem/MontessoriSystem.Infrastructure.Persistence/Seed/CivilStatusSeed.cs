using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.CivilStatus;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence.Seed
{
    public class CivilStatusSeed
    {
        public static async Task SetCivilStatus(ICivilStatusService civilStatusService)
        {
            var maritalStatuses = new List<SaveCivilStatusViewModel>
            {
                new SaveCivilStatusViewModel { Name = "Soltero", Description = "No casado o en una relación romántica" },
                new SaveCivilStatusViewModel { Name = "Casado", Description = "Legalmente casado con otra persona" },
                new SaveCivilStatusViewModel { Name = "Divorciado", Description = "Legalmente separado de un cónyuge" },
                new SaveCivilStatusViewModel { Name = "Viudo", Description = "El cónyuge ha fallecido" },
                new SaveCivilStatusViewModel { Name = "Separado", Description = "Viviendo separado de un cónyuge sin divorcio legal" },
                new SaveCivilStatusViewModel { Name = "En una relación", Description = "En una relación romántica pero no casado" },
                new SaveCivilStatusViewModel { Name = "Comprometido", Description = "Formalmente acordado casarse" }
            };
            foreach (var item in maritalStatuses)
            {
               await civilStatusService.Add(item);
            }

        }
    }
}
