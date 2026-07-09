using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.InitData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence.Seed
{
    public static class InitDataSeed
    {
       

        public static async Task SetDataInit(IInitDataService initDataService)
        {
            var generalModel = new InitDataSaveViewModel
            {
                Description = InitDataEnum.EducationalCenter.ToString(),
                Ready = false,
                CreatedBy = "Admin",
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                LastModifiedBy = "Admin"
            };            
            await initDataService.Add(generalModel);

            generalModel.Description = InitDataEnum.Teachers.ToString();
            await initDataService.Add(generalModel);

            generalModel.Description = InitDataEnum.StudentsAndFather.ToString();
            await initDataService.Add(generalModel);

            generalModel.Description = InitDataEnum.Room.ToString();
            await initDataService.Add(generalModel);
        }
    }
}
