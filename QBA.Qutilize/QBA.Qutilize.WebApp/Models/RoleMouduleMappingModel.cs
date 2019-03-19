using QBA.Qutilize.WebApp.DAL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class RoleMouduleMappingModel
    {
        public int RoleId { get; set; }
        public int SysModuleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public bool IsActive { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        //public DataTable GetAllRoles()
        //{
        //    DataTable dt = null;
        //    try
        //    {
        //        dt = objSQLHelper.ExecuteDataTable("[dbo].[USPRoles_Get]");
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return dt;
        //}

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
        public DataTable GetAllModules()
        {
            DataTable dt = null;
            try
            {
                dt = objSQLHelper.ExecuteDataTable("USPModules_Get");
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable GetAllModuleByRoleID(int UserID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                         new SqlParameter("@RoleId",UserID),

                                      };
                dt = objSQLHelper.ExecuteDataTable("USPRoleModuleMap_GetModuleByID", param);
            }
            catch (Exception ex)
            {


            }
            return dt;
        }

        public DataTable DeleteAllExistingMapping(int RoleID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                         new SqlParameter("@RoleId",RoleID),

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPRoleModuleMap_DeleteMappingByRoleID]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public Boolean InsertRoleModuleMappingdata(RoleMouduleMappingModel roleMouduleMapping)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;


            try
            {
                SqlParameter[] param ={
                    new SqlParameter("@RoleID",roleMouduleMapping.RoleId),
                    new SqlParameter("@SysModuleId",roleMouduleMapping.SysModuleId),
                    new SqlParameter("@AddedBy",roleMouduleMapping.CreatedBy),
                    new SqlParameter("@AddedTS",roleMouduleMapping.CreateDate),

                };
                dt = objSQLHelper.ExecuteDataTable("USPRoleMouduleMapping_Insert", param);
                roleMouduleMapping.ISErr = false;
                roleMouduleMapping.ErrString = "Data Saved Successfully!!!";
                result = true;

            }
            catch (Exception ex)
            {
                roleMouduleMapping.ISErr = true;
                roleMouduleMapping.ErrString = "Error Occured!!!";
                result = false;
            }
            return result;

        }
    }
}