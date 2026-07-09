using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Helpers
{
    public interface IDateAndTimeManage
    {
        List<DateTime> GetWeekdaysOfWeek(int year, int month, int weekNumber);
       public List<string> GetMonthsFromJanuaryToCurrent();
       public List<string> GetCurrentOrPreviousWeekDays();
       DateTime GetDateFromDayOfWeek(string dayOfWeekString);
       int GetMonthNumber(string monthName);
        int CalculateAge(string fechaString);
    }
}
