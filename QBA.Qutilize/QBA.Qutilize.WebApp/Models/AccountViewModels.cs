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

        public DataTable VerifyLogin(string userName, string password)
        {
            DataTable dt = null;
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
        public DataSet GetDashBoardData(int userID, DateTime startDate, DateTime endDate)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetDashBoardData(userID,  startDate,  endDate);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }
        
    }
    
}