using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.ObservationCommentEvaluation;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class ObservationCommentEvaluationService:GenericService<SaveViewModelObservationCommentEvaluation, ObservationCommentEvaluationViewModel, ObservationCommentEvaluation>, IObservationCommentEvaluationService
    {
        private readonly IObservationCommentEvaluationRepository _evaluationRepository;
        private readonly IMapper _mapper;

        public ObservationCommentEvaluationService(IObservationCommentEvaluationRepository evaluationRepository, IMapper mapper)
        : base(evaluationRepository, mapper)
        {
            _mapper = mapper;
            _evaluationRepository = evaluationRepository;
        }


    }
}
