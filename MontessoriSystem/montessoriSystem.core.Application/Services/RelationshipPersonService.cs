using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.RelationshipPerson;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class RelationshipPersonService:GenericService<RelationshipPersonSaveViewModel, RelationshipPersonViewModel, RelationshipPerson>, IRelationshipPersonService
        
    {

        private readonly IRelationshipPersonRepository _relationshipPersonRepository;
        private readonly IMapper _mapper;

        public RelationshipPersonService(IRelationshipPersonRepository relationshipPersonRepository, IMapper mapper)
        : base(relationshipPersonRepository, mapper)
        {
            _mapper = mapper;
            _relationshipPersonRepository = relationshipPersonRepository;
        }
    }
}
