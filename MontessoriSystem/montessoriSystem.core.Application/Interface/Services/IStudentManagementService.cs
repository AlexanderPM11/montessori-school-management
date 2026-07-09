using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Student;
using MontessoriSystem.Core.Application.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface IStudentManagementService
    {
        Task<GeneralResponse<List<StudentViewModel>>> GetAllStudents();
        Task<GeneralResponse<bool>> CreateUpdateStudent(StudentSaveViewModel model);
        Task<GeneralResponse<List<StudentViewModel>>> GetDatosEstudianteByParents(string IdParent);
        Task<GeneralResponse<List<StudentViewModel>>> GetStudentsRoom(int IdRoom);
        Task<GeneralResponse<List<StudentViewModel>>> GetStudentsToAddRoom(int IdRoom);
        Task<GeneralResponse<int>> QuitStudentRoom(int idStudent);
        Task<GeneralResponse<int>> AddStudentRoom(QuitAddStudentRoomDTO vm);
    }
}
