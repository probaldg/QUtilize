using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class TaskStatusModel
    {
        public int StatusId { get; set; }

        [Display(Name = "Code")]
        public string StatusCode { get; set; }

        [Display(Name = "Name")]
        public string StatusName { get; set; }

        public int Rank { get; set; }
        //public int OrgId { get; set; }
        public bool IsActive { get; set; }

        public int StatusOrgId { get; set; }

        [Display(Name = "Organisation")]
        public List<OrganisationModel> OrganisationList { get; set; }

        public int AddedBy { get; set; }
        public int EditedBy { get; set; }
        public DateTime AddedTS { get; set; }
        public DateTime EditedTS { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        public TaskStatusModel()
        {
            OrganisationList = new List<OrganisationModel>();
        }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public DataTable GetTaskStatus(int? orgID = 0, int? statusID = 0)
        {
            DataTable dt = null;
            try
            {


                SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgID ==0? null: orgID),
                                        new SqlParameter("@StatusID",statusID ==0? null: statusID),

                                      };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterStatus", param);
            }
            catch (Exception)
            {

                throw;
            }
            return dt;
        }

        public bool InsertTaskStatusData(TaskStatusModel model, out int id)
        {
            DataTable dt = null;
            id = 0;
            bool result = false;
            try
            {
                SqlParameter Status = new SqlParameter("@Identity", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                SqlParameter[] param ={Status,
                    new SqlParameter("@StatusCode",model.StatusCode),
                    new SqlParameter("@StatusName",model.StatusName),
                    new SqlParameter("@Rank",model.Rank),
                    new SqlParameter("@OrgID", model.StatusOrgId),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@AddedBy",model.AddedBy),
                    new SqlParameter("@AddedTS",model.AddedTS)

                };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterStatus_Insert", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    if (id > 0)
                    {
                        model.StatusId = id;
                        result = true;
                    }
                    else
                    {
                        id = 0;
                        result = false;
                    }
                }
                else
                {
                    id = 0;
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public Boolean UpdateTaskStatusData(TaskStatusModel model)
        {
            DataTable dt = null;
            bool result = false;

            try
            {
                SqlParameter[] param ={
                    new SqlParameter("@StatusCode",model.StatusCode),
                    new SqlParameter("@StatusName",model.StatusName),
                    new SqlParameter("@Rank",model.Rank),
                    new SqlParameter("@OrgID", model.StatusOrgId),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedTS",model.EditedTS),
                    new SqlParameter("@StatusID",model.StatusId)

                };

                dt = objSQLHelper.ExecuteDataTable("USPtblMasterStatus_Update", param);
                result = true;
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        public List<OrganisationModel> GetOrgInList(int orgId = 0)
        {
            DataTable dt = null;
            List<OrganisationModel> Organisations = new List<OrganisationModel>();
            try
            {

                if (orgId != 0)
                {

                    SqlParameter[] param ={
                                            new SqlParameter("@id",orgId)
                                          };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[sp_getOrganisationByID]", param);

                }
                else
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[SP_GetAllOrganisation]");
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        OrganisationModel organisation = new OrganisationModel();
                        organisation.id = Convert.ToInt32(item["id"]);
                        organisation.orgname = item["orgname"].ToString();
                        organisation.isActive = Convert.ToBoolean(item["IsActive"]);
                        Organisations.Add(organisation);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Organisations;
        }
    }
}