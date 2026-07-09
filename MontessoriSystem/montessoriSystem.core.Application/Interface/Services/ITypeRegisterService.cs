using MontessoriSystem.Core.Application.ViewModels.TypeRegister;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ITypeRegisterService:IGenericService<TypeRegisterSaveViewModel, TypeRegisterViewModel, TypeRegister>
    {
    }
}
