using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.Models
{
    public class ManageDepartmentViewModel
    {
        public DepartmentModel Department { get; set; } = new DepartmentModel();

        public int UserID { get; set; }
        public bool IsRoleSysAdmin { get; set; }
        public int[] UserRoleIDs { get; set; }
        public int[] UserDepartmentIDs { get; set; }
        public int UserOrganisationID { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        public List<UserModel> Users { get; set; }
        public List<OrganisationModel> Organisations { get; set; }
        public List<DepartmentModel> Departments { get; set; }
        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public ManageDepartmentViewModel(int userId)
        {
            UserID = userId;
            Users = new List<UserModel>();
            Organisations = new List<OrganisationModel>();
            DataTable dtUserInfo = GetUserInfo(userId);
            SetUserInformation(dtUserInfo);

        }
        public ManageDepartmentViewModel()
        {
            Users = new List<UserModel>();
            Organisations = new List<OrganisationModel>();
        }
        private void SetUserInformation(DataTable dt)
        {

            try
            {
                if (dt.Rows.Count > 0)
                {
                    int rowCounter = 0;
                    UserOrganisationID = Convert.ToInt32(dt.Rows[0]["OrganisationID"]);
                    foreach (DataRow item in dt.Rows)
                    {
                        UserRoleIDs = new int[dt.Rows.Count];
                        UserDepartmentIDs = new int[dt.Rows.Count];

                        UserRoleIDs[rowCounter] = Convert.ToInt32(item["RoleId"]);
                        UserDepartmentIDs[rowCounter] = Convert.ToInt32(item["DepartmentId"]);
                        rowCounter++;
                    }
                    IsRoleSysAdmin = CheckUserIsSysAdmin(UserRoleIDs);
                }

                DataTable dtOrganisation = GetOrganisation();

                if (!IsRoleSysAdmin)
                {
                    if (dtOrganisation != null && dt.Rows.Count > 0)
                    {
                        DataRow[] dataRow = dtOrganisation.Select("id=" + UserOrganisationID);
                        if (dataRow.Length > 0)
                        {
                            Organisations.Add(new OrganisationModel { id = Convert.ToInt32(dataRow[0]["id"]), orgname = dataRow[0]["orgname"].ToString() });
                        }
                    }
                }
                else
                {
                    if (dtOrganisation != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtOrganisation.Rows)
                        {
                            Organisations.Add(new OrganisationModel { id = Convert.ToInt32(item["id"]), orgname = item["orgname"].ToString() });
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetUserInfo(int userId)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param = {

                    new SqlParameter("@UserID",userId)

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_Role_Department_Organisation_Info]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable GetOrganisation()
        {
            DataTable dt = null;
            try
            {
                dt = objSQLHelper.ExecuteDataTable("[SP_GetAllOrganisation]");
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public Boolean CheckUserIsSysAdmin(int[] roleIds)
        {
            bool result = false;
            if (roleIds == null)
            {
                return result;
            }
            try
            {
                if (roleIds.Length > 0)
                {
                    foreach (int role in roleIds)
                    {
                        if (role == 1)
                        {
                            result = true;
                        }
                    }

                }
            }
            catch (Exception)
            {

                result = false;
            }

            return result;
        }
    }
}