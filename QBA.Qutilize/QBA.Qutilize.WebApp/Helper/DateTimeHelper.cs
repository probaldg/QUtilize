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
                if (DateTime.TryParse(strDate, out DateTime dateTimeConverted))
                {
                    date = dateTimeConverted;
                }
                else
                {
                    strDate.Replace('-', '/');
                    var stringDateArray = strDate.Split('/');

                    var newBirthDayString = stringDateArray[1] + "/" + stringDateArray[0] + "/" + stringDateArray[2];
                    DateTime newDate = Convert.ToDateTime(newBirthDayString);
                    date = newDate;
                }
            }

            return date;
        }
    }
}