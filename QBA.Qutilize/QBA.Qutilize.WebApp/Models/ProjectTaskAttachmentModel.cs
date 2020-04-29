using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectTaskAttachmentModel
    {
        public int ID { get; set; }
        public int ProjectTaskID { get; set; }
        public string AttachmentName { get; set; }
        public DateTime AddedTS { get; set; }
        public int AddedBy { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public DataTable GetProjectTasksAttachments(int ProjectTaskID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@ProjectTaskID",ProjectTaskID)
                                      };
                dt = objSQLHelper.ExecuteDataTable("USP_tblMasterProjectTaskAttachments_Get", param);
            }
            catch (Exception ex)
            {

            }
            return dt;

        }



        public Boolean InsertAttachmentdata(ProjectTaskAttachmentModel model, out int id)
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
                    new SqlParameter("@ProjectTaskID",model.ProjectTaskID),
                    new SqlParameter("@AttachmentName",model.AttachmentName),
                    new SqlParameter("@AddedTS",model.AddedTS),
                    new SqlParameter("@AddedBy", model.AddedBy)                    
                };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterProjectTaskAttachments_Insert", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    if (id > 0)
                    {
                        model.ID = id;
                        model.ISErr = false;
                        model.ErrString = "Data Saved Successfully.";
                        result = true;
                    }
                    else
                    {
                        id = 0;
                        result = false;
                        model.ISErr = true;
                        model.ErrString = "Error Occured.";
                    }

                }
                else
                {
                    id = 0;
                    result = false;
                    model.ISErr = true;
                    model.ErrString = "Error Occured.";
                }
            }
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured.";
                result = false;
            }
            return result;

        }

    }
}