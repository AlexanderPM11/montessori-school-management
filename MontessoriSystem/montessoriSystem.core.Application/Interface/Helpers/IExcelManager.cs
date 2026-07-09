using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Helpers
{
    public interface IExcelManager
    {
        Task<GeneralResponse<string>> StudentAndParentsImport(IFormFile excelFile);
        Task<GeneralResponse<string>> TeacherImport(IFormFile excelFile, string origin, int IdInstitu);
        Task<GeneralResponse<string>> EducationalCenterImport(IFormFile excelFile);
        Task<GeneralResponse<string>> RoomImport(IFormFile excelFile);
        Task<GeneralResponse<string>> AdministrativeImport(IFormFile excelFile, string origin);

    }
}
