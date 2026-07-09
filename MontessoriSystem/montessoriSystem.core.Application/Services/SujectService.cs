using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;
using MontessoriSystem.Core.Application.ViewModels.Suject;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class SujectService : GenericService<SujectSaveViewModel, SujectViewModel, MontessoriSystem.Core.Domain.Entities.Suject>, ISujectService
    {
        private readonly ISujectRepository _sujectRepository;
        private readonly IMapper _mapper;

        public SujectService(ISujectRepository sujectRepository, IMapper mapper)
        : base(sujectRepository, mapper)
        {
            _mapper = mapper;
            _sujectRepository = sujectRepository;
        }
    
    }
}
