using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using QBA.Qutilize.WebApp.DAL;

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
        [Display(Name = "User ID")]
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
                ds = DataAccess.GetDashBoardData(userID,  startDate,  endDate, strUser, strProject);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
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
        public DataSet GetReportData(int userID, DateTime startDate, DateTime endDate,int ProjectID,int MainUser,string Role)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportData(userID, startDate, endDate,ProjectID,MainUser,Role);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet GetReportDataProjectWiseSummary(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportDataProjectWiseSummary(userID, startDate, endDate, ProjectID, MainUser, Role);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet GetReportDataProjectWiseCosting(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportDataProjectWiseCosting(userID, startDate, endDate, ProjectID, MainUser, Role);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet GetReportDataResourceUtilizationSummary(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportDataResourceUtilizationSummary(userID, startDate, endDate, ProjectID, MainUser, Role);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet GetReportDataResourceCosting(int userID, DateTime startDate, DateTime endDate, int ProjectID, int MainUser, string Role)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetReportDataResourceCosting(userID, startDate, endDate, ProjectID, MainUser, Role);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet TimeSheet_ByResource_CurrentMonth()
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.TimeSheet_ByResource_CurrentMonth();
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet Weekly_TimeSheet_ByTask_CurrentWeekOrPreviousWeek(int period, int deptid, int projectid, int userid, int TaskID)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Weekly_TimeSheet_ByTask_CurrentWeekOrPreviousWeek(period, deptid, projectid, userid, TaskID);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet Monthly_TimeSheet_ByTask_CurrentMonthOrPreviousMonth(int period, int deptid, int projectid, int userid, int TaskID)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Monthly_TimeSheet_ByTask_CurrentMonthOrPreviousMonth(period, deptid, projectid, userid,TaskID);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        //
        public DataSet Weekly_TimeSheet_ByDepartment_CurrentWeekOrPreviousWeek(int period, int deptid, int userid)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Weekly_TimeSheet_ByDepartment_CurrentWeekOrPreviousWeek(period, deptid, userid);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet Weekly_TimeSheet_ByProject_CurrentWeekOrPreviousWeek(int period, int deptid, int projectid, int userid)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Weekly_TimeSheet_ByProject_CurrentWeekOrPreviousWeek(period, deptid, projectid, userid);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet Monthly_TimeSheet_ByProject_CurrentMonthOrPreviousMonth(int period,int deptid,int projectid,int userid)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Monthly_TimeSheet_ByProject_CurrentMonthOrPreviousMonth(period, deptid, projectid, userid);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        //

        public DataSet Monthly_TimeSheet_ByClient_CurrentMonthOrPreviousMonth(int period, int clientID, int ProjectId, int userid)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Monthly_TimeSheet_ByClient_CurrentMonthOrPreviousMonth(period, clientID, ProjectId, userid);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet Monthly_TimeSheet_ByDepartment_CurrentMonthOrPreviousMonth(int period, int deptid, int userid)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.Monthly_TimeSheet_ByDepartment_CurrentMonthOrPreviousMonth(period,deptid,userid);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        public DataSet TimeSheet_ByResource_PreviousMonth()
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.TimeSheet_ByResource_PreviousMonth();
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