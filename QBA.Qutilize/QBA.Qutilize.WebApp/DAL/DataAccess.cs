using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.DAL
{
    public static class DataAccess
    {
        public static DataSet VerifyLogin(string userName, string password)
        {
            DataSet dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userName),
                                        new SqlParameter("@Password",password)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataset("USPUsers_VerifyLogin", param);
                }
            }
            catch (Exception ex)
            {

            }

            return dt;
        }
        public static DataSet GetDashBoardData(int userID, DateTime startDate, DateTime endDate)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",Convert.ToInt32(userID)),
                                        new SqlParameter("@StartDate",Convert.ToDateTime(startDate)),
                                        new SqlParameter("@EndDate",Convert.ToDateTime(endDate))
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_Get", param);
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public static DataSet GetUserDetailData(int userID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={new SqlParameter("@Id", Convert.ToInt32(userID))};
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPUserDetailData", param);
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public static DataTable GetUsers()
        {
            DataSet ds = null;
            try
            { 
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPUsers_GetForWeb");
                }
            }
            catch (Exception ex)
            {

            }

            return ds.Tables[0];
        }
        public static DataTable GetDashBoardMenu(int UserID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",UserID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataTable("sp_GetDashBoardMenu", param);
                }
            }
            catch (Exception ex)
            {

            }

            return dt;
        }
    }
}