using MontessoriSystem.Core.Application.ViewModels.Common;
using MontessoriSystem.Core.Application.ViewModels.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Group
{
    public class GroupSaveViewModel: AuditableBaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int IdRoom { get; set; }
        public List<int>? IdsGrade { get; set; } = new List<int>();
        public List<string> Grades { get; set; } = new List<string>();
        public List<GradeViewModel> GradeViewModels { get; set; } = new List<GradeViewModel> ();
    }
}
