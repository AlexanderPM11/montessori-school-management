using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Group;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class GroupService: GenericService<GroupSaveViewModel, GroupViewModel, GroupCenter>, IGroupService
    {

        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository,IMapper mapper)
        : base(groupRepository,mapper)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
            
        }
    }
}
