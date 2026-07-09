using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.TeachingPeriods;
using MontessoriSystem.Core.Domain.Entities;

namespace MontessoriSystem.Core.Application.Services
{
    internal class TeachingPeriodsService: GenericService<TeachingPeriodsSaveViewModel, TeachingPeriodsViewModel, TeachingPeriods>, ITeachingPeriodsService
    {
        private readonly ITeachingPeriodsRepository _periodsRepository;
        private readonly IMapper _mapper;

        public TeachingPeriodsService(ITeachingPeriodsRepository periodsRepository, IMapper mapper)
        : base(periodsRepository, mapper)
        {
            _mapper = mapper;
            _periodsRepository = periodsRepository;
        }
    }
}
