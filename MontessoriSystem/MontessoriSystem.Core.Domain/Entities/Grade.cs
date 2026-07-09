using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class Grade:AuditableBaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Age { get; set; }

        public EducationalInstitution? EducationalInstitution { get; set; }
        public int? IdEducationalInsti { get; set; }

        public IEnumerable<RoomTeacher>? RoomTeachers { get; set; }
        public IEnumerable<GroupGrade>? GroupGrade { get; set; }
        public IEnumerable<Student>? Students { get; set; }
    }
}
