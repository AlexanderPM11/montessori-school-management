using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class ObservationCommentEvaluation:AuditableBaseEntity
    {
        public string Period { get; set; }
        public string Comment1 { get; set; }
        public string? Comment2 { get; set; }
        public int Year { get; set; }
        public string GradeStudent { get; set; }
        // Navigatiion Property
        public int IdStudent { get; set; }
        public Student Student { get; set; }
    }
}
