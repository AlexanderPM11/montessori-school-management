using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.InstitutionalCenterUsers;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class InstitutionalCenterUsersService: GenericService<InstitutionalCenterUsersSaveViewModel, InstitutionalCenterUsersViewModel, InstitutionalCenterUsers>, IInstitutionalCenterUsersService
    {
        private readonly IInstitutionalCenterUsersRepository _repository;
        private readonly IMapper _mapper;

        public InstitutionalCenterUsersService(IInstitutionalCenterUsersRepository repository, IMapper mapper)
        : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
    }
}
