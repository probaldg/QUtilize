﻿using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectTaskCommentModel
    {
        public int ID { get; set; }
        public int ProjectTaskID { get; set; }
        public int TaskStatusID { get; set; }
        public string Comment { get; set; }        
        public DateTime AddedTS { get; set; }
        public int AddedBy { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }


        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        
        public DataSet GetProjectTasksComments(int ProjectTaskID)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] param ={
                                       new SqlParameter("@ProjectTaskID",ProjectTaskID)
                                      };
                ds = objSQLHelper.ExecuteDataset("[dbo].[USP_getProjectTaskCommentsAndAttachments_Get]", param);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }


        public Boolean InsertCommentdata(ProjectTaskCommentModel model, out int id, out string strMailToName, out string strMailTo)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
             strMailToName = string.Empty;
             strMailTo = string.Empty;
            id = 0;

            try
            {
                SqlParameter Status = new SqlParameter("@Identity", SqlDbType.Int);
                Status.Direction = ParameterDirection.Output;
                SqlParameter[] param ={Status,
                    new SqlParameter("@ProjectTaskID",model.ProjectTaskID),
                    new SqlParameter("@TaskStatusID",model.TaskStatusID),
                    new SqlParameter("@Comment",model.Comment),                    
                    new SqlParameter("@AddedTS",model.AddedTS),
                    new SqlParameter("@AddedBy", model.AddedBy)
                };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterProjectTaskComments_Insert", param);

                if (!(Status.Value is DBNull))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        strMailToName = Convert.ToString(dt.Rows[0]["MailToName"]);
                        strMailTo = Convert.ToString(dt.Rows[0]["MailTo"]);
                    }
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