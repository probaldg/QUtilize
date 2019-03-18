using QBA.Qutilize.Models;
using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Models
{
    public class DailyTaskViewModel
    {
        public DailyTaskViewModel()
        {
            SetWeekStartDate();
            ProjectsList = new List<Project>();
            DailyTaskList = new List<DailyTaskModel>();
            DailyTaskModel = new DailyTaskModel();
        }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int UserId { get; set; }
        public DailyTaskModel DailyTaskModel { get; set; }
        public SelectList ProjecSelectList { get; set; }
        public List<Project> ProjectsList { get; set; }
        public List<DailyTaskModel> DailyTaskList { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        private void SetWeekStartDate()
        {

            DayOfWeek day = DateTime.Now.DayOfWeek;
            int idays = day - DayOfWeek.Monday;
            int iVisibleDaysEdit = 0;
            int iVisibleDaysDisplay = 0;

            WeekStartDate = DateTime.Now.AddDays(-(idays + iVisibleDaysEdit));
            WeekEndDate = WeekStartDate.AddDays(6 + iVisibleDaysEdit);

            DateTime startDate = DateTime.Now.AddDays(-(idays + iVisibleDaysEdit));
            StartDate = $"{startDate.Day}/{startDate.Month}/{startDate.Year}";
            EndDate = $"{ startDate.AddDays(6 + iVisibleDaysEdit).Day}/{ startDate.AddDays(6 + iVisibleDaysEdit).Month}/{ startDate.AddDays(6 + iVisibleDaysEdit).Year}";
        }

        public List<Project> GetAllProjects(int userId)
        {
            DataTable dt = null;
            List<Project> ProjectsList = new List<Project>();
            try
            {

                SqlParameter[] param ={
                    new SqlParameter("@UserId",userId),

                };

                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPUserProjects_Get]", param);

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

        public List<DailyTaskModel> GetAllTaskByDateRange(int userID, DateTime startDate, DateTime endTime)
        {
            DataTable dt = null;
            List<DailyTaskModel> taskList = new List<DailyTaskModel>();
            try
            {
                SqlParameter[] param ={
                    new SqlParameter("@UserID",userID),
                    new SqlParameter("@STARTDT",startDate),
                    new SqlParameter("@ENDDT", endTime)
                };
                dt = objSQLHelper.ExecuteDataTable("[USPDailyTask_GetByDateRange]", param);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        DailyTaskModel dailyTaskModel = new DailyTaskModel();

                        dailyTaskModel.DailyTaskId = Convert.ToInt32(item["DailyTaskId"]);
                        dailyTaskModel.ProjectID = Convert.ToInt32(item["ProjectId"]);
                        dailyTaskModel.ProjectName = item["ProjectName"].ToString();

                        //if (!DBNull.Value.Equals(item["TaskDate"]))
                        //{
                        //    dailyTaskModel.TaskDate = Convert.ToDateTime(item["TaskDate"]);
                        //}
                        //else
                        dailyTaskModel.TaskDate = Convert.ToDateTime(item["StartTime"]);


                        dailyTaskModel.TaskName = item["TaskName"] == null ? "" : item["TaskName"].ToString();


                        //Start time only contains date part so creating full date time with taskDate

                        var tempStartTime = Convert.ToDateTime(item["StartTime"]).TimeOfDay;
                        dailyTaskModel.StartTime = new DateTime(dailyTaskModel.TaskDate.Year, dailyTaskModel.TaskDate.Month, dailyTaskModel.TaskDate.Day, tempStartTime.Hours, tempStartTime.Minutes, tempStartTime.Seconds);
                        dailyTaskModel.StartTimeToDisplay = item["StartTime"].ToString();

                        if (!DBNull.Value.Equals(item["EndTime"]))
                        {
                            var tempEndTime = Convert.ToDateTime(item["EndTime"]).TimeOfDay;

                            dailyTaskModel.EndTime = new DateTime(dailyTaskModel.TaskDate.Year, dailyTaskModel.TaskDate.Month, dailyTaskModel.TaskDate.Day, tempEndTime.Hours, tempEndTime.Minutes, tempEndTime.Seconds);
                            dailyTaskModel.EndTimeToDisplay = item["EndTime"].ToString();
                            dailyTaskModel.Hours = CalculateTimeDiffrence(Convert.ToDateTime(item["StartTime"]), Convert.ToDateTime(item["EndTime"]));
                        }

                        dailyTaskModel.Description = item["Description"] == null ? "" : item["Description"].ToString();



                        taskList.Add(dailyTaskModel);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return taskList;
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
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),

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


        public Boolean DeleteDailyTaskByID(int dailyTaskId, string EditedBy, DateTime EditedOn)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
            try
            {

                SqlParameter[] param ={
                    new SqlParameter("@DailyTaskId",dailyTaskId),
                    new SqlParameter("@EditedBy",EditedBy),
                    new SqlParameter("@EditedOn",EditedOn)
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


        private decimal CalculateTimeDiffrence(DateTime startTime, DateTime endTime)
        {
            try
            {
                TimeSpan ts = endTime.TimeOfDay - startTime.TimeOfDay;
                return Convert.ToDecimal(ts.Hours + "." + ts.Minutes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}