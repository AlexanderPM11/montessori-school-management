using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class Attendance:AuditableBaseEntity
    {
        public DateTime Date { get; set; }
        public string? Observation { get; set; }
        public bool? IsPresent { get; set; }
        public bool? IsDelay { get; set; }
        public bool? IsAbsent { get; set; }
        public bool? IsExcuse { get; set; }


        //Navigation Property
        public Student Student { get; set; }
        public int IdStudent { get; set; }
        public Room Room { get; set; }
        public int IdRoom { get; set; }
        public Suject? Suject { get; set; }
        public int? IdSuject { get; set; }
    }
}
