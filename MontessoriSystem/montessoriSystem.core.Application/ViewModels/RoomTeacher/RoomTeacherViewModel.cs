using MontessoriSystem.Core.Application.ViewModels.Common;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.RoomTeacher
{
    public class RoomTeacherViewModel:AuditableBaseModel
    {
        public string IdTeacher { get; set; }
        public int IdSuject { get; set; }
        public int IdRoom { get; set; }

        public MontessoriSystem.Core.Domain.Entities.Suject Suject { get; set; }
        public string? NameTeacher { get; set; } = "";
        public string? NameGrade { get; set; } = "";

    }
}
