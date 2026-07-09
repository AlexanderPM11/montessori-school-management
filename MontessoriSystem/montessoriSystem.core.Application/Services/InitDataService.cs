using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Grade;
using MontessoriSystem.Core.Application.ViewModels.InitData;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class InitDataService : GenericService<InitDataSaveViewModel, InitDataViewModel, InitData>, IInitDataService
    {
        private readonly IInitDataRepository _initDataRepository;
        private readonly IMapper _mapper;
        public InitDataService(IInitDataRepository initDataRepository, IMapper mapper) :
            base(initDataRepository, mapper)
        {
            _initDataRepository = initDataRepository;
            _mapper = mapper;
        }
    }
}
