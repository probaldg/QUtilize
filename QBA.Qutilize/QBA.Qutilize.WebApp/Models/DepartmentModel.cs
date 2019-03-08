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
    public class DepartmentModel
    {
        [Key]
        public int DepartmentID { get; set; }

        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int DepartmentHeadId { get; set; }
        [Display(Name = "Department Head Name")]
        public string DepartmentHeadName { get; set; }

        public int OrganisationID { get; set; }

        [Display(Name = "Organisation Name")]

        public string OrganisationName { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTS { get; set; }
        public DateTime EditedTS { get; set; }
        public int EditedBy { get; set; }
      

       

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public DataTable GetAllDepartments()
        {
            DataTable dt = null;
            try
            {
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDepartment_Get]");
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
       
        public DataTable GetDepartmentByID(int id)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",id)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDepartment_Get]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public Boolean InsertDepartmentdata(DepartmentModel model, out int id)
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
                    new SqlParameter("@Code",model.DepartmentCode),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@Description", model.Description),
                    new SqlParameter("@ORGID", model.OrganisationID),
                   new SqlParameter("@IsActive",model.IsActive),

                    new SqlParameter("@AddedBy",model.CreatedBy),
                    new SqlParameter("@AddedTS",model.CreatedTS)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDepartment_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.DepartmentID = id;
                    //model.ISErr = false;

                    //model.ErrString = "Data Saved Successfully!!!";
                    result = true;
                }
                else
                {
                    id = 0;
                    result = false;
                    //model.ISErr = true;
                    //model.ErrString = "Error Occured!!!";
                }
            }
            catch (Exception ex)
            {
                //model.ISErr = true;
                //model.ErrString = "Error Occured!!!";
                result = false;
            }
            return result;

        }

        public Boolean UpdateDepartmentDetails(DepartmentModel model)
        {
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@DepartmentID",model.DepartmentID),
                    new SqlParameter("@Code",model.DepartmentCode),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@Description", model.Description),
                    new SqlParameter("@DeptHeadID",model.DepartmentHeadId),
                    new SqlParameter("@ORGID",model.OrganisationID),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@EDITEDBY",model.EditedBy),
                    new SqlParameter("@EDITEDTS",model.EditedTS)

                };
                dt = objSQLHelper.ExecuteDataTable("[USPDepartment_Update]", param);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public Boolean DeleteDepartmentDetails(int departmentId, int UserId)
        {
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@DepartmentId",departmentId),
                    new SqlParameter("@EDITEDBY",UserId),
                    new SqlParameter("@EDITEDTS",DateTime.Now)
                };
                dt = objSQLHelper.ExecuteDataTable("[USPDepartment_Delete]", param);
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