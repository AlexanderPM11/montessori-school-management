using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence.Repositories
{
    public class AttendanceRepository:GenericRepository<Attendance>, IAttendanceRepository
    {
        private readonly ApplicationContext _applicationContext;

        public AttendanceRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            this._applicationContext = applicationContext;
        }


        public async Task<GeneralResponse<bool>> MakeAllPresent(List<AttendanceSaveViewModel> attendanceSaveViewModels)
        {
            GeneralResponse<bool> response = new GeneralResponse<bool>();

            try
            {
                foreach (var item in attendanceSaveViewModels)
                {
                    item.IdSuject = item.IdSuject == 0 ? null : item.IdSuject;

                    var resutl = _applicationContext.Set<Attendance>().Where(atten => atten.IdSuject == item.IdSuject
                    && atten.IdStudent == item.IdStudent && atten.Date.Date == item.Date).FirstOrDefault();

                    var itemToAction = new Attendance
                    {
                        Date = item.Date,
                        IdStudent = item.IdStudent,
                        IdSuject = item.IdSuject,
                        IsPresent = true,
                        IsAbsent = false,
                        IsDelay = false,
                        IsExcuse = false,
                        IdRoom = item.IdRoom,
                    };
                   
                    if (resutl == null)
                    {
                        _applicationContext.Set<Attendance>().Add(itemToAction);
                    }
                    else
                    {
                        resutl.Date = item.Date;
                        resutl.IdStudent = item.IdStudent;
                        resutl.IdSuject = item.IdSuject;
                        resutl.IsPresent = true;
                        resutl.IsAbsent = false;
                        resutl.IsDelay = false;
                        resutl.IsExcuse = false;
                        resutl.IdRoom = item.IdRoom;

                        _applicationContext.Set<Attendance>().Update(resutl);
                    }
                }
                await _applicationContext.SaveChangesAsync();
            }            
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add(ex.Message);
                return response;
            }    

            response.result = true;
            return response;
        }
    }
}
