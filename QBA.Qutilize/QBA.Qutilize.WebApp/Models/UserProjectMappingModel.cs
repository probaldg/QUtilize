using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QBA.Qutilize.WebApp.Models
{
    public class UserProjectMappingModel
    {
        [Key]
        public int ID { get; set; }
        public string ProjectName { get; set; }

        //public int OrgID { get; set; }
        //public string OrgName { get; set; }
        //public SelectList OrganisationList { get; set; }
        public int EditedBy { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedTS { get; set; }
        public DateTime EditedTS { get; set; }

        //public ModuleMapping MP { get; set; }
        public bool isActive { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public DataTable GetAllProjects()
        {
            DataTable dt = null;
            try
            {
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjects_Get]");
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable GetAllUsers()
        {
            DataTable dt = null;
            try
            {
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUsers_GetForWeb]");
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
    }

  
}