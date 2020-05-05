using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace QBA.Qutilize.WebApp.Helper
{
    public class SendMailClass: IDisposable
    {
        public void Dispose()
        {
           // throw new NotImplementedException();
        }

        public bool SendMail(string to, string subject, string mailBody, string from = "", string pass = "")
        {
            bool success = false;
            try
            {
                if (from.Trim() == string.Empty)
                { from = ConfigurationManager.AppSettings["smtpFrom"]; }
                if (pass.Trim() == string.Empty)
                { pass = ConfigurationManager.AppSettings["smtpPass"]; }
                using (MailMessage mail = new MailMessage(from, to))
                {
                    mail.Subject = subject;
                    mail.Body = mailBody;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["smtpServer"];// "smtp.gmail.com";
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                    NetworkCredential networkCredential = new NetworkCredential(from, pass);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);

                    try
                    {
                        smtp.Send(mail);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                    ex.ToString());
                        success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

    }
}