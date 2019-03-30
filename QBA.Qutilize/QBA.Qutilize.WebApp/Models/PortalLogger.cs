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
        public string AreaAccessed { get; set; }
        public string UserAgent { get; set; }
        public string browser { get; set; }
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