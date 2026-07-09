using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class RoomTeacher:AuditableBaseEntity
    {
        public string IdTeacher { get; set; }      

        public Room Room { get; set; }

        public int IdRoom { get; set; }

        public Suject Suject { get; set; }
        public int IdSuject { get; set; }
    }
}
