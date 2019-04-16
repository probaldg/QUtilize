using QBA.Qutilize.WebApp.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class UserRoleMappingModel
    {
        [Key]
        public int UserId { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }

        public int EditedBy { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedTS { get; set; }
        public DateTime EditedTS { get; set; }

        //public ModuleMapping MP { get; set; }
        public bool isActive { get; set; }

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


        public DataTable GetAllRoles(int? OrgId = null)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@OrgId",OrgId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPRoles_Get]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable DeleteAllExistingMapping(int UserID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                         new SqlParameter("@UserId",UserID),

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUserRoles_DeleteMappingByUserID]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable GetAllRolesByUserID(int UserID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                         new SqlParameter("@UserID",UserID),

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUserRoles_Get]", param);
            }
            catch (Exception ex)
            {


            }
            return dt;
        }

        public Boolean InsertUserRoleMappingdata(UserRoleMappingModel userRoleMapping)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;


            try
            {
                SqlParameter[] param ={
                    new SqlParameter("@UserID",userRoleMapping.UserId),
                    new SqlParameter("@RoleID",userRoleMapping.RoleId),

                };
                dt = objSQLHelper.ExecuteDataTable("USPUserRoles_Insert", param);
                userRoleMapping.ISErr = false;
                userRoleMapping.ErrString = "Data Saved Successfully.";
                result = true;

            }
            catch (Exception ex)
            {
                userRoleMapping.ISErr = true;
                userRoleMapping.ErrString = "Error Occured.";
                result = false;
            }
            return result;

        }
    }
}