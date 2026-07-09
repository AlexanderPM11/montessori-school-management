using MontessoriSystem.Core.Application.ViewModels.TipoAdjunto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ITipoAdjuntoService : IGenericService<SaveTipoAdjuntoViewModel, 
        TipoAdjuntoViewModel, MontessoriSystem.Core.Domain.Entities.TipoAdjunto>
    {

    }
}
