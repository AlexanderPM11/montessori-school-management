using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Application.ViewModels.TipoAdjunto;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class TipoAdjuntoServices : GenericService<SaveTipoAdjuntoViewModel, TipoAdjuntoViewModel, TipoAdjunto>, ITipoAdjuntoService
    {
        private readonly ITipoAdjuntoRepository _tipoAdjuntoRepository;
        private readonly IMapper _mapper;

        public TipoAdjuntoServices(ITipoAdjuntoRepository tipoAdjuntoRepository, IMapper mapper)
        : base(tipoAdjuntoRepository, mapper)
        {
            _tipoAdjuntoRepository=tipoAdjuntoRepository;
            _mapper = mapper;
        }

    }
}
