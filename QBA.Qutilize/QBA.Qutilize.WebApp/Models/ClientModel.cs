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
        [Display(Name = "Customer Name")]
        public string ClientName { get; set; }

        [Display(Name = "Customer Code")]
        public string ClientCode { get; set; }

        [Display(Name = "Customer Address")]
        public string ClientAddress { get; set; }

        [Display(Name = "Customer Phone No.")]
        public string ClientPhno { get; set; }
        public string Location { get; set; }
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

        public DataTable GetClientManagerByID(int clientID)
        {
            DataTable dt = null;
            try
            {

                SqlParameter[] param ={
                                        new SqlParameter("@ClientID",clientID)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPtblMasterClientManager_Get]", param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public bool InsertClientdata(ClientModel model, out int id, List<ClientManagerDetail> lstClientMgrDetails)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            DataTable dt = null;
            id = 0;
            bool result = false;
            try { 
                //Insert Parent
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
                    new SqlParameter("@isActive",model.IsActive),
                    new SqlParameter("@ClientAddress ",model.ClientAddress==null?"":model.ClientAddress),
                    new SqlParameter("@ClientContactNo ",model.ClientPhno),
                    new SqlParameter("@ClientLocation ",model.Location)
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

            //Insert Child

            try
            {
                   
                if (model.ClientID != 0 && lstClientMgrDetails.Count > 0)
                {
                    foreach (ClientManagerDetail vMD in lstClientMgrDetails)
                    {
                        try
                        {
                            SqlParameter[] paramNDetail ={//StatusGoal,
                                    new SqlParameter("@ClientID",model.ClientID),
                                    new SqlParameter("@Name",vMD.ClientMgrName),
                                    new SqlParameter("@Address",vMD.ClientMgrAddress),
                                    new SqlParameter("@phone",vMD.ClientMgrPhno),
                                    new SqlParameter("@email",vMD.ClientMgrEmail),
                                    new SqlParameter("@createdBy",model.CreatedBy),
                                    new SqlParameter("@createdDate",model.CreateDate),
                                    new SqlParameter("@isActive",true)
                                    // new SqlParameter("@Identity",1)

                                    };

                                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPtblMasterClientManager_Insert]", paramNDetail);
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
        }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }


        public bool UpdateClientdata(ClientModel model, List<ClientManagerDetail> lstClientMgrDetails)
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
                    new SqlParameter("@ClientID",model.ClientID),
                     new SqlParameter("@ClientAddress",model.ClientAddress==null?"":model.ClientAddress),
                    new SqlParameter("@ClientContactNo",model.ClientPhno),
                    new SqlParameter("@ClientLocation",model.Location)

                };

                  dt = objSQLHelper.ExecuteDataTable("[dbo].[USPMasterClient_Update]", param);
                  result = true;
               
                    try
                    {
                        DataTable dt1 = null;
                        SqlParameter[] param1 ={
                        new SqlParameter("@ClientID",model.ClientID)   };
                        dt1 = objSQLHelper.ExecuteDataTable("[dbo].[USPtblMasterClientManager_Delete]", param1);
                        result = true;
                        if (result == true)
                    {
                        //Update Child

                        try
                        {

                            if (model.ClientID != 0 && lstClientMgrDetails.Count > 0)
                            {
                                foreach (ClientManagerDetail vMD in lstClientMgrDetails)
                                {
                                    try
                                    {
                                        SqlParameter[] paramNDetail ={//StatusGoal,
                                    new SqlParameter("@ClientID",model.ClientID),
                                    new SqlParameter("@Name",vMD.ClientMgrName),
                                    new SqlParameter("@Address",vMD.ClientMgrAddress),
                                    new SqlParameter("@phone",vMD.ClientMgrPhno),
                                    new SqlParameter("@email",vMD.ClientMgrEmail),
                                    new SqlParameter("@createdBy",model.EditedBy),
                                    new SqlParameter("@createdDate",model.EditedDate),
                                    new SqlParameter("@isActive",true)
                                   

                                    };

                                        dt = objSQLHelper.ExecuteDataTable("[dbo].[USPtblMasterClientManager_Insert]", paramNDetail);
                                        result = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        result = false;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result = false;
                        }
                    }      

                    }
                    catch(Exception)
                    {
                        result = false;
                        throw;
                    }
               
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;

        }
    }

    public class ClientManagerDetail
    {
        public string ClientMgrName { get; set; }
        public string ClientMgrAddress { get; set; }
        public string ClientMgrPhno { get; set; }
        public string ClientMgrEmail { get; set; }
   
    }
}