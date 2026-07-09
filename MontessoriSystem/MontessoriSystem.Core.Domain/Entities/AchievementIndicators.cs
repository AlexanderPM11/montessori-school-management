using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class AchievementIndicators:AuditableBaseEntity
    {
        public string Description { get; set; }
        public string AssociatedGrades { get; set; }
        public string? CodeSubject { get; set; }

        //public IEnumerable<EvaluationsPeriod> EvaluationsPeriod { get; set; }
    }
}
