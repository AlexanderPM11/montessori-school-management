using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.TeachingPeriods
{
    public class TeachingPeriodsViewModel: AuditableBaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateInit { get; set; }
        public DateTime DateFinish { get; set; }

        //Navigation property
        public int IdEducationalInsti { get; set; }
    }
}
