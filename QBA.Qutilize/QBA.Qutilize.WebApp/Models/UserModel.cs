using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

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

        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        public string Designation { get; set; }

        public int UserOrgId { get; set; }

        public int ManagerId { get; set; }
        public List<int> DepartmentIds { get; set; }
        public string DepartmentIdsInString { get; set; }
        public string ManagerName { get; set; }

        [Display(Name = "Manager")]
        public List<UserModel> UsersList { get; set; }

        [Display(Name = "Organisation")]
        public List<OrganisationModel> OrganisationList { get; set; }

        [Display(Name = "Departments")]
        public List<DepartmentModel> DepartmentList { get; set; }
        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }

        [Display(Name = "Alternet Contact No")]
        public string AlterNetContactNo { get; set; }

        [Display(Name = "Birth Date")]
        //[DataType(DataType.Date, ErrorMessage = "Date only")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }


        public string Gender { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        //public DataTable GetAllUsers()
        //{
        //    DataTable dt = null;
        //    try
        //    {
        //        dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]");
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return dt;
        //}

        public UserModel()
        {
            UsersList = new List<UserModel>();
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
                    new SqlParameter("@EmailId", model.EmailId),
                    new SqlParameter("@Password",model.Password),
                    new SqlParameter("@Designation",model.Designation??""),
                    new SqlParameter("@ManagerId",model.ManagerId),
                    new SqlParameter("@DepartmentIds",model.DepartmentIdsInString),
                    new SqlParameter("@PhoneNo",model.ContactNo??""),
                    new SqlParameter("@AlternetPhoneNo",model.AlterNetContactNo??""),
                    new SqlParameter("@birthDate",model.BirthDate),
                    new SqlParameter("@Gender",model.Gender??""),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@CreatedBy",model.CreatedBy),
                    new SqlParameter("@CreatedDate",model.CreateDate),

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.ID = id;
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

        public Boolean Update_UserDetails(UserModel model)
        {
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@ID",model.ID),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@userName",model.UserName),
                    new SqlParameter("@EmailId", model.EmailId),
                    new SqlParameter("@Designation", model.Designation??""),
                    new SqlParameter("@ManagerID", model.ManagerId),
                    new SqlParameter("@DepartmentIds",model.DepartmentIdsInString),
                    new SqlParameter("@PhoneNo",model.ContactNo??""),
                    new SqlParameter("@AlternetPhoneNo",model.AlterNetContactNo??""),
                    new SqlParameter("@birthDate",model.BirthDate),
                    new SqlParameter("@Gender",model.Gender??null),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),
                    new SqlParameter("@isActive",model.IsActive)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_Update]", param);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        public DataTable checkemail(string email)
        {
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Email",email)
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

            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",ID),
                                        new SqlParameter("@password",newPwd),
                                        new SqlParameter("@EditedBy",editedBy),
                                        new SqlParameter("@EditedDate",editedTS)

                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_UpdatePassword]", param);
            }
            catch (Exception ex)
            {
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
}