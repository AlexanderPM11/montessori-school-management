
using MontessoriSystem.Core.Application.ViewModels.Common;

namespace MontessoriSystem.Core.Application.ViewModels.RoomSchedule
{
    public class SaveViewModelRoomSchedule:AuditableBaseModel
    {
        public   string InitDate {  get; set; }
        public string FinishDate {  get; set; }
        public int IdSubject { get; set; }
        public string? IdTeacher { get; set; }

        public int IdRoom { get; set; }

    }
}
