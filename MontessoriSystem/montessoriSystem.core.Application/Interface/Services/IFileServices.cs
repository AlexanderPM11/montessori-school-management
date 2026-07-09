using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface IFileServices
    {
        Task<GeneralResponse<string>> CreateOrUpdateFile(IFormFile Archivo, string pathNewFolder);
        Task DeleteFile(string path);
        Task<GeneralResponse<string>> ConvertFileToBase64(IFormFile archivo);
        Task<GeneralResponse<string>> GetImageAsBase64(string fileName);
    }
}
