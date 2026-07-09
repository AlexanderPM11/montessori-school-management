using AutoMapper;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Attendance;

namespace MontessoriSystem.Core.Application.Services
{
    public class AttendanceService:GenericService<AttendanceSaveViewModel, AttendanceViewModel, MontessoriSystem.Core.Domain.Entities.Attendance>, IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;

        public AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper)
        : base(attendanceRepository, mapper)
        {
            _mapper = mapper;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<GeneralResponse<bool>> MakeAllPresent(List<AttendanceSaveViewModel> attendanceSaveViewModels)
        {
            var response = await _attendanceRepository.MakeAllPresent(attendanceSaveViewModels);

            return response;
        }
    }
}
