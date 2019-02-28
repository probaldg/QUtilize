using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.Models
{
    public class UserModel
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Name { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email Id")]
        public string EmailId { get; set; }
      
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public DataTable GetAllUsers()
        {
            DataTable dt = null;
            try
            {
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]");
            }
            catch (Exception ex)
            {
                
            }
            return dt;
        }
        public DataTable GetUsersByID(int id)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",id)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]",param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public Boolean InsertUserdata(UserModel model, out int id)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
            id = 0;

            try
            {
                SqlParameter Status = new SqlParameter("@Identity", SqlDbType.Int);
                Status.Direction = ParameterDirection.Output;
                SqlParameter[] param ={Status,
                    new SqlParameter("@UserName",model.UserName),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@EmailId", model.EmailId),
                    new SqlParameter("@Password",model.Password),
                    new SqlParameter("@CreatedBy",model.CreatedBy),
                    new SqlParameter("@CreatedDate",model.CreateDate),
                   
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.ID = id;
                    model.ISErr = false;
                    model.ErrString = "Data Saved Successfully!!!";
                    result = true;
                }
                else
                {
                    id = 0;
                    result = false;
                    model.ISErr = true;
                    model.ErrString = "Error Occured!!!";
                }
            }
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured!!!";
                result = false;
            }
            return result;

        }

        public Boolean Update_UserDetails(UserModel model)
        {
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@ID",model.ID),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@userName",model.UserName),
                    new SqlParameter("@EmailId", model.EmailId),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),
                    new SqlParameter("@isActive",model.IsActive)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_Update]", param);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        public DataTable checkemail(string email)
        {
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Email",email)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_CheckEmail]", param);
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTable updatePassword(int ID, string newPwd, int editedBy, DateTime editedTS)
        {
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",ID),
                                        new SqlParameter("@password",newPwd),
                                        new SqlParameter("@EditedBy",editedBy),
                                        new SqlParameter("@EditedDate",editedTS)

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_UpdatePassword]", param);
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
    }
}