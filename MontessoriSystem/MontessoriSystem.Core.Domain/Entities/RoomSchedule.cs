using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class RoomSchedule:AuditableBaseEntity
    {
        public string InitDate { get; set; }
        public string FinishDate { get; set; }
        public int IdSubject { get; set; }
        public string? IdTeacher { get; set; }

        //Navigation property
        public Room Room { get; set; }
        public int IdRoom { get; set; }
    }
}
