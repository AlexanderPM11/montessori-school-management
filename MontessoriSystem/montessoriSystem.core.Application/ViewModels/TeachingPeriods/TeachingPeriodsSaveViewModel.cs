using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.TeachingPeriods
{
    public class TeachingPeriodsSaveViewModel:AuditableBaseModel
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateInit { get; set; } = DateTime.Now;
        public DateTime DateFinish { get; set; }= DateTime.Now;

        //Navigation property
        public int IdEducationalInsti { get; set; }
    }
}
