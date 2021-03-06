﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using QBA.Qutilize.WebApp.DAL;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class AccountViewModels
    {
        public DataTable GetDashBoardMenu(int UserID)
        {
            DataTable dt = null;
            try
            {
                dt = DataAccess.GetDashBoardMenu(UserID);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }

        public DataSet VerifyLogin(string userName, string password)
        {
            DataSet dt = null;
            try
            {
                dt = DataAccess.VerifyLogin(userName, password);
            }
            catch (Exception ex)
            {

            }

            return dt;
        }
        public DataSet GetUserDetailData(int userID)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetUserDetailData(userID);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet GetDashBoardData(int userID, DateTime startDate, DateTime endDate, string strUser, string strProject)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetDashBoardData(userID, startDate, endDate, strUser, strProject);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        #region DashBoard Data
        #region DashBoard Data - Admin
        public DataSet GetDashBoardAdminProjectData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_AdminProject_Get", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetDashBoardAdminTaskData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_AdminTask_Get", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetDashBoardAdminTicketData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_AdminTicket_Get", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetDashBoardAdminTisheetData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_AdminTimesheet_Get", param);
                }
            }
            catch (Exception ex) { }
            return ds;
        }
        #endregion
        #region DashBoard Data - Admin
        public DataSet GetDashBoardPMProjectData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_PMProject_Get", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetDashBoardPMTaskData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_PMTask_Get", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetDashBoardPMTicketData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_PMTicket_Get", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetDashBoardPMTisheetData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_PMTimesheet_Get", param);
                }
            }
            catch (Exception ex) { }
            return ds;
        }
        #endregion
        #region DashBoard Data - Self
        public DataSet GetDashBoardSelfTaskData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_SelfTask_Get", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetDashBoardSelfTicketData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_SelfTicket_Get", param);
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetDashBoardSelfTisheetData(int userID, int orgID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID),
                                        new SqlParameter("@orgID",orgID)
                                      };
                using (SqlHelper objSQLHelper = new SqlHelper())
                {
                    ds = objSQLHelper.ExecuteDataset("USP_Dashboard_SelfTimesheet_Get", param);
                }
            }
            catch (Exception ex){}
            return ds;
        }
        #endregion
        #endregion
        public DataSet GetOnlineUser(int UserID, DateTime CurrDate)
        {
            DataSet dt = null;
            try
            {
                dt = DataAccess.GetOnlineUser(UserID, CurrDate);
            }
            catch (Exception ex)
            {

            }

            return dt;
        }
        public DataSet GetReportData(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportData(userID, startDate, endDate, ProjectID, MainUser, Role);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet GetReportDataProjectWiseSummary(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role, int OrgID = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportDataProjectWiseSummary(userID, startDate, endDate, ProjectID, MainUser, Role, OrgID);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet GetReportDataProjectWiseCosting(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, int OrgID = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportDataProjectWiseCosting(userID, startDate, endDate, ProjectID, MainUser, OrgID);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet GetReportDataResourceUtilizationSummary(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, int OrgID = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportDataResourceUtilizationSummary(userID, startDate, endDate, ProjectID, MainUser, OrgID);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet GetReportDataResourceCosting(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, int OrgId = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportDataResourceCosting(userID, startDate, endDate, ProjectID, MainUser, OrgId);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet TimeSheet_ByResource_CurrentMonth(int orgid = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.TimeSheet_ByResource_CurrentMonth(orgid);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet Weekly_TimeSheet_ByTask_CurrentWeekOrPreviousWeek(int period, int deptid, int projectid, int userid, int TaskID, int OrgId = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Weekly_TimeSheet_ByTask_CurrentWeekOrPreviousWeek(period, deptid, projectid, userid, TaskID, OrgId);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet Monthly_TimeSheet_ByTask_CurrentMonthOrPreviousMonth(int period, int deptid, int projectid, int userid, int TaskID, int OrgId = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Monthly_TimeSheet_ByTask_CurrentMonthOrPreviousMonth(period, deptid, projectid, userid, TaskID, OrgId);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        //
        public DataSet Weekly_TimeSheet_ByDepartment_CurrentWeekOrPreviousWeek(int period, int deptid, int userid, int OrgId = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Weekly_TimeSheet_ByDepartment_CurrentWeekOrPreviousWeek(period, deptid, userid, OrgId);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet Weekly_TimeSheet_ByProject_CurrentWeekOrPreviousWeek(int period, int deptid, int projectid, int userid, int OrgID = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Weekly_TimeSheet_ByProject_CurrentWeekOrPreviousWeek(period, deptid, projectid, userid, OrgID);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet Monthly_TimeSheet_ByProject_CurrentMonthOrPreviousMonth(int period, int deptid, int projectid, int userid, int OrgId = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Monthly_TimeSheet_ByProject_CurrentMonthOrPreviousMonth(period, deptid, projectid, userid, OrgId);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        //

        public DataSet Monthly_TimeSheet_ByClient_CurrentMonthOrPreviousMonth(int period, int clientID, int ProjectId, int userid, int OrgId = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Monthly_TimeSheet_ByClient_CurrentMonthOrPreviousMonth(period, clientID, ProjectId, userid, OrgId);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet Monthly_TimeSheet_ByDepartment_CurrentMonthOrPreviousMonth(int period, int deptid, int userid, int OrgId = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Monthly_TimeSheet_ByDepartment_CurrentMonthOrPreviousMonth(period, deptid, userid, OrgId);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet TimeSheet_ByResource_PreviousMonth(int orgid = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.TimeSheet_ByResource_PreviousMonth(orgid);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet getSessionHistory(int userID, int limit = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.getSessionHistory(userID, limit);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet getActivityHistory(int userID, int limit = 0)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.getActivityHistory(userID, limit);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

    }

}