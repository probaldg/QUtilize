using QBA.Qutilize.WebApp.Models;
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
        public static bool SetUserSessionLog(UserSessionLog mUSL)
        {
            bool bRetVal = true;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@LoggerId",mUSL.LoggerId),
                                        new SqlParameter("@LogedUserId",mUSL.LogedUserId),
                                        new SqlParameter("@IPAddress",mUSL.IPAddress),
                                        new SqlParameter("@Application",mUSL.Application),
                                        new SqlParameter("@StartTime",mUSL.StartTime),
                                        new SqlParameter("@EndTime",mUSL.EndTime),
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    DataSet ds = objSQLHelper.ExecuteDataset("USP_UserSessionLog_Insert", param);
                    bRetVal = true;
                }
            }
            catch (Exception ex)
            {
                bRetVal = false;
            }

            return bRetVal;
        }
        public static bool SetUserSessionLogout(UserSessionLog mUSL)
        {
            bool bRetVal = true;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@LogedUserId",mUSL.LogedUserId),
                                        new SqlParameter("@Application",mUSL.Application),
                                        new SqlParameter("@EndTime",mUSL.EndTime),
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    DataSet ds = objSQLHelper.ExecuteDataset("USP_UserSessionLog_Update", param);
                    bRetVal = true;
                }
            }
            catch (Exception ex)
            {
                bRetVal = false;
            }

            return bRetVal;
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

        #region Organization
        public static DataTable GetALLOrganisationData()
        {
            DataTable dt = null;

            try
            {
                using (SqlHelper objSQLHelper = new SqlHelper())
                { dt = objSQLHelper.ExecuteDataTable("[dbo].[SP_GetAllOrganisation]"); }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public static DataTable GetALLActiveOrganisationData()
        {
            DataTable dt = null;

            try
            {
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[SP_GetAllActiveOrganisation]");
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public static Boolean insert_OrganisationData(OrganisationModel model, out int id)
        {
            string str = string.Empty;
            bool bln = false;
            DataTable dt = null;
            id = 0;
            try
            {
                SqlParameter Status = new SqlParameter("@ID", SqlDbType.Int);
                Status.Direction = ParameterDirection.Output;

                SqlParameter[] param ={Status,

                                        new SqlParameter("@orgname",model.orgname),
                                        new SqlParameter("@address",model.address),
                                        new SqlParameter("@url", model.url),
                                        new SqlParameter("@logo",model.logo),
                                        new SqlParameter("@wikiurl",model.wikiurl),
                                        new SqlParameter("@contact_email_id",model.contact_email_id),
                                        new SqlParameter("@createdTS",model.createdTS),
                                        new SqlParameter("@createdBy",model.createdBy),
                                        new SqlParameter("@isActive",model.isActive)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_tblOrganisationInsert]", param);
                }

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.id = id;

                    bln = true;
                }
                else
                {
                    id = 0;
                    bln = false;

                }
            }
            catch (Exception ex)
            {

                bln = false;
            }
            return bln;
        }


        public static DataTable GetOrganisationDataByID(int id)
        {
            bool bln = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                    //new SqlParameter("@ID",model.ID),
                    new SqlParameter("@id",id),


                };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_getOrganisationByID]", param);
                }

                //model.ISErr = false;
                //model.ErrString = "Data Saved Successfully!!!";
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public static Boolean updateOrganisation(OrganisationModel model)
        {
            string str = string.Empty;
            bool bln = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                   new SqlParameter("@ID",model.id),
                   new SqlParameter("@orgname",model.orgname),
                   new SqlParameter("@address",model.address),
                   new SqlParameter("@url", model.url),
                   new SqlParameter("@logo",model.logo),
                   new SqlParameter("@wikiurl",model.wikiurl),
                   new SqlParameter("@contact_email_id",model.contact_email_id),
                   new SqlParameter("@editedTS",model.editedTS),
                   new SqlParameter("@editedBY",model.editedBy),
                   new SqlParameter("@isActive",model.isActive)
                };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_tblOrganisationUpdate]", param);
                }

                bln = true;

            }
            catch (Exception ex)
            {

                bln = false;
            }
            return bln;
        }

        public static DataTable GetOrganisationDataByURL(string url)
        {
            bool bln = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                    //new SqlParameter("@ID",model.ID),
                    new SqlParameter("@url",url),


                };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_getOrganisationByURL]", param);
                }
                //model.ISErr = false;
                //model.ErrString = "Data Saved Successfully!!!";
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public static DataTable GetALLOrganisationForCategory()
        {
            DataTable dt = null;

            try
            {
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[SP_GetAllOrganisationForCategory]");
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public static int GetOrganisationIDByName(string orgName)
        {
            int orgId = 0;
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                    new SqlParameter("@OrganizationName",orgName),
                };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_GetOrganizationByName]", param);
                }
                if (dt.Rows.Count > 0)
                {
                    orgId = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                }
            }
            catch (Exception ex)
            {

            }
            return orgId;
        }
        #endregion

        
    }
}