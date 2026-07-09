using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.AchievementIndicators;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class AchievementIndicatorsService: GenericService<SaveAchievementIndicatorsViewModel, AchievementIndicatorsViewModel, AchievementIndicators>, IAchievementIndicatorsService
    {

        private readonly IAchievementIndicatorsRepository _achievementIndicatorsRepository;
        private readonly IMapper _mapper;

        public AchievementIndicatorsService(IAchievementIndicatorsRepository achievementIndicatorsRepository, IMapper mapper)
        : base(achievementIndicatorsRepository, mapper)
        {
            _mapper = mapper;
            _achievementIndicatorsRepository = achievementIndicatorsRepository;
        }

    }
}
