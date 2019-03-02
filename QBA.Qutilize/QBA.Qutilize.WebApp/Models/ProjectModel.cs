using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectModel
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int? ParentProjectID { get; set; }

        public string ParentProjectName { get; set; }
        public int MaxProjectTimeInHours { get; set; } = 8;
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        public SelectList ProjectList { get; set; }
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
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@ParentProjectId", model.ParentProjectID),
                    new SqlParameter("@CreatedBy",model.CreatedBy),
                    new SqlParameter("@CreatedDate",model.CreateDate),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@MaxProjectTimeInHours",model.MaxProjectTimeInHours),
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjecs_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.ProjectID = id;
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

        public Boolean Update_ProjectDetails(ProjectModel model)
        {
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@Id",model.ProjectID),
                    new SqlParameter("@Name",model.ProjectName),
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@ParentProjectId", model.ParentProjectID),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@MaxProjectTimeInHours",model.MaxProjectTimeInHours)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPProjects_Update]", param);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

    }
}