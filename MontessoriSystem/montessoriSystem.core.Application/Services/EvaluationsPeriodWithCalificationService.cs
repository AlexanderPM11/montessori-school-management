using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.EvaluationsPeriodWithCalification;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class EvaluationsPeriodWithCalificationService:GenericService<EvaluationsPeriodWithCalificationSaveViewModel, EvaluationsPeriodWithCalificationViewModel, EvaluationsPeriodWithCalification>, IEvaluationsPeriodWithCalificationService
    {
        private readonly IEvaluationsPeriodWithCalificationRepository _evaluationsPeriodWithCalificationRepository;
        private readonly IMapper _mapper;

        public EvaluationsPeriodWithCalificationService(IEvaluationsPeriodWithCalificationRepository evaluationsPeriodWithCalificationRepository, IMapper mapper)
        : base(evaluationsPeriodWithCalificationRepository, mapper)
        {
            _mapper = mapper;
            _evaluationsPeriodWithCalificationRepository = evaluationsPeriodWithCalificationRepository;
        }
    }
}
