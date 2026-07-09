using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.RelationshipPerson
{
    public class RelationshipPersonSaveViewModel:AuditableBaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
