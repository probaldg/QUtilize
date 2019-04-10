using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
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




        public DateTime TaskStartDate { get; set; }

        [Display(Name = "Start Date")]
        public string TaskStartDateDisplay { get; set; }


        public DateTime TaskEndDate { get; set; }

        [Display(Name = "End Date")]
        public string TaskEndDateDisplay { get; set; }
        public int TaskStatusID { get; set; }

        [Display(Name = "Status")]
        public string TaskStatusName { get; set; }

        public List<ProjectTaskModel> TaskList { get; set; }
        public List<ProjectStatusModel> StatusList { get; set; }
        public List<UserModel> UserList { get; set; }
        public List<int> PercentageComplete { get; set; }

        [Display(Name = "Complete percent")]
        public int CompletePercent { get; set; }


        public DateTime? ActualTaskStartDate { get; set; }

        [Display(Name = "Actual Start Date")]
        public string ActualTaskStartDateDisplay { get; set; }


        public DateTime? ActualTaskEndDate { get; set; }

        [Display(Name = "Actual End Date")]
        public string ActualTaskEndDateDisplay { get; set; }
        public bool IsActive { get; set; }
        public bool? IsMilestone { get; set; }

        public string UserIdsTaskAssigned { get; set; }
        // public string UsereIdsInString { get; set; }
        public int AddedBy { get; set; }
        public int EditedBy { get; set; }
        public DateTime AddedTS { get; set; }
        public DateTime EditedTS { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }


        public ProjectTaskModel()
        {
            TaskList = new List<ProjectTaskModel>();
            StatusList = new List<ProjectStatusModel>();
            UserList = new List<UserModel>();
            PercentageComplete = new List<int>();
        }


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

        public DataTable GetProjectTasksByTaskId(int TaskId)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@taskId",TaskId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterProjectTask_Get", param);
            }
            catch (Exception ex)
            {

            }
            return dt;

        }

        public DataTable GetAllUserOfOrganisationByProjectID(int? ProjectId = 0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@ProjectId",ProjectId ==0? null: ProjectId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("USP_GetAllUserByProjectID", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataSet GetTasksData(int? projectId = 0, int? OrgId = 0)
        {
            DataSet dataSet = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@OrgId",OrgId ==0? null: OrgId),
                                        new SqlParameter("@ProjectId",projectId ==0? null: projectId),

                                      };
                dataSet = objSQLHelper.ExecuteDataset("USPGetProjectTaskData", param);
            }
            catch (Exception ex)
            {

            }
            return dataSet;

        }

        public DataTable GetStatusList(int OrgId)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@OrgId",OrgId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterStatus", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public Boolean InsertTaskdata(ProjectTaskModel model, out int id)
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
                    new SqlParameter("@isMilestone",model.IsMilestone),
                    new SqlParameter("@TaskStartDateActual",model.ActualTaskStartDate != null? model.ActualTaskStartDate: null),
                    new SqlParameter("@TaskEndDateActual",model.ActualTaskEndDate!= null?model.ActualTaskEndDate:null),
                    new SqlParameter("@StatusID",model.TaskStatusID),
                    new SqlParameter("@CompletePercent",model.CompletePercent),
                    new SqlParameter("@isACTIVE",model.IsActive),
                    new SqlParameter("@ADDEDBY",model.AddedBy),
                    new SqlParameter("@ADDEDTS",model.AddedTS),
                    new SqlParameter("@UserIds",model.UserIdsTaskAssigned)


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

        public Boolean UpdateTaskdata(ProjectTaskModel model)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param ={
                    new SqlParameter("@TaskID",model.TaskId),
                    new SqlParameter("@TaskCode",model.TaskCode),
                    new SqlParameter("@TaskName",model.TaskName),
                    new SqlParameter("@ParentTaskID", model.ParentTaskId),
                    new SqlParameter("@TaskStartDate",model.TaskStartDate),
                    new SqlParameter("@TaskEndDate",model.TaskEndDate),
                    new SqlParameter("@IsMilestone",model.IsMilestone),
                    new SqlParameter("@StatusID",model.TaskStatusID),
                    new SqlParameter("@CompletePercent",model.CompletePercent),
                    new SqlParameter("@TaskStartDateActual",model.ActualTaskStartDate),
                    new SqlParameter("@TaskEndDateActual",model.ActualTaskEndDate),
                    new SqlParameter("@isACTIVE",model.IsActive),
                    new SqlParameter("@EditedBY",model.EditedBy),
                    new SqlParameter("@EditedTS",model.EditedTS),
                    new SqlParameter("@UserIds",model.UserIdsTaskAssigned)
                };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterProjectTask_Update", param);


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