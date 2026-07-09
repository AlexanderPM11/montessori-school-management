using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Helpers.InitData
{
    public class InitDataHelper : IInitDataHelper
    {
        private readonly IInitDataService _initDataService;
        public InitDataHelper(IInitDataService initDataService) 
        { 
            _initDataService = initDataService;
        }
        public async Task<(string action, string controller)> ValidatedInitData()
        {
            var initData = await _initDataService.GetAllViewModel();

            var unreadyScreens = initData.FirstOrDefault(x => x.Ready == false);

            if (unreadyScreens != null)
            {
                // Devuelve la acción y el controlador según la pantalla pendiente
                return unreadyScreens.Description switch
                {
                    "EducationalCenter" => ("FillEducationalCenter", "InitData"),
                    "Teachers" => ("FillTeachers", "InitData"),
                    "StudentsAndFather" => ("FillStudentsAndFather", "InitData"),
                    "Room" => ("FillRoom", "InitData")
                };
            }

            // Si todas las pantallas están listas, devuelve valores por defecto
            return (null, null);
        }
    }
}
