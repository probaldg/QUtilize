using QBA.Qutilize.Models;
using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Models
{
    public class DailyTaskModel
    {
        public DailyTaskModel()
        {
            SetWeekStartDate();
        }
        public int DailyTaskId { get; set; }
        public  int UserID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public DateTime TaskDate { get; set; }
        public DateTime StartTime { get; set; }

        public string StartTimeToDisplay { get; set; }
        public string EndTimeToDisplay { get; set; }


        public DateTime EndTime { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public decimal Hours { get; set; }

        public SelectList ProjecSelectList { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion


        public List<Project> GetAllProjects( int userId)
        {
            DataTable dt = null;
            List<Project> ProjectsList = new List<Project>();
            try
            {

                SqlParameter[] param ={
                    new SqlParameter("@UserId",userId),

                };

                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUserProjects_Get]",param);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Project project = new Project
                        {
                            ProjectID = Convert.ToInt32(item["ProjectId"]),
                            ProjectName = item["Name"].ToString()
                        };
                        ProjectsList.Add(project);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ProjectsList;
        }

        public DataTable GetAllTaskByDateRange(int userID, DateTime startDate, DateTime endTime)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                    new SqlParameter("@UserID",userID),
                    new SqlParameter("@STARTDT",startDate),
                    new SqlParameter("@ENDDT", endTime)
                };
                dt = objSQLHelper.ExecuteDataTable("[USPDailyTask_GetByDateRange]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public Boolean InsertDailyTaskdata(DailyTaskModel model, out int id)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
            id = 0;
            try
            {
                SqlParameter Status = new SqlParameter("@Identity", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                SqlParameter[] param ={Status,
                    new SqlParameter("@UserID",model.UserID),
                    new SqlParameter("@ProjectID",model.ProjectID),
                    new SqlParameter("@StartDateTime", model.StartTime),
                    new SqlParameter("@EndDateTime",model.EndTime),
                    new SqlParameter("@TaskName",model.TaskName),
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@CreatedBy",model.CreatedBy),
                    new SqlParameter("@CreateDate",model.CreateDate),
                    new SqlParameter("@TaskDate",model.TaskDate),
                    new SqlParameter("@IsActive",model.IsActive),

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDailyTask_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.DailyTaskId = id;
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

        // TODO create function for update and delete daily task...
        public Boolean UpdateDailyTaskdata(DailyTaskModel model)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
            try
            {
               
                SqlParameter[] param ={
                    new SqlParameter("@DailyTaskId",model.DailyTaskId),
                    new SqlParameter("@StartDateTime", model.StartTime),
                    new SqlParameter("@EndDateTime",model.EndTime),
                    new SqlParameter("@TaskName",model.TaskName),
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@EditedBy",model.CreatedBy),
                    new SqlParameter("@EditedDate",model.CreateDate),

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDailyTask_UpdateForWeb]", param);

                result = true;
            }
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured!!!";
                result = false;
            }
            return result;

        }


        public Boolean DeleteDailyTaskByID(int dailyTaskId)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
            try
            {

                SqlParameter[] param ={
                    new SqlParameter("@DailyTaskId",dailyTaskId),
                    
                };
                dt = objSQLHelper.ExecuteDataTable("[USPDailyTask_Delete]", param);

                result = true;
            }
            catch (Exception ex)
            {
                //model.ISErr = true;
                //model.ErrString = "Error Occured!!!";
                result = false;
            }
            return result;

        }


        private void SetWeekStartDate()
        {

            DayOfWeek day = DateTime.Now.DayOfWeek;
            int idays = day - DayOfWeek.Monday;
            int iVisibleDaysEdit = 0;
            int iVisibleDaysDisplay = 0;

            //iVisibleDaysEdit = Convert.ToInt32(ConfigurationManager.AppSettings["VisibleDaysEdit"].Trim());
            //iVisibleDaysDisplay = Convert.ToInt32(ConfigurationManager.AppSettings["VisibleDaysDisplay"].Trim());

            WeekStartDate = DateTime.Now.AddDays(-(idays + iVisibleDaysEdit));
            WeekEndDate = WeekStartDate.AddDays(6 + iVisibleDaysEdit);
        }
    }
}