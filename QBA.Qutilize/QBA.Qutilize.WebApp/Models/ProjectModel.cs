﻿using QBA.Qutilize.WebApp.DAL;
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
        public string Description { get; set; }
        public int? ParentProjectID { get; set; }
        public string ParentProjectName { get; set; }
        [Display(Name = "Maximum Time for Project In Hours")]
        public int MaxProjectTimeInHours { get; set; } = 8;
        public int DepartmentID { get; set; }
        public int PMUserID { get; set; }

        [Display(Name = "Department List")]
        public List<DepartmentModel> DepartmentList { get; set; }

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

        public ProjectTaskModel ProjectTask { get; set; }

        public ProjectModel()
        {
            DepartmentList = new List<DepartmentModel>();
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
                    new SqlParameter("@DeptID", model.DepartmentID),
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
                    new SqlParameter("@Description",model.Description ??""),
                    new SqlParameter("@ParentProjectId", model.ParentProjectID),
                    new SqlParameter("@DeptID", model.DepartmentID),
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
                        department.DisplayTextForCumboWithOrgName = item["NAME"].ToString() + "-" + item["OrganisationName"].ToString();
                        departments.Add(department);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return departments;
        }
    }
}