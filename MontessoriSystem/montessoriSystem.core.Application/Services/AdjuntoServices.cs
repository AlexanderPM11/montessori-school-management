using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;

namespace MontessoriSystem.Core.Application.Services
{
    public class AdjuntoServices : GenericService<SaveAdjuntoViewModel,AdjuntoViewModel, MontessoriSystem.Core.Domain.Entities.Adjunto>, IAdjuntoServices
    {
        private readonly IAdjuntoRepository _adjuntoRepository;
        private readonly IMapper _mapper;

        public AdjuntoServices(IAdjuntoRepository adjuntoRepository, IMapper mapper)
        : base(adjuntoRepository, mapper)
        {
            _mapper = mapper;
            _adjuntoRepository = adjuntoRepository;
        }


    }
}
