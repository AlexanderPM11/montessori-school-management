using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;
using MontessoriSystem.Core.Application.ViewModels.RoomSchedule;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class RoomScheduleService : GenericService<SaveViewModelRoomSchedule, RoomScheduleViewModel, RoomSchedule>, IRoomScheduleService
    {
        private readonly IRoomSchedule _roomSchedule;
        private readonly IMapper _mapper;

        public RoomScheduleService(IRoomSchedule roomSchedule, IMapper mapper)
        : base(roomSchedule, mapper)
        {
            _mapper = mapper;
            _roomSchedule = roomSchedule;
        }

    }

}
