﻿using QBA.Qutilize.Models;
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

            //iVisibleDaysEdit = Convert.ToInt32(ConfigurationManager.AppSettings["VisibleDaysEdit"].Trim());
            //iVisibleDaysDisplay = Convert.ToInt32(ConfigurationManager.AppSettings["VisibleDaysDisplay"].Trim());

            WeekStartDate = DateTime.Now.AddDays(-(idays + iVisibleDaysEdit));
            WeekEndDate = WeekStartDate.AddDays(6 + iVisibleDaysEdit);
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
                        DailyTaskModel dailyTaskModel = new DailyTaskModel
                        {
                            DailyTaskId = Convert.ToInt32(item["DailyTaskId"]),
                            ProjectID = Convert.ToInt32(item["ProjectId"]),
                            ProjectName = item["ProjectName"].ToString(),
                            TaskDate = Convert.ToDateTime(item["TaskDate"]),
                            TaskName = item["TaskName"].ToString(),
                            StartTime = Convert.ToDateTime(item["StartTime"]),
                            EndTime = Convert.ToDateTime(item["EndTime"]),
                            StartTimeToDisplay = item["StartTime"].ToString(),
                            EndTimeToDisplay = item["EndTime"].ToString(),
                            Hours = CalculateTimeDiffrence(Convert.ToDateTime(item["StartTime"]), Convert.ToDateTime(item["EndTime"])),
                            Description = item["Description"].ToString()

                        };
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