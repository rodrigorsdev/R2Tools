namespace R2.Tools.Date
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DateTool
    {
        public static DateTime? ClearDateAndReturnHourAndMinutes(string valor)
        {
            DateTime? date = null;

            if (!string.IsNullOrEmpty(valor))
            {
                var textHour = valor.Substring(0, 2);
                var textMinute = valor.Substring(3, 2);

                var hora = int.Parse(textHour);
                var min = int.Parse(textMinute);

                date = new DateTime(1900, 1, 1, hora, min, 0);
            }

            return date;
        }

        public static DateTime ConvertStringToDatetime(string date)
        {
            date = date.Replace("/", string.Empty);
            int year = Convert.ToInt32(date.Substring(4));
            int month = Convert.ToInt32(date.Substring(2, 2));
            int day = Convert.ToInt32(date.Substring(0, 2));
            return new DateTime(year, month, day);
        }

        public static List<TimeZoneInfo> ListTimeZone
        {
            get
            {
                return TimeZoneInfo.GetSystemTimeZones().ToList();
            }
        }

        public static DateTime? ConvertDatetimeWithTimeZone(DateTime date, TimeZoneInfo timeZone)
        {
            DateTime? result = null;

            try
            {
                result = TimeZoneInfo.ConvertTime(date, timeZone);
            }
            catch
            {
                result = date;
            }

            return result;
        }

        public static DateTime? ConvertDatetimeWithLocalTimeZone(DateTime date)
        {
            DateTime? result = null;

            try
            {
                result = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local);
            }
            catch
            {
                result = date;
            }

            return result;
        }
    }
}
