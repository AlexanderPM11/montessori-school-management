using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.TitlesAchieved;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class TitlesAchievedService:GenericService<TitlesAchievedSaveViewModel, TitlesAchievedViewModel, TitlesAchieved>,ITitlesAchievedsService
    {
        private readonly ITitlesAchievedRepository _titlesAchievedRepository;
        private readonly IMapper _mapper;

        public TitlesAchievedService(ITitlesAchievedRepository titlesAchievedRepository, IMapper mapper)
        : base(titlesAchievedRepository, mapper)
        {
            _mapper = mapper;
            _titlesAchievedRepository = titlesAchievedRepository;
            
        }

    }
}
