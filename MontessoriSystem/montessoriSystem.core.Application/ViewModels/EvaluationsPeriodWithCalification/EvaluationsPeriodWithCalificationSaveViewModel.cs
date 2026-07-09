using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.EvaluationsPeriodWithCalification
{
    public class EvaluationsPeriodWithCalificationSaveViewModel : AuditableBaseModel
    {
        public string Periodo { get; set; }
        public int Calification { get; set; }
        public int? RecuperaPedg { get; set; }
        public int Year { get; set; }
        public string GradeStudent { get; set; }
        public int IdSubject { get; set; }
        //Navigation Property
        public int IdStudent { get; set; }
        public MontessoriSystem.Core.Domain.Entities.Student Student { get; set; }
        public int? IdAchievementIndicator { get; set; }
    }
}
