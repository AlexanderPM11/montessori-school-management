using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ICustAdjuntoService
    {
        Task<GeneralResponse<List<AdjuntoViewModel>>> GetAdjunto(int idStudent);
        Task<GeneralResponse<int>> CreateOrUpdate([FromForm] SaveAdjuntoViewModel model);
        Task<GeneralResponse<int>> Delete(int idAdjunto);
    }
}
