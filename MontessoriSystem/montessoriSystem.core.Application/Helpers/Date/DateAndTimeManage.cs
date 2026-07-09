using MontessoriSystem.Core.Application.Interface.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Helpers.Date
{
    public class DateAndTimeManage: IDateAndTimeManage
    {

        public List<DateTime> GetWeekdaysOfWeek(int year, int month, int weekNumber)
        {
            if (weekNumber < 1 || weekNumber > 5)
                throw new ArgumentOutOfRangeException(nameof(weekNumber), "Week number must be between 1 and 5.");

            // Primer día del mes
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime baseMondayForFirstWeek; // Este será el lunes de inicio real para la "Semana 1"

            // Busca el primer lunes del mes...
            DateTime firstMondayInCurrentMonth = firstDayOfMonth;
            while (firstMondayInCurrentMonth.DayOfWeek != DayOfWeek.Monday)
            {
                firstMondayInCurrentMonth = firstMondayInCurrentMonth.AddDays(1);
            }

            // Esta sección calcula el lunes de la semana a la que pertenece el firstDayOfMonth,
            // incluso si ese lunes cae en el mes anterior.
            int daysToSubtractFromFirstDay = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            DateTime mondayOfCalendarWeekContainingFirstDay = firstDayOfMonth.AddDays(-daysToSubtractFromFirstDay);

            // EVALUACIÓN CLAVE para determinar el Lunes Base de la "Semana 1":
            // Si el 1er día del mes es Lunes (ej. Dic 2025),
            // O si el 1er día del mes es Domingo Y el 2do del mes es Lunes (ej. Feb 2026),
            // entonces la "Semana 1" debe empezar en el primer lunes del mes actual.
            if (firstDayOfMonth.DayOfWeek == DayOfWeek.Monday ||
                (firstDayOfMonth.DayOfWeek == DayOfWeek.Sunday && firstMondayInCurrentMonth.Day == 2))
            {
                baseMondayForFirstWeek = firstMondayInCurrentMonth; // El primer lunes del mes es el inicio de la Semana 1.
            }
            else
            {
                // En cualquier otro caso (ej. 1er día del mes es Mar, Mié, Jue, Vie, Sáb),
                // necesitamos retroceder para que la "Semana 1" empiece en Lunes (con data dummy del mes anterior).
                baseMondayForFirstWeek = mondayOfCalendarWeekContainingFirstDay;
            }

            // Ahora, todas las semanas se calculan a partir de este 'baseMondayForFirstWeek'.
            // Si weekNumber es 1, AddDays((1-1)*7) = AddDays(0).
            // Si weekNumber es 5, AddDays((5-1)*7) = AddDays(28).
            DateTime requestedWeekStart = baseMondayForFirstWeek.AddDays((weekNumber - 1) * 7);

            // Generar los 5 días de Lunes a Viernes
            var weekDays = new List<DateTime>();
            for (int i = 0; i < 5; i++)
            {
                weekDays.Add(requestedWeekStart.AddDays(i));
            }

            return weekDays;
        }
        public List<string> GetMonthsFromJanuaryToCurrent()
        {
            List<string> months = new List<string>();
            DateTime today = DateTime.Today;

            for (int month = 1; month <= today.Month; month++)
            {
                DateTime date = new DateTime(today.Year, month, 1);
                string monthName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(date.ToString("MMMM", new CultureInfo("es-ES")));
                months.Add(monthName);
            }

            return months;
        }
        public int GetMonthNumber(string monthName)
        {
            monthName = monthName.ToLower();
            // Define el CultureInfo para español
            var cultureInfo = new CultureInfo("es-ES");
            // Obtén los nombres de los meses en español
            var monthNames = cultureInfo.DateTimeFormat.MonthNames;

            // Busca el índice del mes proporcionado en el array de nombres de meses
            int monthNumber = Array.IndexOf(monthNames, monthName) + 1;

            // Si el mes no se encuentra, devuelve 0 (o puedes manejarlo de otra forma)
            return monthNumber > 0 && monthNumber <= 12 ? monthNumber : 0;
        }
        public List<string> GetCurrentOrPreviousWeekDays()
        {
            List<string> weekDays = new List<string>();
            DateTime today = DateTime.Today;

            // If today is Saturday or Sunday, move to the previous Friday
            if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                today = today.AddDays(-1); // Move to Friday
            }
            else if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                today = today.AddDays(-2); // Move to Friday
            }

            // Get the days from today back to Monday
            while (today.DayOfWeek >= DayOfWeek.Monday)
            {
                weekDays.Add(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(today.ToString("dddd", new CultureInfo("es-ES"))));
                today = today.AddDays(-1);
            }

            // Define the correct order of days in Spanish
            //List<string> orderedDays = new List<string> { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes" };
            //weekDays = weekDays.OrderBy(day => orderedDays.IndexOf(day)).ToList();

            return weekDays;
        }
        public DateTime GetDateFromDayOfWeek(string dayOfWeekString)
        {
            // Mapeo de días en español a días en inglés
            Dictionary<string, DayOfWeek> spanishToEnglishDays = new Dictionary<string, DayOfWeek>()
            {
                {"Lunes", DayOfWeek.Monday},
                {"Martes", DayOfWeek.Tuesday},
                {"Miércoles", DayOfWeek.Wednesday},
                {"Jueves", DayOfWeek.Thursday},
                {"Viernes", DayOfWeek.Friday}
            };

            // Obtener el día de la semana en inglés desde el mapeo
            if (!spanishToEnglishDays.TryGetValue(dayOfWeekString, out DayOfWeek requestedDayOfWeek))
            {
                throw new ArgumentException("Nombre de día inválido. Asegúrate de usar nombres de días en español.");
            }

            // Obtener la fecha y hora actual
            DateTime now = DateTime.Now;

            // Obtener el primer día de esta semana (domingo) y restar días según el día de la semana solicitado
            DateTime requestedDate = now.Date; // Iniciar con la fecha actual sin la hora exacta
            DayOfWeek currentDayOfWeek = now.DayOfWeek;

            // Calcular la diferencia de días entre el día actual y el día solicitado
            int daysDifference = currentDayOfWeek - requestedDayOfWeek;
            if (daysDifference < 0)
            {
                daysDifference += 7; // Si es negativo, añadir 7 días para retroceder a la semana anterior
            }

            // Restar los días de diferencia para obtener la fecha del día solicitado
            requestedDate = requestedDate.AddDays(-daysDifference);

            return requestedDate;
        }
        public int CalculateAge(string fechaString)
        {
            // Intenta parsear cualquier formato de fecha válido (incluye ISO 8601)
            if (!DateTime.TryParse(fechaString, out DateTime fechaDeNacimiento))
            {
                return -1;
            }

            DateTime fechaActual = DateTime.Today;
            int edad = fechaActual.Year - fechaDeNacimiento.Year;

            if (fechaActual.Month < fechaDeNacimiento.Month ||
                (fechaActual.Month == fechaDeNacimiento.Month && fechaActual.Day < fechaDeNacimiento.Day))
            {
                edad--;
            }

            return edad;
        }
        


        #region Private Method
        private int ObtenerNumeroMes(string nombreMes)
        {
            switch (nombreMes.ToLower())
            {
                case "enero":
                    return 1;
                case "febrero":
                    return 2;
                case "marzo":
                    return 3;
                case "abril":
                    return 4;
                case "mayo":
                    return 5;
                case "junio":
                    return 6;
                case "julio":
                    return 7;
                case "agosto":
                    return 8;
                case "septiembre":
                    return 9;
                case "octubre":
                    return 10;
                case "noviembre":
                    return 11;
                case "diciembre":
                    return 12;
                default:
                    throw new ArgumentException($"Nombre de mes no válido: {nombreMes}");
            }
        }

        #endregion


    }
}
