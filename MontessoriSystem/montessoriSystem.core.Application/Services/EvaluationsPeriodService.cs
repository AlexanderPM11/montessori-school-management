using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.EvaluationsPeriod;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class EvaluationsPeriodService : GenericService<SaveEvaluationsPeriodViewModel, EvaluationsPeriodViewModel, EvaluationsPeriod>, IEvaluationsPeriodService
    {
        private readonly IEvaluationsPeriodRepository _evaluationsPeriodRepository;
        private readonly IMapper _mapper;

        public EvaluationsPeriodService(IEvaluationsPeriodRepository evaluationsPeriodRepository, IMapper mapper)
        : base(evaluationsPeriodRepository, mapper)
        {
            _mapper = mapper;
            _evaluationsPeriodRepository = evaluationsPeriodRepository;
        }
    }
}
