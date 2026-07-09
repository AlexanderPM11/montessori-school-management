using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class EvaluationsPeriodWithCalification : AuditableBaseEntity
    {
        public string Periodo { get; set; }
        public int Calification { get; set; }
        public int? RecuperaPedg { get; set; } = 0;
        public int Year { get; set; }
        public int IdSubject { get; set; }
        public string GradeStudent { get; set; }
        //Navigation Property
        public int IdStudent { get; set; }
        public Student Student { get; set; }
        public int? IdAchievementIndicator { get; set; }
        //public AchievementIndicators AchievementIndicator { get; set; }
    }
}
