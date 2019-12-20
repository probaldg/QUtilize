using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectModel
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int ProjectTypeID { get; set; }
        public int ProjectPricingID { get; set; }
        public string ProjectTypeName { get; set; }

        [Display(Name = "Project Type")]
        public List<ProjectTypeModel> ProjectTypeList { get; set; }
        [Display(Name = "Project Pricing")]
        public List<ProjectPricingModel> ProjectPricingList { get; set; }
        [Display(Name = "Currency")]
        public List<MasterCurrencyModel> CurrencyList { get; set; }
        public int CurrencyID { get; set; }


        [Display(Name = "Project Billing Type")]
        public List<ProjectBillingModel> ProjectBillingList { get; set; }

        public int ProjectBillingID { get; set; }
        public int? ParentProjectID { get; set; }


        public string ParentProjectName { get; set; }
        [Display(Name = "Maximum Time for Project In Hours")]
        public int MaxProjectTimeInHours { get; set; } = 8;
        public int DepartmentID { get; set; }
        public int PMUserID { get; set; }
        public int ClientD { get; set; }
        [Display(Name = "Department List")]
        public List<DepartmentModel> DepartmentList { get; set; }
        [Display(Name = "Manager List")]
        public List<UserModel> UserList { get; set; }
        [Display(Name = "Client List")]
        public List<ClientModel> ClientList { get; set; }
        [Display(Name = "Client PO No")]
        public string ClientPoNo { get; set; }
        [Display(Name = "Client PO Date")]
        public string ClientPoDateToDisplay { get; set; }
        public DateTime ClientPoDate { get; set; }

        [Display(Name = "Project Start Date")]
        public string ProjectStartDateToDisplay { get; set; }
        public DateTime ProjectStartDate { get; set; }

        [Display(Name = "Project End Date")]
        public string ProjectEndDateToDisplay { get; set; }
        public DateTime ProjectEndDate { get; set; }
        [Display(Name = "Project Rate")]
        public double ProjectRate { get; set; }

        public int ManagerID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        [Display(Name = "Severity")]
        public List<MasterSeverityModel> SeverityList { get; set; }
        public int SeverityID { get; set; }

        [Display(Name = "Project Name")]
        public List<ProjectModel> ActiveProjectList { get; set; }
        public int ActiveProjectID { get; set; }

        public SelectList ProjectList { get; set; }
        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public ProjectTaskModel ProjectTask { get; set; }
        public ProjectIssueModel ProjectIssue { get; set; }

        public ProjectModel()
        {
            DepartmentList = new List<DepartmentModel>();
            UserList = new List<UserModel>();
        }
    
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
        //**
        public List<ProjectModel> Get_ActiveProject(int orgId = 0)
        {
            DataTable dt = null;
            List<ProjectModel> lstProjectDetl = new List<ProjectModel>();
            try
            {
                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjects_Get]", param);

                }
               
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ProjectModel projectModel = new ProjectModel();
                        projectModel.ProjectID = Convert.ToInt32(item["Id"]);
                        projectModel.ProjectCode = item["ProjectCode"].ToString();
                        projectModel.ProjectName = item["Name"].ToString();
                        projectModel.IsActive = Convert.ToBoolean(item["IsActive"]);
                        projectModel.ProjectTypeName= item["ProjectTypeName"].ToString();
                        projectModel.ProjectTypeID = Convert.ToInt32(item["ProjectTypeID"]);
                       // projectModel.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["SeverityName"].ToString();
                        lstProjectDetl.Add(projectModel);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstProjectDetl;
        }
        public List<ProjectModel> Get_ActiveProjectMappedwithUser(int userID = 0)
        {
            DataTable dt = null;
            List<ProjectModel> lstProjectDetl = new List<ProjectModel>();
            try
            {
                if (userID != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@UserID",userID)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjects_ActiveAndAssignedtoUser_Get]", param);

                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ProjectModel projectModel = new ProjectModel();
                        projectModel.ProjectID = Convert.ToInt32(item["Id"]);
                        projectModel.ProjectCode = item["ProjectCode"].ToString();
                        projectModel.ProjectName = item["Name"].ToString();
                        projectModel.IsActive = Convert.ToBoolean(item["IsActive"]);
                        projectModel.ProjectTypeName = item["ProjectTypeName"].ToString();
                        projectModel.ProjectTypeID = Convert.ToInt32(item["ProjectTypeID"]);
                        // projectModel.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["SeverityName"].ToString();
                        lstProjectDetl.Add(projectModel);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstProjectDetl;
        }
        //**

        public DataTable GetProjectByID(int id)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",id)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjects_Get]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable GetAllUserListByProjectID(int projectID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@projectID",projectID)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_GetProjectMemberList]", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetAllTaskListByProjectID(int projectID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@projectID",projectID)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_GetProjectTaskList]", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public Boolean InsertProjectdata(ProjectModel model, out int id)
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
                    new SqlParameter("@Name",model.ProjectName),
                    new SqlParameter("@projectCode",model.ProjectCode),
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@ParentProjectId", model.ParentProjectID),
                    new SqlParameter("@DeptID", model.DepartmentID),
                    new SqlParameter("@ManagerID", (model.PMUserID ==0)?(int?)null:model.PMUserID),
                    new SqlParameter("@ClientID", model.ClientD),
                    new SqlParameter("@CreatedBy",model.CreatedBy),
                    new SqlParameter("@CreatedDate",model.CreateDate),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@MaxProjectTimeInHours",model.MaxProjectTimeInHours),
                    new SqlParameter("@ProjectTypeID",model.ProjectTypeID),
                    new SqlParameter("@PricingModelID",model.ProjectPricingID),
                    new SqlParameter("@BillingTypeID",model.ProjectBillingID),
                    new SqlParameter("@ClientPoNo",model.ClientPoNo),
                    new SqlParameter("@ClientPoDate",model.ClientPoDate),
                    new SqlParameter("@StartDate",model.ProjectStartDate),
                    new SqlParameter("@EndDate",model.ProjectEndDate),
                    new SqlParameter("@CurrencyTypeID",model.CurrencyID),
                    new SqlParameter("@ProjectRate",model.ProjectRate),
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjecs_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    //model.ProjectID = id;
                    //model.ISErr = false;
                    //model.ErrString = "Data Saved Successfully.";
                    result = true;
                }
                else
                {
                    id = 0;
                    result = false;
                    //model.ISErr = true;
                    //model.ErrString = "Error Occured.";
                }
            }
            catch (Exception ex)
            {
                //model.ISErr = true;
                //model.ErrString = "Error Occured.";
                result = false;
            }
            return result;

        }

        public Boolean Update_ProjectDetails(ProjectModel model)
        {
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@Id",model.ProjectID),
                    new SqlParameter("@Name",model.ProjectName),
                    new SqlParameter("@projectCode",model.ProjectCode??""),
                    new SqlParameter("@Description",model.Description ??""),
                    new SqlParameter("@ParentProjectId", model.ParentProjectID),
                    new SqlParameter("@DeptID", model.DepartmentID),
                    new SqlParameter("@ManagerID", model.PMUserID),
                    new SqlParameter("@ClientID", model.ClientD),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@MaxProjectTimeInHours",model.MaxProjectTimeInHours),
                    new SqlParameter("@ProjectTypeID",model.ProjectTypeID),
                     new SqlParameter("@PricingModelID",model.ProjectPricingID),
                    new SqlParameter("@BillingTypeID",model.ProjectBillingID),
                    new SqlParameter("@ClientPoNo",model.ClientPoNo),
                    new SqlParameter("@ClientPoDate",model.ClientPoDate),
                    new SqlParameter("@StartDate",model.ProjectStartDate),
                    new SqlParameter("@EndDate",model.ProjectEndDate),
                    new SqlParameter("@CurrencyTypeID",model.CurrencyID),
                    new SqlParameter("@ProjectRate",model.ProjectRate)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjects_Update]", param);
                if (dt != null)
                {

                    result = true;
                }
                else
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

        public List<DepartmentModel> GetDepartments(int orgId = 0)
        {
            DataTable dt = null;
            List<DepartmentModel> departments = new List<DepartmentModel>();
            try
            {
                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDepartment_Get]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDepartment_Get]");


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        DepartmentModel department = new DepartmentModel();
                        department.DepartmentID = Convert.ToInt32(item["ID"]);
                        department.OrganisationName = item["OrganisationName"].ToString();
                        department.Name = item["NAME"].ToString();
                        department.IsActive = Convert.ToBoolean(item["IsActive"]);
                        //department.DisplayTextForCumboWithOrgName = item["NAME"].ToString() + "-" + item["OrganisationName"].ToString();
                        department.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["NAME"].ToString();

                        departments.Add(department);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return departments;
        }

        public List<UserModel> GetManagers(int orgID = 0)
        {
            DataTable dt = null;
            List<UserModel> userModelsList = new List<UserModel>();
            try
            {
                if (orgID != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgID)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]");


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        UserModel user = new UserModel();
                        user.ID = Convert.ToInt32(item["Id"]);
                        user.OrganisationName = item["orgname"].ToString();
                        user.Name = item["Name"].ToString();
                        user.IsActive = Convert.ToBoolean(item["IsActive"]);
                        user.OrgName_UserNameForCombo = item["orgname"].ToString() + "-" + item["Name"].ToString();
                        userModelsList.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return userModelsList;
        }

        public List<ClientModel> GetClients(int orgID = 0)
        {
            DataTable dt = null;
            List<ClientModel> clientsList = new List<ClientModel>();
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


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ClientModel client = new ClientModel();
                        client.ClientID = Convert.ToInt32(item["ClientID"]);
                        client.OrganisationName = item["orgname"].ToString();
                        client.ClientName = item["ClientName"].ToString();
                        client.IsActive = Convert.ToBoolean(item["IsActive"]);
                        client.OrgName_ClientNameForCombo = item["orgname"].ToString() + "-" + item["ClientName"].ToString();
                        clientsList.Add(client);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return clientsList;
        }

        public DataTable GetAllUserByProjectID(int projectId)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                         new SqlParameter("@ProjectID",projectId),

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUserProjects_GetByProjectID]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable DeleteAllExistingMappingByProjectID(int ProjectId)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                         new SqlParameter("@ProjectID",ProjectId),

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUserProjects_DeleteMappingByProjectId]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
    }

    public class ProjectTypeModel
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Project Type Code")]
        public string CODE { get; set; }
        [Display(Name = "Project Type Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrganisationID { get; set; }
        [Display(Name = "Organisation Name")]
        public string OrganisationName { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTS { get; set; }
        public DateTime EditedTS { get; set; }
        public int EditedBy { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }
        public string DisplayTextForCumboWithOrgName { get; set; }

        SqlHelper objSQLHelper = new SqlHelper();

        public List<ProjectTypeModel> GetProjectType(int orgId = 0)
        {
            DataTable dt = null;
            List<ProjectTypeModel> lstProjType = new List<ProjectTypeModel>();
            try
            {
                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjectType_Get]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjectType_Get]");


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ProjectTypeModel projectType = new ProjectTypeModel();
                        projectType.ID = Convert.ToInt32(item["ID"]);
                        projectType.OrganisationName = item["OrganisationName"].ToString();
                        projectType.Name = item["NAME"].ToString();
                        projectType.IsActive = Convert.ToBoolean(item["IsActive"]);
                        //department.DisplayTextForCumboWithOrgName = item["NAME"].ToString() + "-" + item["OrganisationName"].ToString();
                        projectType.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["NAME"].ToString();
                        lstProjType.Add(projectType);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstProjType;
        }
    }

    public class ProjectPricingModel
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Project Pricing Code")]
        public string CODE { get; set; }
        [Display(Name = "Project Pricing Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrganisationID { get; set; }
        [Display(Name = "Organisation Name")]
        public string OrganisationName { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTS { get; set; }
        public DateTime EditedTS { get; set; }
        public int EditedBy { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }
        public string DisplayTextForCumboWithOrgName { get; set; }

        SqlHelper objSQLHelper = new SqlHelper();

        //***
        public List<ProjectPricingModel> Get_ProjectPricingDetails(int orgId = 0)
        {
            DataTable dt = null;
            List<ProjectPricingModel> lstProjectPrice = new List<ProjectPricingModel>();
            try
            {
                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_ProjectPricingModel_GET]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_ProjectPricingModel_GET]");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ProjectPricingModel ProjectPrice = new ProjectPricingModel();
                        ProjectPrice.ID = Convert.ToInt32(item["ID"]);
                        ProjectPrice.OrganisationName = item["OrganisationName"].ToString();
                        ProjectPrice.Name = item["NAME"].ToString();
                        ProjectPrice.IsActive = Convert.ToBoolean(item["IsActive"]);
                        //department.DisplayTextForCumboWithOrgName = item["NAME"].ToString() + "-" + item["OrganisationName"].ToString();
                        ProjectPrice.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["NAME"].ToString();
                        lstProjectPrice.Add(ProjectPrice);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstProjectPrice;
        }
        //**
    }

    public class ProjectBillingModel
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Project Billing Code")]
        public string CODE { get; set; }
        [Display(Name = "Project Billing Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrganisationID { get; set; }
        [Display(Name = "Organisation Name")]
        public string OrganisationName { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTS { get; set; }
        public DateTime EditedTS { get; set; }
        public int EditedBy { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }
        public string DisplayTextForCumboWithOrgName { get; set; }

        SqlHelper objSQLHelper = new SqlHelper();

        //***
        public List<ProjectBillingModel> Get_ProjectBillingDetails(int orgId = 0)
        {
            DataTable dt = null;
            List<ProjectBillingModel> lstPrjBillingType = new List<ProjectBillingModel>();
            try
            {
                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_ProjectBillingType_GET]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_ProjectBillingType_GET]");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ProjectBillingModel ProjectBill = new ProjectBillingModel();
                        ProjectBill.ID = Convert.ToInt32(item["ID"]);
                        ProjectBill.OrganisationName = item["OrganisationName"].ToString();
                        ProjectBill.Name = item["NAME"].ToString();
                        ProjectBill.IsActive = Convert.ToBoolean(item["IsActive"]);
                        //department.DisplayTextForCumboWithOrgName = item["NAME"].ToString() + "-" + item["OrganisationName"].ToString();
                        ProjectBill.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["NAME"].ToString();
                        lstPrjBillingType.Add(ProjectBill);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstPrjBillingType;
        }
        //**
    }

    public class MasterCurrencyModel
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Currency Code")]
        public string CODE { get; set; }
        [Display(Name = "Currency Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrganisationID { get; set; }
        [Display(Name = "Organisation Name")]
        public string OrganisationName { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTS { get; set; }
        public DateTime EditedTS { get; set; }
        public int EditedBy { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }
        public string DisplayTextForCumboWithOrgName { get; set; }

        SqlHelper objSQLHelper = new SqlHelper();

        //***
        public List<MasterCurrencyModel> Get_CurrencyDetails(int orgId = 0)
        {
            DataTable dt = null;
            List<MasterCurrencyModel> lstCurrencyDetl = new List<MasterCurrencyModel>();
            try
            {
                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterCurrency_GET]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterCurrency_GET]");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        MasterCurrencyModel masterCurrency = new MasterCurrencyModel();
                        masterCurrency.ID = Convert.ToInt32(item["ID"]);
                        masterCurrency.OrganisationName = item["OrganisationName"].ToString();
                        masterCurrency.Name = item["NAME"].ToString();
                        masterCurrency.IsActive = Convert.ToBoolean(item["IsActive"]);
                        //department.DisplayTextForCumboWithOrgName = item["NAME"].ToString() + "-" + item["OrganisationName"].ToString();
                        masterCurrency.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["NAME"].ToString();
                        lstCurrencyDetl.Add(masterCurrency);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstCurrencyDetl;
        }
        //**
    }

    public class MasterSeverityModel
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Severity Code")]
        public string CODE { get; set; }
        [Display(Name = "Severity Name")]
        public string Name { get; set; }
        public int Rank { get; set; }
      
        public int OrganisationID { get; set; }
        [Display(Name = "Organisation Name")]
        public string OrganisationName { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTS { get; set; }
        public DateTime EditedTS { get; set; }
        public int EditedBy { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }
        public string DisplayTextForCumboWithOrgName { get; set; }

        SqlHelper objSQLHelper = new SqlHelper();

        //***
        public List<MasterSeverityModel> Get_SeverityDetails(int orgId = 0)
        {
            DataTable dt = null;
            List<MasterSeverityModel> lstSeverityDetl = new List<MasterSeverityModel>();
            try
            {
                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterSeverity_GET]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterSeverity_GET]");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        MasterSeverityModel masterSeverity = new MasterSeverityModel();
                        masterSeverity.ID = Convert.ToInt32(item["SeverityID"]);
                        masterSeverity.OrganisationName = item["OrganisationName"].ToString();
                        masterSeverity.Name = item["SeverityName"].ToString();
                        masterSeverity.IsActive = Convert.ToBoolean(item["IsActive"]);
                        //department.DisplayTextForCumboWithOrgName = item["NAME"].ToString() + "-" + item["OrganisationName"].ToString();
                        masterSeverity.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["SeverityName"].ToString();
                        lstSeverityDetl.Add(masterSeverity);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstSeverityDetl;
        }
        //**
    }
 
}