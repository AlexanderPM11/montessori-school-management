using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Specialization
{
    public class SpecializationsSaveViewModel:AuditableBaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
