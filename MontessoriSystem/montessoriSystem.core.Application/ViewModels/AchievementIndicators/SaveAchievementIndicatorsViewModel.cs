using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.AchievementIndicators
{
    public class SaveAchievementIndicatorsViewModel:AuditableBaseModel
    {
        public string Description { get; set; }
        public string AssociatedGrades { get; set; }
        public string? CodeSubject { get; set; }
    }
}
