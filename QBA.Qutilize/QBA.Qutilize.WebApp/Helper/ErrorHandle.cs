using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;

namespace QBA.Qutilize.WebApp.Helper
{
    public class ErrorHandle : IDisposable
    {
        public bool WriteErrorLog(string LogMessage)
        {
            bool Status = false;
            string LogDirectory = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogDirectory"].ToString());

            DateTime CurrentDateTime = DateTime.Now;
            string CurrentDateTimeString = CurrentDateTime.ToString();
            CheckCreateLogDirectory(LogDirectory);
            string logLine = BuildLogLine(CurrentDateTime, LogMessage);
            LogDirectory = (LogDirectory + "Log_" + LogFileName(DateTime.Now) + ".txt");

            lock (typeof(ErrorHandle))
            {
                StreamWriter oStreamWriter = null;
                try
                {
                    oStreamWriter = new StreamWriter(LogDirectory, true);
                    oStreamWriter.WriteLine(logLine);
                    oStreamWriter.WriteLine("#############################################################################################");
                    oStreamWriter.WriteLine("###################################### END OF LINE ##########################################");
                    oStreamWriter.WriteLine("#############################################################################################");
                    Status = true;
                }
                catch
                {

                }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }
            return Status;
        }

        public bool WriteErrorLog(Exception err)
        {
            bool Status = false;
            string LogDirectory = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogDirectory"].ToString());

            DateTime CurrentDateTime = DateTime.Now;
            string CurrentDateTimeString = CurrentDateTime.ToString();
            CheckCreateLogDirectory(LogDirectory);


            string errorMessage = string.Empty;
            errorMessage = "TIME: " + CurrentDateTime.ToString() + "\r\n ERR:" + err.ToString() + "\r\n Message:" + err.Message + "\r\n Source: " + err.Source + "\r\n Inner Message: " + err.InnerException + "\r\n Help Link: " + err.HelpLink;

            //string logLine = BuildLogLine(CurrentDateTime, LogMessage.);


            LogDirectory = (LogDirectory + "Log_" + LogFileName(DateTime.Now) + ".txt");

            lock (typeof(ErrorHandle))
            {
                StreamWriter oStreamWriter = null;
                try
                {
                    oStreamWriter = new StreamWriter(LogDirectory, true);
                    oStreamWriter.WriteLine(errorMessage);
                    oStreamWriter.WriteLine("#############################################################################################");
                    oStreamWriter.WriteLine("###################################### END OF LINE ##########################################");
                    oStreamWriter.WriteLine("#############################################################################################");
                    Status = true;

                    HttpContext.Current.Session["Exception"] = errorMessage;
                    // HttpContext.Current.Session["ExceptionMode"] = (HttpContext.Current.Session["UserName"] == null) ? "SessionTimeOut" : "PageError";
                    HttpContext.Current.Response.Redirect("~/Account/NotFound404", true);

                }
                catch
                {

                }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }
            return Status;
        }

        private bool CheckCreateLogDirectory(string LogPath)
        {
            bool loggingDirectoryExists = false;
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(LogPath);
            if (oDirectoryInfo.Exists)
            {
                loggingDirectoryExists = true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(LogPath);
                    loggingDirectoryExists = true;
                }
                catch
                {
                    // Logging failure
                }
            }
            return loggingDirectoryExists;
        }

        private string BuildLogLine(DateTime CurrentDateTime, string LogMessage)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append(LogFileEntryDateTime(CurrentDateTime));
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(LogMessage);
            return loglineStringBuilder.ToString();
        }

        public string LogFileEntryDateTime(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }

        private string LogFileName(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd_MM_yyyy");
        }

        public void Init(HttpApplication context)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}