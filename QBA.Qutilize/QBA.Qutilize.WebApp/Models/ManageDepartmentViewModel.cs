using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.Models
{
    public class ManageDepartmentViewModel
    {
        public DepartmentModel Department { get; set; }

        public int UserID { get; set; }
        public bool IsRoleSysAdmin { get; set; }
        public int[] UserRoleIDs { get; set; }
        public int UserOrganisationID { get; set; }

        public int selected { get; set; } = 1;

        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        public List<UserModel> Users { get; set; }
        public List<OrganisationModel> Organisations { get; set; }
        public List<DepartmentModel> Departments { get; set; }
        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public ManageDepartmentViewModel()
        {
            Users = new List<UserModel>();
            Organisations = new List<OrganisationModel>();
            Department = new DepartmentModel();
          
        }

        public DataTable GetUserInfo(int userId)
        {
            DataTable dt = null;
            try
            {
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUser_Role_Department_Organisation_Info]");
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable GetAllOrganisation()
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
        public Boolean IsUserSysAdmin(DataTable dtUserInfo)
        {
            bool result = false;
            if (dtUserInfo == null)
            {
                return result;
            }
            try
            {
                if (dtUserInfo.Rows.Count > 0)
                {
                    foreach (DataRow item in dtUserInfo.Rows)
                    {
                        if (Convert.ToInt32(item["RoleId"]) == 1 || item[""].ToString().ToLower() == "SysAdmin".ToLower())
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