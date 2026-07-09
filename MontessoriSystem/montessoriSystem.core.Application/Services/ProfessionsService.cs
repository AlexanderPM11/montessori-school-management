using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Professions;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class ProfessionsService:GenericService<ProfessionsSaveViewModel, ProfessionsViewModel, Professions>, IProfessionsService
    {
        private readonly IProfessionsRepository _professionsRepository;
        private readonly IMapper _mapper;
        public ProfessionsService(IProfessionsRepository professionsRepository, IMapper mapper) :
            base(professionsRepository, mapper)
        {
            _professionsRepository = professionsRepository;
            _mapper = mapper;
        }
    }
}
