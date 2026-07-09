using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.RoomTeacher
{
    public class RoomTeacherSaveViewModel:AuditableBaseModel
    {
        public string IdTeacher { get; set; }
        public int IdSuject { get; set; }
        public int IdRoom { get; set; }

        public string? NameTeacher { get; set; } = "";
        public string? NameGrade { get; set; } = "";


    }
}
