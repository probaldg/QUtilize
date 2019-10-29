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