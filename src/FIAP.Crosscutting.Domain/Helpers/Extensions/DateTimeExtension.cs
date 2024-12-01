using System.Globalization;
using System.Runtime.InteropServices;

namespace FIAP.Crosscutting.Domain.Helpers.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToBrazilianTimezone(this DateTime dateTime)
        {
            TimeZoneInfo targetTimeZone;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            else
                targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

            var targetDatetime = TimeZoneInfo.ConvertTime(dateTime, targetTimeZone);

            return targetDatetime;
        }

        public static string ToFriendlyDateTimeString(this DateTime date)
        {
            var isToday = Math.Round(DateTime.Now.ToBrazilianTimezone().Subtract(date).TotalDays) == 0;
            var isWeek = Math.Round(DateTime.Now.ToBrazilianTimezone().Subtract(date).TotalDays) <= 7;
            var isYesterday = Math.Round(DateTime.Now.ToBrazilianTimezone().Subtract(date).TotalDays) >= 1 && Math.Round(DateTime.Now.ToBrazilianTimezone().Subtract(date).TotalDays) < 2;

            if (isToday)
                return "hoje";

            if (isYesterday)
                return "ontem";

            if (isWeek)
                return date.ToString("dddd", new CultureInfo("pt-BR")).ToLower();

            return date.ToString(date.Year == DateTime.Now.Year
                ? "d 'de' MMMM"
                : "d 'de' MMM 'de' yyyy", new CultureInfo("pt-BR")).ToLower();
        }
    }
}
