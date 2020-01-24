using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;


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

        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm new Password")]
        public string ConfirmNewPassword { get; set; }

        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        [Display(Name = "User code")]
        public string UserCode { get; set; }

        public string Designation { get; set; }

        public int UserOrgId { get; set; }

        public int UserOrgName { get; set; }

        public int ManagerId { get; set; }
        public int ManagerId_II { get; set; }
        public int ManagerId_III { get; set; }
        public List<int> DepartmentIds { get; set; }
        public string DepartmentIdsInString { get; set; }
        public string ManagerName { get; set; }

        [Display(Name = "Manager")]
        public List<UserModel> UsersList { get; set; }
        [Display(Name = "Manager II")]
        public List<UserModel> UsersList_II { get; set; }
        [Display(Name = "Manager III")]
        public List<UserModel> UsersList_III { get; set; }


        [Display(Name = "Organisation")]
        public List<OrganisationModel> OrganisationList { get; set; }

        [Display(Name = "Departments")]
        public List<DepartmentModel> DepartmentList { get; set; }
        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }

        [Display(Name = "Alternet Contact No")]
        public string AlterNetContactNo { get; set; }

        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Birth Date")]
        public string birthDayToDisplay { get; set; }

        public string Gender { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        public string ManagerEmpCode { get; set; }
        public string EmployeeCode { get; set; }
        public string RoleName { get; set; }
        public string DepartmentName { get; set; }
        public string OrganisationName { get; set; }
        public string ProjectName { get; set; }

        public string OrgName_UserNameForCombo { get; set; }

        [Display(Name = "Functional Role")]
        public List<MasterFunctionalRoleModel> FunctionalRoleList { get; set; }
        public int FunctionalRoleId { get; set; }
        [Display(Name = "Marital Status")]
        public List<MasterMaritalStatusModel> MaritalStatusList { get; set; }
        public int MaritalStatusID { get; set; }
        [Display(Name = "Date Of Joining")]
        public string JoiningDateDisplay { get; set; }
        public DateTime ? DOJ { get; set; }

        [Display(Name = "Exit Date")]
        public string ExitDateDisplay { get; set; }
        public DateTime ? ExitDate { get; set; }

        [Display(Name = "Anniversary Date")]
        public string AnniversaryDateDisplay { get; set; }
        public DateTime ? AnniversaryDate { get; set; }
        [Display(Name = "User Monthly Cost")]
        public double UserCost { get; set; }


        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion


        public UserModel()
        {
            UsersList = new List<UserModel>();
            UsersList_II = new List<UserModel>();
            UsersList_III = new List<UserModel>();
            OrganisationList = new List<OrganisationModel>();
            DepartmentList = new List<DepartmentModel>();
            DepartmentIds = new List<int>();
        }
        public DataTable GetAllUsers(int orgId = 0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@OrgID",orgId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public List<OrganisationModel> GetAllOrgInList(int orgId = 0)
        {
            DataTable dt = null;
            List<OrganisationModel> Organisations = new List<OrganisationModel>();
            try
            {
                //SqlParameter[] param ={
                //                        new SqlParameter("@OrgID",orgId)
                //                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[SP_GetAllOrganisation]");

                foreach (DataRow item in dt.Rows)
                {
                    OrganisationModel organisation = new OrganisationModel();
                    organisation.id = Convert.ToInt32(item["id"]);
                    organisation.orgname = item["orgname"].ToString();
                    organisation.isActive = Convert.ToBoolean(item["IsActive"]);
                    Organisations.Add(organisation);
                }
            }
            catch (Exception ex)
            {

            }
            return Organisations;
        }

        public List<UserModel> GetAllUsersInList(int orgId = 0)
        {
            DataTable dt = null;
            List<UserModel> users = new List<UserModel>();
            try
            {
                if (orgId == 0)
                {
                    dt = null;
                }
                else
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgID",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]", param);
                }

                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        UserModel user = new UserModel();
                        user.ID = Convert.ToInt32(item["Id"]);
                        user.Name = item["Name"].ToString();
                        user.IsActive = Convert.ToBoolean(item["IsActive"]);
                        users.Add(user);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return users;
        }

        public List<DepartmentModel> GetAllDepartmentInList(int orgId = 0)
        {
            DataTable dt = null;
            List<DepartmentModel> departments = new List<DepartmentModel>();
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDepartment_Get]", param);

                foreach (DataRow item in dt.Rows)
                {
                    DepartmentModel department = new DepartmentModel();
                    department.DepartmentID = Convert.ToInt32(item["Id"]);
                    department.Name = item["Name"].ToString();
                    department.IsActive = Convert.ToBoolean(item["IsActive"]);
                    departments.Add(department);
                }
            }
            catch (Exception ex)
            {

            }
            return departments;
        }

        public DataTable GetUsersByID(int id)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",id)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]", param);
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
                    new SqlParameter("@UserCode",model.UserCode),
                    new SqlParameter("@EmailId", model.EmailId),
                    new SqlParameter("@Password",model.Password),
                    new SqlParameter("@Designation",model.Designation??""),
                    new SqlParameter("@ManagerId",model.ManagerId),
                    new SqlParameter("@ManagerId_II",model.ManagerId_II),
                    new SqlParameter("@ManagerId_III",model.ManagerId_III),
                    new SqlParameter("@DepartmentIds",model.DepartmentIdsInString),
                    new SqlParameter("@PhoneNo",model.ContactNo??""),
                    new SqlParameter("@AlternetPhoneNo",model.AlterNetContactNo??""),
                    new SqlParameter("@birthDate",model.BirthDate),
                    new SqlParameter("@Gender",model.Gender??""),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@CreatedBy",model.CreatedBy),
                    new SqlParameter("@CreatedDate",model.CreateDate),
                    new SqlParameter("@MaritalStatus",model.MaritalStatusID),
                    new SqlParameter("@AnniversaryDate", model.AnniversaryDate!= null?model.AnniversaryDate:null),
                    new SqlParameter("@UserCostMonthly",model.UserCost),
                    new SqlParameter("@ExitDate",  model.ExitDate!= null?model.ExitDate:null),
                    new SqlParameter("@JoiningDate", model.DOJ!= null?model.DOJ:null),
                    new SqlParameter("@FunctionalRole",model.FunctionalRoleId),
                              

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    if (id > 0)
                    {
                        model.ID = id;
                        model.ISErr = false;
                        model.ErrString = "Data Saved Successfully.";
                        result = true;
                    }
                    else
                    {
                        id = 0;
                        result = false;
                        model.ISErr = true;
                        model.ErrString = "Error Occured.";
                    }

                }
                else
                {
                    id = 0;
                    result = false;
                    model.ISErr = true;
                    model.ErrString = "Error Occured.";
                }
            }
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured.";
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
                    new SqlParameter("@userCode",model.UserCode),
                    new SqlParameter("@userName",model.UserName),
                    new SqlParameter("@EmailId", model.EmailId),
                    new SqlParameter("@Designation", model.Designation??""),
                    new SqlParameter("@ManagerID", model.ManagerId),
                    new SqlParameter("@ManagerId_II",model.ManagerId_II),
                    new SqlParameter("@ManagerId_III",model.ManagerId_III),
                    new SqlParameter("@DepartmentIds",model.DepartmentIdsInString),
                    new SqlParameter("@PhoneNo",model.ContactNo??""),
                    new SqlParameter("@AlternetPhoneNo",model.AlterNetContactNo??""),
                    new SqlParameter("@birthDate",model.BirthDate),
                    new SqlParameter("@Gender",model.Gender??null),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),
                    new SqlParameter("@isActive",model.IsActive),
                    new SqlParameter("@MaritalStatus",model.MaritalStatusID),
                    new SqlParameter("@AnniversaryDate", model.AnniversaryDate!= null?model.AnniversaryDate:null),
                    new SqlParameter("@UserCostMonthly",model.UserCost),
                    new SqlParameter("@ExitDate",  model.ExitDate!= null?model.ExitDate:null),
                    new SqlParameter("@JoiningDate", model.DOJ!= null?model.DOJ:null),
                    new SqlParameter("@FunctionalRole",model.FunctionalRoleId),

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_Update]", param);
                model.ISErr = false;
                model.ErrString = "Data Saved Successfully.";

            }
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured.";
                result = false;
            }
            return result;
        }


        public DataTable checkemail(string email, int orgId)
        {
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Email",email),
                                        new SqlParameter("@orgId",orgId)
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
            // bool result = false;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",ID),
                                        new SqlParameter("@password",newPwd),
                                        new SqlParameter("@EditedBy",editedBy),
                                        new SqlParameter("@EditedDate",editedTS)

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_UpdatePassword]", param);
                // result = true;

            }
            catch (Exception ex)
            {
                dt = null;
                // result = false;
            }
            return dt;
        }


        public DataTable GetMyAccountData(int ID)
        {
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@ID",ID)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_GetAccountDetails]", param);
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

       

    }

    public class MasterFunctionalRoleModel
    {
        [Key]
        public int ID { get; set; }


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
        public List<MasterFunctionalRoleModel> Get_FunctinalRoleDetl(int orgId = 0)
        {
            DataTable dt = null;
            List<MasterFunctionalRoleModel> lstFunctinalRole = new List<MasterFunctionalRoleModel>();
            try
            {
                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterFunctionalRole_GET]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterFunctionalRole_GET]");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        MasterFunctionalRoleModel masterFunctinalRole = new MasterFunctionalRoleModel();
                        masterFunctinalRole.ID = Convert.ToInt32(item["ID"]);
                        masterFunctinalRole.OrganisationName = item["OrganisationName"].ToString();
                        masterFunctinalRole.Name = item["Name"].ToString();
                        masterFunctinalRole.IsActive = Convert.ToBoolean(item["IsActive"]);
                        //department.DisplayTextForCumboWithOrgName = item["NAME"].ToString() + "-" + item["OrganisationName"].ToString();
                        masterFunctinalRole.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["Name"].ToString();
                        lstFunctinalRole.Add(masterFunctinalRole);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstFunctinalRole;
        }
        //**
    }
    public class MasterMaritalStatusModel
    {
        [Key]
        public int ID { get; set; }


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
        public List<MasterMaritalStatusModel> Get_MaritalStatus(int orgId = 0)
        {
            DataTable dt = null;
            List<MasterMaritalStatusModel> lstMaritalStatus = new List<MasterMaritalStatusModel>();
            try
            {
                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterMaritalStatus_GET]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterMaritalStatus_GET]");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        MasterMaritalStatusModel masterMaritalStatus = new MasterMaritalStatusModel();
                        masterMaritalStatus.ID = Convert.ToInt32(item["ID"]);
                        masterMaritalStatus.OrganisationName = item["OrganisationName"].ToString();
                        masterMaritalStatus.Name = item["Name"].ToString();
                        masterMaritalStatus.IsActive = Convert.ToBoolean(item["IsActive"]);
                        //department.DisplayTextForCumboWithOrgName = item["NAME"].ToString() + "-" + item["OrganisationName"].ToString();
                        masterMaritalStatus.DisplayTextForCumboWithOrgName = item["OrganisationName"].ToString() + "-" + item["Name"].ToString();
                        lstMaritalStatus.Add(masterMaritalStatus);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstMaritalStatus;
        }
        //**
    }
}