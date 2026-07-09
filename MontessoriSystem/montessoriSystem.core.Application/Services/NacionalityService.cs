using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Nationality;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class NacionalityService:GenericService<NationalitySaveViewModel, NationalityViewModel, Nationality>, INacionalityService
    {
        private readonly INationalityRepository _nationalityRepository;
        private readonly IMapper _mapper;

        public NacionalityService(INationalityRepository nationalityRepository, IMapper mapper)
        : base(nationalityRepository, mapper)
        {
            _mapper = mapper;
            _nationalityRepository = nationalityRepository;
        }
    }
}
