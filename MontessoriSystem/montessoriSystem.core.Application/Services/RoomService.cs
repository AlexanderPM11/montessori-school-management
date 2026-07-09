using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using MontessoriSystem.Core.Application.ViewModels.Room;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class RoomService: GenericService<RoomSaveViewModel, RoomoViewModel, MontessoriSystem.Core.Domain.Entities.Room>, IRoomService
    {
        private readonly IRoomRespository _roomRespository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRespository roomRespository, IMapper mapper)
        : base(roomRespository, mapper)
        {
            _mapper = mapper;
            _roomRespository = roomRespository;
        }
    


    }
}
