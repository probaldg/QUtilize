using QBA.Qutilize.WebApp.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectTaskModel
    {
        public int TaskId { get; set; }
        public int ProjectID { get; set; }
        [Display(Name = "Task Code")]
        public string TaskCode { get; set; }
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }

        public int ParentTaskId { get; set; }

        public string ProjectName { get; set; }
        [Display(Name = "Parent Task Name")]
        public string ParentTaskName { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime TaskStartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime TaskEndDate { get; set; }

        public int TaskStatusID { get; set; }

        [Display(Name = "Status")]
        public string TaskStatusName { get; set; }

        [Display(Name = "Complete percent")]
        public int CompletePercent { get; set; }
        public DateTime ActualTaskStartDate { get; set; }
        public DateTime ActualTaskEndDate { get; set; }
        public bool IsActive { get; set; }

        public string UsereIdsInString { get; set; }
        public int AddedBy { get; set; }
        public int EditedBy { get; set; }
        public DateTime AddedTS { get; set; }
        public DateTime EditedTS { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion
        public DataTable GetProjectTasks(int? projectId = 0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@ProjectId",projectId ==0? null: projectId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("USP_tblMasterProjectTask_Get", param);
            }
            catch (Exception ex)
            {

            }
            return dt;

        }

        public Boolean InsertUserdata(ProjectTaskModel model, out int id)
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
                    new SqlParameter("@ProjectID",model.ProjectID),
                    new SqlParameter("@TaskCode",model.TaskCode),
                    new SqlParameter("@TaskName",model.TaskName),
                    new SqlParameter("@ParentTaskID", model.ParentTaskId),
                    new SqlParameter("@TaskStartDate",model.TaskStartDate),
                    new SqlParameter("@TaskEndDate",model.TaskEndDate),
                    new SqlParameter("@StatusID",model.TaskStatusID),
                    new SqlParameter("@CompletePercent",0),
                    new SqlParameter("@TaskStartDateActual",null),
                    new SqlParameter("@TaskEndDateActual",null),
                    new SqlParameter("@isACTIVE",model.IsActive),
                    new SqlParameter("@ADDEDBY",model.AddedBy),
                    new SqlParameter("@ADDEDTS",model.AddedTS),
                    new SqlParameter("@UserIds",model.UsereIdsInString)


                };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterProjectTask_Insert", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    if (id > 0)
                    {
                        model.TaskId = id;
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
    }
}