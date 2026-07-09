using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.EducationalLevel;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class EducationalLevelService:GenericService<EducationalLevelSaveViewModel, EducationalLevelViewModel, EducationalLevel>, IEducationalLevelService
    {
        private readonly IEducationalLevelRepository _educationalLevelRepository;
        private readonly IMapper _mapper;

        public EducationalLevelService(IEducationalLevelRepository educationalLevelRepository, IMapper mapper)
        : base(educationalLevelRepository, mapper)
        {
            _mapper = mapper;
            _educationalLevelRepository = educationalLevelRepository;
        }
    }
}
