using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Room
{
    public class RoomoViewModel: AuditableBaseModel
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public int IdEducationalInsti { get; set; }
        public string IdTeacherLead { get; set; }
        public string? IdTypeRegisters { get; set; }
        public string? TeacherFullName { get; set; }
        public string Level { get; set; }

    }
}
