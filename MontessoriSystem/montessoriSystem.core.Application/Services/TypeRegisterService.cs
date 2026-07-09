using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.TypeRegister;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class TypeRegisterService:GenericService<TypeRegisterSaveViewModel, TypeRegisterViewModel, TypeRegister>, ITypeRegisterService
    {
        private readonly ITypeRegisterRepository _typeRegisterRepository;
        private readonly IMapper _mapper;

        public TypeRegisterService(ITypeRegisterRepository typeRegisterRepository, IMapper mapper)
        : base(typeRegisterRepository, mapper)
        {
            _mapper = mapper;
            _typeRegisterRepository = typeRegisterRepository;
        }
    }
}
