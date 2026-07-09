using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.RoomTeacher;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class RoomTeacherService: GenericService<RoomTeacherSaveViewModel, RoomTeacherViewModel, RoomTeacher>, IRoomTeacherService
    {
        private readonly IRoomTeacherRepository _roomTeacherRepository;
        private readonly IMapper _mapper;

        public RoomTeacherService(IRoomTeacherRepository roomTeacherRepository, IMapper mapper)
        : base(roomTeacherRepository, mapper)
        {
            _mapper = mapper;
            _roomTeacherRepository = roomTeacherRepository;
        }
    }
}
