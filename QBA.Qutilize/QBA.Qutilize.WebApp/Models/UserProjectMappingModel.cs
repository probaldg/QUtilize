using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class UserProjectMappingModel
    {
        [Key]
        public int UserId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        //public int OrgID { get; set; }
        //public string OrgName { get; set; }
        //public SelectList OrganisationList { get; set; }
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
        
        public DataTable GetAllProjects(int orgId = 0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@OrgID",orgId)
                                      };

                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjects_Get]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable GetAllUsers(int orgId = 0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@OrgID",orgId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]",param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable GetAllProjectByUserID(int UserID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                         new SqlParameter("@UserID",UserID),

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUserProjects_Get]", param);
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
                                         new SqlParameter("@UserID",UserID),

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUserProjects_DeleteMappingByUserID]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public Boolean InsertUserProjectMappingdata( UserProjectMappingModel userProjectMapping)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
           

            try
            {
                
                SqlParameter[] param ={
                    new SqlParameter("@UserID",userProjectMapping.UserId),
                    new SqlParameter("@ProjectID",userProjectMapping.ProjectId),
                   
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUserProjects_Insert]", param);
                userProjectMapping.ISErr = false;
                userProjectMapping.ErrString = "Data Saved Successfully!!!";
                result = true;
              
            }
            catch (Exception ex)
            {
                userProjectMapping.ISErr = true;
                userProjectMapping.ErrString = "Error Occured!!!";
                result = false;
            }
            return result;

        }
    }

  
}