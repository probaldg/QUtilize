using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.Models
{
    public class PortalLogger
    { }
    public class UserActivityLog
    {
        public Guid LoggerId { get; set; }
        public string LogedUserId { get; set; }
        public string IPAddress { get; set; }
        public string UrlAccessed { get; set; }
        public string UserAgent { get; set; }
        public string IsMobileDevice { get; set; }
        public string Browser { get; set; }
        public string MACAddress { get; set; }
        public string Platform { get; set; }
        public DateTime AccessDateTime { get; set; }
        public bool SetUserActivityLog(UserActivityLog mUSL)
        {
            bool bRetVal = true;
            try
            {
                return DataAccess.SetUserActivityLog(mUSL);
            }
            catch (Exception ex)
            {
                bRetVal = false;
            }
            return bRetVal;
        }
    }
    public class UserSessionLog
    {
        public Guid LoggerId { get; set; }
        public int LogedUserId { get; set; }
        public string IPAddress { get; set; }
        public string Application { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool SetUserSessionLog(UserSessionLog mUSL)
        {
            bool bRetVal = true;
            try
            {
                return DataAccess.SetUserSessionLog(mUSL);
            }
            catch (Exception ex)
            {
                bRetVal = false;
            }

            return bRetVal;
        }
        public bool SetUserSessionLogout(UserSessionLog mUSL)
        {
            bool bRetVal = true;
            try
            {
                return DataAccess.SetUserSessionLogout(mUSL);
            }
            catch (Exception ex)
            {
                bRetVal = false;
            }

            return bRetVal;
        }
    }
}