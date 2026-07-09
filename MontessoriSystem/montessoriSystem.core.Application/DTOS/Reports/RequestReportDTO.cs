

namespace MontessoriSystem.Core.Application.DTOS.Reports
{
    public class RequestReportDTO
    {
        public int IdStudent { get; set; }
        public int IdAchievementIndicator { get; set; }
        public int Score { get; set; }
        public int IdSubject { get; set; }
        public string Period { get; set; }
        public string Estado { get; set; }
        public bool isRp { get; set; } = false;
        public int? rp { get; set; } = null;

    }
}
