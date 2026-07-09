using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface IStudentServices: IGenericService<StudentSaveViewModel,StudentViewModel,Student>
    {
        Task ActiveInactiveStudent(int id);
        Task QuitStudentRoom(int id);
        Task AddStudentRoom(int id, int IdRoom);
    }
}
