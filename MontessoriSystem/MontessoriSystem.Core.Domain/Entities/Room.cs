using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class Room : AuditableBaseEntity
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string? ImageUrl { get; set; }
        public string? IdTypeRegisters { get; set; }

        //TODO: Agreagr relacion para manejar los maestro y maestro encargados

        //Navigation property

        public EducationalInstitution? EducationalInstitution { get; set; }
        public int? IdEducationalInsti { get; set; }
        public string IdTeacherLead { get; set; }

        public IEnumerable<RoomSchedule> Schedules { get; set; }
        public IEnumerable<RoomTeacher> RoomTeachers { get; set; }
        public IEnumerable<GroupCenter> GroupCenter { get; set; }
        public IEnumerable<Student> Student { get; set; }
        public IEnumerable<Attendance> Attendance { get; set; }
    }
}
