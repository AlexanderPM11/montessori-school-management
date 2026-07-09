using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.DTOS.Account
{
    public class QuitAddTeacherRoomDTO
    {
        public List<string> IdTeachers { get; set; }
        public int IdRoom { get; set; }
    }
}
