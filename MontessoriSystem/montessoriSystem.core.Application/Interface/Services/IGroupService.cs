using MontessoriSystem.Core.Application.ViewModels.Group;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface IGroupService:IGenericService<GroupSaveViewModel, GroupViewModel, GroupCenter>
    {

    }
}
