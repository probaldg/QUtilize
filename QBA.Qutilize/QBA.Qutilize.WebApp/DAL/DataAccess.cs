using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
        public static DataSet GetOnlineUser(int UserID, DateTime CurrDate)
        {
            DataSet dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",UserID),
                                        new SqlParameter("@CurrDate",CurrDate)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    dt = objSQLHelper.ExecuteDataset("USP_OnlineUser_Get", param);
                }
            }
            catch (Exception ex)
            {

            }

            return dt;
        }
        public static bool SetUserActivityLog(UserActivityLog mUSL)
        {
            bool bRetVal = true;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@LoggerId",mUSL.LoggerId),
                                        new SqlParameter("@LogedUserId",mUSL.LogedUserId),
                                        new SqlParameter("@IPAddress",mUSL.IPAddress),
                                        new SqlParameter("@UrlAccessed",mUSL.UrlAccessed),
                                        new SqlParameter("@UserAgent",mUSL.UserAgent),
                                        new SqlParameter("@IsMobileDevice",mUSL.IsMobileDevice),
                                        new SqlParameter("@Browser",mUSL.Browser),
                                        new SqlParameter("@MACAddress",mUSL.MACAddress),
                                        new SqlParameter("@Platform",mUSL.Platform),
                                        new SqlParameter("@AccessDateTime",mUSL.AccessDateTime)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    DataSet ds = objSQLHelper.ExecuteDataset("USP_UserActivityLog_Update", param);
                    bRetVal = true;
                }
            }
            catch (Exception ex)
            {
                bRetVal = false;
            }

            return bRetVal;
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
        public static DataSet GetDashBoardData(int userID, DateTime startDate, DateTime endDate, string strUser, string strProject)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",Convert.ToInt32(userID)),
                                        new SqlParameter("@StartDate",Convert.ToDateTime(startDate)),
                                        new SqlParameter("@EndDate",Convert.ToDateTime(endDate)),
                                        new SqlParameter("@sUser",Convert.ToInt32(strUser)),
                                        new SqlParameter("@sProject",Convert.ToInt32(strProject))
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

        public static DataSet GetReportData(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",Convert.ToInt32(userID)),
                                        new SqlParameter("@StartDate",Convert.ToDateTime(startDate)),
                                        new SqlParameter("@EndDate",Convert.ToDateTime(endDate)),
                                        new SqlParameter("@ProjectID",ProjectID),
                                        new SqlParameter("@MainUser",MainUser),
                                        new SqlParameter("@Role",Role)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPDailyTask_GetByDateRangeAndUserAndProject", param);
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public static DataSet GetReportDataProjectWiseSummary(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        //new SqlParameter("@UserID",Convert.ToInt32(userID)),
                                        new SqlParameter("@StartDate",Convert.ToDateTime(startDate)),
                                        new SqlParameter("@EndDate",Convert.ToDateTime(endDate)),
                                        new SqlParameter("@ProjectID",ProjectID)
                                        //new SqlParameter("@MainUser",MainUser),
                                        //new SqlParameter("@Role",Role)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPDailyTask_GetByDateRangeAndProjectWiseSummary", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public static DataSet GetReportDataProjectWiseCosting(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                     
                                        new SqlParameter("@StartDate",Convert.ToDateTime(startDate)),
                                        new SqlParameter("@EndDate",Convert.ToDateTime(endDate)),
                                        new SqlParameter("@ProjectID",ProjectID)
                                        
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPDailyTask_GetByDateRangeAndProjectWiseCosting", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public static DataSet GetReportDataResourceUtilizationSummary(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        //new SqlParameter("@UserID",Convert.ToInt32(userID)),
                                        new SqlParameter("@StartDate",Convert.ToDateTime(startDate)),
                                        new SqlParameter("@EndDate",Convert.ToDateTime(endDate)),
                                        //new SqlParameter("@ProjectID",ProjectID)
                                        //new SqlParameter("@MainUser",MainUser),
                                        //new SqlParameter("@Role",Role)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPDailyTask_GetByDateRangeAndResourceUtilization", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public static DataSet GetReportDataResourceCosting(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                       
                                        new SqlParameter("@StartDate",Convert.ToDateTime(startDate)),
                                        new SqlParameter("@EndDate",Convert.ToDateTime(endDate)),
                                        
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPDailyTask_GetByDateRangeAndResourceCosting", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public static DataSet TimeSheet_ByResource_CurrentMonth()
        {
            DataSet ds = null;
            try
            {
                
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("usp_rpt_Custom_TimeSheet_ByResource_CurrentMonth");
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public static DataSet Weekly_TimeSheet_ByDepartment_CurrentWeekOrPreviousWeek(int period, int deptid, int userid)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={

                                        new SqlParameter("@preiod",period),
                                        new SqlParameter("@deptId",deptid),
                                        new SqlParameter("@UserID",userid),

                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("usp_rpt_Weekly_TimeSheet_ByDepartment", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public static DataSet Monthly_TimeSheet_ByClient_CurrentMonthOrPreviousMonth(int period, int clientID,int ProjectId, int userid)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={

                                        new SqlParameter("@preiod",period),
                                        new SqlParameter("@ClientId",clientID),
                                        new SqlParameter("@ProjectID",ProjectId),
                                        new SqlParameter("@UserID",userid),

                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("usp_rpt_Monthly_TimeSheet_ByClient", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public static DataSet Monthly_TimeSheet_ByDepartment_CurrentMonthOrPreviousMonth(int period, int deptid, int userid)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={

                                        new SqlParameter("@preiod",period),
                                        new SqlParameter("@deptId",deptid),
                                        new SqlParameter("@UserID",userid),

                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("usp_rpt_Monthly_TimeSheet_ByDepartment", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        //
        public static DataSet Weekly_TimeSheet_ByTask_CurrentWeekOrPreviousWeek(int period, int deptid, int projectid, int userid, int TaskId)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={

                                        new SqlParameter("@preiod",period),
                                        new SqlParameter("@deptId",deptid),
                                        new SqlParameter("@ProjectID",projectid),
                                        new SqlParameter("@UserID",userid),
                                        new SqlParameter("@TaskId",TaskId),

                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("usp_rpt_Weekly_TimeSheet_ByTask", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public static DataSet Monthly_TimeSheet_ByTask_CurrentMonthOrPreviousMonth(int period, int deptid, int projectid, int userid,int TaskId)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={

                                        new SqlParameter("@preiod",period),
                                        new SqlParameter("@deptId",deptid),
                                        new SqlParameter("@ProjectID",projectid),
                                        new SqlParameter("@UserID",userid),
                                        new SqlParameter("@TaskId",TaskId),

                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("usp_rpt_Monthly_TimeSheet_ByTask", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public static DataSet Monthly_TimeSheet_ByProject_CurrentMonthOrPreviousMonth(int period,int deptid,int projectid,int userid)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={

                                        new SqlParameter("@preiod",period),

                                        new SqlParameter("@deptId",deptid),
                                        new SqlParameter("@ProjectID",projectid),
                                        new SqlParameter("@UserID",userid),

                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("usp_rpt_Monthly_TimeSheet_ByProject", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public static DataSet Weekly_TimeSheet_ByProject_CurrentWeekOrPreviousWeek(int period, int deptid, int projectid, int userid)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={

                                        new SqlParameter("@preiod",period),

                                        new SqlParameter("@deptId",deptid),
                                        new SqlParameter("@ProjectID",projectid),
                                        new SqlParameter("@UserID",userid),

                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("usp_rpt_Weekly_TimeSheet_ByProject", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public static DataSet TimeSheet_ByResource_PreviousMonth()
        {
            DataSet ds = null;
            try
            {
                //SqlParameter[] param ={

                //                        new SqlParameter("@StartDate",Convert.ToDateTime(startDate)),
                //                        new SqlParameter("@EndDate",Convert.ToDateTime(endDate)),

                //                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("usp_rpt_Custom_TimeSheet_ByResource_PreviousMonth");
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
                SqlParameter[] param = { new SqlParameter("@Id", Convert.ToInt32(userID)) };
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


        public static DataSet getSessionHistory(int userID, int limit = 0)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param = { new SqlParameter("@UserId", Convert.ToInt32(userID)), new SqlParameter("@limit", Convert.ToInt32(limit)) };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPUserSessionHistory", param);
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public static DataSet getActivityHistory(int userID, int limit = 0)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param = { new SqlParameter("@UserId", Convert.ToInt32(userID)), new SqlParameter("@limit", Convert.ToInt32(limit)) };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPUserActivityHistory", param);
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
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
                //model.ErrString = "Data Saved Successfully.";
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
                //model.ErrString = "Data Saved Successfully.";
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
        public static DataSet GetSkillManagementDetailData(int userID, int ORGID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param = { new SqlParameter("@UserId", userID), new SqlParameter("@ORGID", ORGID) };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USPSkillManagementDetail_Get", param);
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public static bool InsertUserSkillData(List<MapUserSkill> lstUserSkill)
        {
            string str = string.Empty;
            bool bln = false;
            DataTable dt = null;
            try
            {
                if (lstUserSkill.Count > 0)
                {
                    //delete existing drafted  goals
                    SqlParameter[] paramGoalDel ={//StatusGoal,
                                    new SqlParameter("@userID",lstUserSkill[0].userID)};
                    using (SqlHelper objSQLHelper = new SqlHelper())
                    {
                        dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MapUserSkill_Del]", paramGoalDel);
                    }
                    foreach (MapUserSkill mUserSkill in lstUserSkill)
                    {
                        SqlParameter[] param ={
                                    new SqlParameter("@skillID",mUserSkill.skillID),
                                    new SqlParameter("@userID",mUserSkill.userID),
                                    new SqlParameter("@SkillRatingID",mUserSkill.SkillRatingID),
                                    new SqlParameter("@CreatedBy",mUserSkill.CreatedBy),
                                    new SqlParameter("@CreateDate",mUserSkill.CreateDate)
                                    };
                        using (SqlHelper objSQLHelper = new SqlHelper())
                        { dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MapUserSkill_Insert]", param); }
                    }
                    bln = true;
                }
            }
            catch (Exception ex)
            {
                bln = false;
            }
            return bln;
        }

    }
}