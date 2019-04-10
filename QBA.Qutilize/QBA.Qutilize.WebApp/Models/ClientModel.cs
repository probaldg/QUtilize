using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class ClientModel
    {
        public int ClientID { get; set; }
        [Display(Name = "Name")]
        public string ClientName { get; set; }

        [Display(Name = "Code")]
        public string ClientCode { get; set; }

        public int ClientOrganisationID { get; set; }
        public string OrganisationName { get; set; }

        [Display(Name = "Organisation")]
        public List<OrganisationModel> OrganisationList { get; set; }

        public string OrgName_ClientNameForCombo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        public ClientModel()
        {
            OrganisationList = new List<OrganisationModel>();
        }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public List<OrganisationModel> GetAllOrgInList(int orgId = 0)
        {
            DataTable dt = null;
            List<OrganisationModel> Organisations = new List<OrganisationModel>();
            try
            {

                dt = objSQLHelper.ExecuteDataTable("[dbo].[SP_GetAllOrganisation]");

                foreach (DataRow item in dt.Rows)
                {
                    OrganisationModel organisation = new OrganisationModel
                    {
                        id = Convert.ToInt32(item["id"]),
                        orgname = item["orgname"].ToString(),
                        isActive = Convert.ToBoolean(item["isActive"])
                    };
                    Organisations.Add(organisation);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Organisations;
        }


        public DataTable GetAllClients(int orgID = 0)
        {
            DataTable dt = null;
            try
            {
                if (orgID != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgID",orgID)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPtblMasterClient_Get]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPtblMasterClient_Get]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetClientByID(int clientID)
        {
            DataTable dt = null;
            try
            {

                SqlParameter[] param ={
                                        new SqlParameter("@ClientID",clientID)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPtblMasterClient_Get]", param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public bool InsertClientdata(ClientModel model, out int id)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            DataTable dt = null;
            id = 0;
            bool result = false;
            try
            {
                SqlParameter Status = new SqlParameter("@Identity", SqlDbType.Int);
                Status.Direction = ParameterDirection.Output;
                SqlParameter[] param ={Status,
                    new SqlParameter("@clientCode",model.ClientCode),
                    new SqlParameter("@clientName",model.ClientName),
                    new SqlParameter("@orgId",model.ClientOrganisationID),
                    new SqlParameter("@createdBy",model.CreatedBy),
                    new SqlParameter("@createdDate",model.CreateDate),
                    new SqlParameter("@isActive",model.IsActive)

                };

                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPtblMasterClient_Insert]", param);

                if (!(Status.Value is DBNull) && Status != null)
                {
                    id = Convert.ToInt32(Status.Value);
                    model.ClientID = id;
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }


        public bool UpdateClientdata(ClientModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            bool result = false;
            try
            {
                DataTable dt = null;
                SqlParameter[] param ={
                    new SqlParameter("@clientCode",model.ClientCode),
                    new SqlParameter("@clientName",model.ClientName),
                    new SqlParameter("@orgID",model.ClientOrganisationID),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),
                    new SqlParameter("@isActive",model.IsActive),
                    new SqlParameter("@ClientID",model.ClientID)

                };

                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPMasterClient_Update]", param);
                result = true;
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;

        }
    }
}