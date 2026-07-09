using MontessoriSystem.Core.Application.ViewModels.Common;
using MontessoriSystem.Core.Domain.Entities;

namespace MontessoriSystem.Core.Application.ViewModels.GroupGrade
{
    public class GroupGradeSaveViewModel:AuditableBaseModel
    {
        public int GroupId { get; set; }
        public int GradeId { get; set; }
        public GroupCenter GroupCenter { get; set; }
    }
}
