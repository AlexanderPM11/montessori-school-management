using MontessoriSystem.Core.Application.ViewModels.Common;

namespace MontessoriSystem.Core.Application.ViewModels.AchievementIndicators
{
    public class AchievementIndicatorsViewModel:AuditableBaseModel
    {
        public string Description { get; set; }
        public string AssociatedGrades { get; set; }
        public string? CodeSubject { get; set; }
    }
}
