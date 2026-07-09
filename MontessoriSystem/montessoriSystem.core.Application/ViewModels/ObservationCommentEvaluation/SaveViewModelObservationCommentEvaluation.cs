using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.ObservationCommentEvaluation
{
    public class SaveViewModelObservationCommentEvaluation:AuditableBaseModel
    {
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Period { get; set; }
        public int IdStudent { get; set; }
        public int Year { get; set; }
        public string GradeStudent { get; set; }
    }
}
