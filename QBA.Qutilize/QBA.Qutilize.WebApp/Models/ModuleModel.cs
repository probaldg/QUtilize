using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.Models
{
    public class ModuleModel
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Parent Module")]
        public int ParentID { get; set; }
        [Display(Name = "Module Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string DisplayCSS { get; set; }
        public string DisplayIcon { get; set; }
        public string URL { get; set; }
        public int Rank { get; set; }
        public Boolean isActive { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedTS { get; set; }
        public int EditedBy { get; set; }
        public DateTime EditedTS { get; set; }


        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion


        public DataTable GetAllModules()
        {
            DataTable dt = null;
            try
            {
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_Module_GetForAdmin]");
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public DataTable GetModuleByID(int id)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",id)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_Module_GetModuleByID]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }




    }
}