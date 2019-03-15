using QBA.Qutilize.WebApp.DAL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Helper
{
    public class UserInfoHelper
    {
        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public int UserId { get; set; }
        public bool IsRoleSysAdmin { get; set; }
        public int[] UserRoleIDs { get; set; }
        //public int[] UserDepartmentIDs { get; set; }
        public int UserOrganisationID { get; set; }

        public UserInfoHelper(int userId)
        {
            UserId = userId;
            DataTable dtUserInfo = GetUserInfo(UserId);
            SetUserInformation(dtUserInfo);
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

        private void SetUserInformation(DataTable dt)
        {

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    int rowCounter = 0;

                    foreach (DataRow item in dt.Rows)
                    {
                        UserRoleIDs = new int[dt.Rows.Count];
                        if (item["RoleId"] != DBNull.Value)
                            UserRoleIDs[rowCounter] = Convert.ToInt32(item["RoleId"]);
                        rowCounter++;
                    }
                    IsRoleSysAdmin = CheckUserIsSysAdmin(UserRoleIDs);

                    if (!IsRoleSysAdmin)
                    {
                        UserOrganisationID = Convert.ToInt32(dt.Rows[0]["OrganisationID"]);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
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