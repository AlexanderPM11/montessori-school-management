using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Grade;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class GradeService :  GenericService<GradeSaveViewModel, GradeViewModel, Grade>,  IGradeService
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IMapper _mapper;
        public GradeService(IGradeRepository gradeRepository, IMapper mapper) : 
            base(gradeRepository, mapper)
        {
            _gradeRepository = gradeRepository;
            _mapper = mapper;
        }
    }
}
