using QBA.Qutilize.WebApp.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

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
        public SelectList ModuleList { get; set; }
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
        public bool ISErr { get; set; }
        public string ErrString { get; set; }

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


        public Boolean InsertModuledata(ModuleModel model, out int id)
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
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@ParentID",model.ParentID),
                    new SqlParameter("@Description", model.Description),
                    new SqlParameter ("@DisplayName",model.DisplayName),
                    new SqlParameter("@DisplayCSS",model.DisplayCSS),
                    new SqlParameter("@DisplayIcon",model.DisplayIcon),
                    new SqlParameter("@URL",model.URL),
                    new SqlParameter("@Rank",model.Rank),
                    new SqlParameter("@isActive",model.isActive),
                    new SqlParameter("@AddedBy",model.AddedBy),
                    new SqlParameter("@AddedTS",model.AddedTS)

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_Module_Insert]", param);

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


        public Boolean Update_ModuleDetails(ModuleModel model)
        {
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@ID",model.ID),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@ParentID",model.ParentID),
                    new SqlParameter("@Description", model.Description),
                    new SqlParameter("@DisplayCSS", model.DisplayCSS),
                    new SqlParameter("@Rank", model.Rank),
                    new SqlParameter("@URL", model.URL),
                    new SqlParameter("@DisplayIcon", model.DisplayIcon),
                    new SqlParameter("@DisplayName", model.DisplayName),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedTS",model.EditedTS),
                    new SqlParameter("@isActive",model.isActive)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_Module_Update]", param);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }




    }
}