using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Group;
using MontessoriSystem.Core.Application.ViewModels.GroupGrade;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class GroupGradeService:GenericService<GroupGradeSaveViewModel, GroupGradeViewModel, GroupGrade>, IGroupGradeService
    {

        private readonly IGroupGradeRepository _groupGradeRepository;
        private readonly IMapper _mapper;

        public GroupGradeService(IGroupGradeRepository groupGradeRepository, IMapper mapper)
        : base(groupGradeRepository, mapper)
        {
            _mapper = mapper;
            _groupGradeRepository = groupGradeRepository;

        }
    }
}
