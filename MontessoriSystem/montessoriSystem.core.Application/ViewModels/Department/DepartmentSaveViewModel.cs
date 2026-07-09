using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Department
{
    public class DepartmentSaveViewModel:AuditableBaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IdUserCreator { get; set; }
    }
}
