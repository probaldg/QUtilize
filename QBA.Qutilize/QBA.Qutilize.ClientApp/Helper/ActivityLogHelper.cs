using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.ClientApp.Helper
{
    public static class ActivityLogHelper
    {
        public static string UpdateUserActivityLog(int UserID, string eventName, string MACAddress, string SystemPlateform)
        {
            string result = "";
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["QBADBConnetion"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(conStr);
                DataTable dt = new DataTable();
                var sqlCmd = new SqlCommand("USP_UserActivityLog_Update", sqlCon)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCmd.Parameters.AddWithValue("@LoggerId", Guid.NewGuid());
                sqlCmd.Parameters.AddWithValue("@LogedUserId", UserID);
                sqlCmd.Parameters.AddWithValue("@IPAddress", DBNull.Value);
                sqlCmd.Parameters.AddWithValue("@UrlAccessed", eventName);
                sqlCmd.Parameters.AddWithValue("@UserAgent", DBNull.Value);
                sqlCmd.Parameters.AddWithValue("@IsMobileDevice", "Desktop");
                sqlCmd.Parameters.AddWithValue("@Browser", DBNull.Value);
                sqlCmd.Parameters.AddWithValue("@MACAddress", MACAddress);
                sqlCmd.Parameters.AddWithValue("@Platform", SystemPlateform);
                sqlCmd.Parameters.AddWithValue("@AccessDateTime", DateTime.Now);
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds != null)
                {
                    result = ds.Tables[0].Rows[0]["MESSAGE"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static string GetOsName()
        {
            OperatingSystem os_info = System.Environment.OSVersion;
            string version =
                os_info.Version.Major.ToString() + "." +
                os_info.Version.Minor.ToString();
            switch (version)
            {
                case "10.0": return "10/Server 2016";
                case "6.3": return "8.1/Server 2012 R2";
                case "6.2": return "8/Server 2012";
                case "6.1": return "7/Server 2008 R2";
                case "6.0": return "Server 2008/Vista";
                case "5.2": return "Server 2003 R2/Server 2003/XP 64-Bit Edition";
                case "5.1": return "XP";
                case "5.0": return "2000";
            }
            return "Unknown";
        }
    }
}
