using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.EvaluationsPeriod
{
    public class EvaluationsPeriodViewModel:AuditableBaseModel
    {
        public string Periodo { get; set; }
        public string Estado { get; set; }
        public int Year { get; set; }
        public string GradeStudent { get; set; }

        //Navigation Property
        public int IdStudent { get; set; }
        public MontessoriSystem.Core.Domain.Entities.Student Student { get; set; }
        public int IdAchievementIndicator { get; set; }
        public MontessoriSystem.Core.Domain.Entities.AchievementIndicators AchievementIndicator { get; set; }
    }
}
