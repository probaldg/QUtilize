using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class RoleModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public List<OrganisationModel> OrganisationList { get; set; }

        public int RolesOrgID { get; set; }
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

        public RoleModel()
        {
            OrganisationList = new List<OrganisationModel>();
        }
        public DataTable GetAllRoles(int? OrgId = null)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@OrgId",OrgId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPRoles_Get]");
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable GetRolesByID(int id)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",id)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPRoles_Get]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public Boolean InsertRoletdata(RoleModel model, out int id)
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
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@Description",model.Description),
                   new SqlParameter("@CreatedBy",model.CreatedBy),
                    new SqlParameter("@CreatedDate",model.CreateDate),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@OrgId",model.RolesOrgID)

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPRoles_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.Id = id;
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

        public Boolean Update_RoleDetails(RoleModel model)
        {
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@Id",model.Id),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),
                    new SqlParameter("@IsActive",model.IsActive),
                     new SqlParameter("@OrgId",model.RolesOrgID)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPRoles_Update]", param);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public List<OrganisationModel> GetAllOrgInList(int orgId = 0)
        {
            DataTable dt = null;
            List<OrganisationModel> Organisations = new List<OrganisationModel>();
            try
            {

                dt = objSQLHelper.ExecuteDataTable("[dbo].[SP_GetAllOrganisation]");

                foreach (DataRow item in dt.Rows)
                {
                    OrganisationModel organisation = new OrganisationModel();
                    organisation.id = Convert.ToInt32(item["id"]);
                    organisation.orgname = item["orgname"].ToString();
                    Organisations.Add(organisation);
                }
            }
            catch (Exception ex)
            {

            }
            return Organisations;
        }
    }
}