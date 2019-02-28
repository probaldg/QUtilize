using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using QBA.Qutilize.WebApp.DAL;

namespace QBA.Qutilize.WebApp.Models
{
    public class UserRoleModel
    {
        [Key]
        public int ID { get; set; }
        public string RoleName { get; set; }

        public int OrgID { get; set; }
        public string OrgName { get; set; }

        public SelectList OrganisationList { get; set; }
   
        public Boolean IsSysAdmin { get; set; }
        public Boolean IsApprover { get; set; }
        public Boolean IsPublisher { get; set; }
        public int EditedBy { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedTS { get; set; }
        public DateTime EditedTS { get; set; }

        public bool isActive { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion


        public DataTable GetAllRoles()
        {
            DataTable dt = null;
            try
            {
                dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_tblsysmodule_select]");
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable GetALLRoleDataByID(int ID)
        {
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                                                new SqlParameter("@ID",ID)
                                              };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_tblsysmodule_select_ByID]", param);
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public Boolean update_UserRoleDetails(UserRoleModel model)
        {
            bool bln = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@ID",model.ID),
                    new SqlParameter("@RoleName",model.RoleName),
                    new SqlParameter("OrgID",model.OrgID),
                     new SqlParameter("@IsSysAdmin",model.IsSysAdmin),
                    new SqlParameter("@IsApprover",model.IsApprover),
                    new SqlParameter("@IsPublisher",model.IsPublisher),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedTS",model.EditedTS),
                    new SqlParameter("@IsActive",model.isActive)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_tblsysrole_update]", param);
                model.ISErr = false;
                model.ErrString = "Data Saved Successfully!!!";
            }
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured!!!";
                bln = false;
            }
            return bln;
        }


        public Boolean InsertRoledata(UserRoleModel model, out int id)
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
                    new SqlParameter("@RoleName",model.RoleName),
                    new SqlParameter("@OrgID",model.OrgID),
                     new SqlParameter("@IsSysAdmin",model.IsSysAdmin),
                    new SqlParameter("@IsApprover",model.IsApprover),
                    new SqlParameter("@IsPublisher",model.IsPublisher),
                    new SqlParameter("@AddedBy",model.AddedBy),
                    new SqlParameter("@AddedTS",model.AddedTS),
                    new SqlParameter("@IsActive",model.isActive)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_tblsysrole_insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.ID = id;

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

        public DataTable GetAllRolesForDropDown(int orgid)
        {
            DataTable dt = null;
            try
            {
                if (orgid < 0)
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_tblsysmodule_selectActive]");
                }
                else
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@orgid",orgid)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_tblsysmodule_selectActiveByOrgID]", param);
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
    }
}