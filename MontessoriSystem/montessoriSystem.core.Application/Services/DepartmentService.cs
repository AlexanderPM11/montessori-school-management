using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Department;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class DepartmentService:GenericService<DepartmentSaveViewModel, DepartmentViewModel, Department>, IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IMapper mapper,IDepartmentRepository  departmentRepository )
        : base(departmentRepository,mapper)
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }
    }
}
