namespace backend.Utils
{
    public class DateUtils
    {
        public static DateTime GetCLDateNow()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "America/Santiago");
        }
        public static int CalculateDaysToDueCL(DateTime paymentDueDate)
        {
            var today = GetCLDateNow().Date;
            var days = (paymentDueDate.Date - today).Days;
            return days > 0 ? days : 0;
        }
    }
}
