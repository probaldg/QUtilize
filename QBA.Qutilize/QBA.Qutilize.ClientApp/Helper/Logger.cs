using System;
using System.IO;
using System.Reflection;

namespace QBA.Qutilize.ClientApp.Helper
{
    public class Logger
    {
        public static void Log(string functionName, string resultType, string message)
        {

            //Getting app data folder path
            // LogFilepath= C:\Users\rahul\AppData\Roaming\QUtilize\

            try
            {
                var appDataFolderPath =Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"QUtilize");
                if (!Directory.Exists(appDataFolderPath))
                    Directory.CreateDirectory(appDataFolderPath);

                string strFile = appDataFolderPath + @"\Log.txt";

                if (!(File.Exists(strFile)))
                {
                    StreamWriter w = File.CreateText(strFile);
                    w.WriteLine($"{DateTime.Now} {functionName}  {resultType}  {message}");
                    w.Close();
                }
                else
                {
                    using (StreamWriter w = File.AppendText(strFile))
                    {
                        w.WriteLine($"{DateTime.Now} {functionName}  {resultType}  {message}");
                        w.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
