using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Grade;
using MontessoriSystem.Core.Application.ViewModels.Province;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class ProvinceDomService : GenericService<ProvinceDomSaveViewModel, ProvinceDomViewModel, ProvinceDom>,    IProvinceDomService
    {
        private readonly IProvinceDomRepository _provinceDomRepository;
        private readonly IMapper _mapper;
        public ProvinceDomService(IProvinceDomRepository provinceDomRepository, IMapper mapper) :
            base(provinceDomRepository, mapper)
        {
            _provinceDomRepository = provinceDomRepository;
            _mapper = mapper;
        }
    }
}
