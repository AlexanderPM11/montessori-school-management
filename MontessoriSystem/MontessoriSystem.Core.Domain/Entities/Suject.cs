using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class Suject : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        //Navigation property

        public EducationalInstitution? EducationalInstitution { get; set; }
        public IEnumerable<Attendance>? Attendance { get; set; }
        public IEnumerable<RoomTeacher>? RoomTeacher { get; set; }
        public int? IdEducationalInsti { get; set; }
    }
}
