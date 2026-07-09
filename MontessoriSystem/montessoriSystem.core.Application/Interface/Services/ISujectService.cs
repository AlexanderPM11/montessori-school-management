using MontessoriSystem.Core.Application.ViewModels.Suject;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ISujectService:IGenericService<SujectSaveViewModel, SujectViewModel, Suject>
    {
    }
}
