using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.DTOS.Student
{
    public class QuitAddStudentRoomDTO
    {
        public List<int> IdStudents { get; set; }
        public int IdRoom { get; set; }
    }
}
