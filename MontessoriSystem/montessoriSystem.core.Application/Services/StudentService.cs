using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class StudentService : GenericService<StudentSaveViewModel, StudentViewModel, Student>, IStudentServices
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        : base(studentRepository, mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task ActiveInactiveStudent(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if(student!= null)
            {
                if (student.Estado == "Activo")
                {
                    student.Estado = "Inactivo";
                }
                else
                {
                    student.Estado = "Activo";
                }
                await _studentRepository.UpdateAsync(student, id);
            }
        }

        public async Task QuitStudentRoom(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if(student!= null)
            {
                student.IdRoom = null;
                await _studentRepository.UpdateAsync(student, id);
            }
        }
         public async Task AddStudentRoom(int id, int IdRoom)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if(student!= null)
            {
                student.IdRoom = IdRoom;
                await _studentRepository.UpdateAsync(student, id);
            }
        }




    }
}
