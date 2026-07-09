using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.CivilStatus;
using MontessoriSystem.Core.Application.ViewModels.EducationalLevel;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class CivilStatusServices: GenericService<SaveCivilStatusViewModel, CivilStatusViewModel, MaritalStatus>, ICivilStatusService
    {
        private readonly ICivilStatusRepository _civilStatusRepository;
        private readonly IMapper _mapper;

        public CivilStatusServices(ICivilStatusRepository civilStatusRepository, IMapper mapper)
        : base(civilStatusRepository, mapper)
        {
            _mapper = mapper;
            _civilStatusRepository = civilStatusRepository;
        }
    }
}
