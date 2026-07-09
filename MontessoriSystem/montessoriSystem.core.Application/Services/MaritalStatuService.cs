using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.MaritalStatus;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class MaritalStatuService: GenericService<MaritalStatusSaveViewModel, MaritalStatusViewModel, MaritalStatus>,
        IMaterialStatusService
    {
        private readonly IMaritalStatusRepository _maritalStatusRepository;
        private readonly IMapper _mapper;

        public MaritalStatuService(IMaritalStatusRepository maritalStatusRepository, IMapper mapper)
        : base(maritalStatusRepository, mapper)
        {
            _mapper = mapper;
            _maritalStatusRepository = maritalStatusRepository;
        }
    }
}
