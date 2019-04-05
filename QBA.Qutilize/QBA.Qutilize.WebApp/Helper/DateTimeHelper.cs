using System;

namespace QBA.Qutilize.WebApp.Helper
{
    public class DateTimeHelper
    {
        public static DateTime ConvertStringToValidDate(string strDate)
        {
            DateTime date = new DateTime();
            if (date != null)
            {
                var stringDateArray = strDate.Split('/');
                var monthPart = Convert.ToInt16(stringDateArray[0]);
                var dayPart = Convert.ToInt16(stringDateArray[1]);
                var yearPart = Convert.ToInt32(stringDateArray[2]);

                date = new DateTime(yearPart, monthPart, dayPart);

            }

            return date;
        }
    }
}