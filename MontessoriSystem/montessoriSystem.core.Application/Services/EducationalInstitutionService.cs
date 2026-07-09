using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class EducationalInstitutionService: GenericService<SaveEducationalInstitutionViewModel, EducationalInstitutionViewModel, EducationalInstitution>, IEducationalInstitutionService
    {
        private readonly IEducationalInstitutionRepository _EducInsRepository;
        private readonly IMapper _mapper;

        public EducationalInstitutionService(IEducationalInstitutionRepository educInsRepository, IMapper mapper)
        : base(educInsRepository, mapper)
        {
            _mapper = mapper;
                _EducInsRepository = educInsRepository;
        } 
    }   


}

