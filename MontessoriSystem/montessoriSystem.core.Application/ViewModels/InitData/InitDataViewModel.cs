using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.InitData
{
    public class InitDataViewModel:AuditableBaseModel
    {
        public string Description { get; set; }
        public bool Ready { get; set; }
    }
}
