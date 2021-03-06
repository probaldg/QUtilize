﻿using QBA.Qutilize.Models;
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
            ProjectTaskList = new List<ProjectTaskModel>();
            IssueList = new List<ProjectIssueModel>();
        }
        public string Ticketno { get; set; }
       public int IssueId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string StartDate { get; set; }
        public DateTime TaskDate { get; set; }
        public string CurrentTaskdate { get; set; }
        public string EndDate { get; set; }
        public int UserId { get; set; }
        public int ProjectID { get; set; }
        public string TaskName { get; set; }
        public int ProjectTaskID { get; set; } //create by malabika 14-11-2019
        public DailyTaskModel DailyTaskModel { get; set; }
        public SelectList ProjecSelectList { get; set; }
        public List<Project> ProjectsList { get; set; }
        public List<ProjectIssueModel> IssueList { get; set; }
        public List<ProjectTaskModel> ProjectTaskList { get; set; }
        public List<DailyTaskModel> DailyTaskList { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }
        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        private void SetWeekStartDate()
        {

            DayOfWeek day = DateTime.Now.DayOfWeek;
            int idays = day - DayOfWeek.Monday;
            int iVisibleDaysEdit = 0;
            int iVisibleDaysDisplay = 0;

            //WeekStartDate = DateTime.Now.AddDays(-(idays + iVisibleDaysEdit));
            //WeekEndDate = WeekStartDate.AddDays(6 + iVisibleDaysEdit);

            //DateTime startDate = DateTime.Now.AddDays(-(idays + iVisibleDaysEdit));
            //StartDate = $"{startDate.Day}/{startDate.Month}/{startDate.Year}";
            //EndDate = $"{ startDate.AddDays(6 + iVisibleDaysEdit).Day}/{ startDate.AddDays(6 + iVisibleDaysEdit).Month}/{ startDate.AddDays(6 + iVisibleDaysEdit).Year}";

            WeekStartDate = DateTime.Now;
            //create by Malabika 13-11-2019*********
            int weekday = Convert.ToInt32(DateTime.Now.DayOfWeek);
            DateTime Previous_startDate = DateTime.Now;
            if (weekday == 1)
            {
                Previous_startDate = WeekStartDate.AddDays(-3);
                WeekStartDate= WeekStartDate.AddDays(-3);
            }
            else if (weekday > 1 && weekday <= 5)
            {
                Previous_startDate = WeekStartDate.AddDays(-1);
                WeekStartDate = WeekStartDate.AddDays(-1);
            }
           //*****End*****
          
            WeekEndDate = DateTime.Now;
           

            DateTime startDate = DateTime.Now;
           // StartDate = $"{startDate.Day}/{startDate.Month}/{startDate.Year}";  //changed by malabika
            EndDate = $"{startDate.Day}/{startDate.Month}/{startDate.Year}";
            StartDate = $"{Previous_startDate.Day}/{Previous_startDate.Month}/{Previous_startDate.Year}";  //create by malabika
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

        //public List<DailyTaskModel> GetAllTaskByDateRange(int userID, DateTime startDate, DateTime endTime)
        //{
        //    DataTable dt = null;
        //    List<DailyTaskModel> taskList = new List<DailyTaskModel>();
        //    try
        //    {
        //        SqlParameter[] param ={
        //            new SqlParameter("@UserID",userID),
        //            new SqlParameter("@STARTDT",startDate),
        //            new SqlParameter("@ENDDT", endTime)
        //        };
        //        dt = objSQLHelper.ExecuteDataTable("[USPDailyTask_GetByDateRange]", param);

        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow item in dt.Rows)
        //            {
        //                DailyTaskModel dailyTaskModel = new DailyTaskModel();

        //                dailyTaskModel.DailyTaskId = Convert.ToInt32(item["DailyTaskId"]);
        //                dailyTaskModel.ProjectID = Convert.ToInt32(item["ProjectId"]);
        //                dailyTaskModel.ProjectName = item["ProjectName"].ToString();
        //                dailyTaskModel.TaskDate = Convert.ToDateTime(item["TaskDate"]);
        //                dailyTaskModel.TaskName = item["TaskName"] == null ? "" : item["TaskName"].ToString();


        //                //Start time only contains date part so creating full date time with taskDate

        //                var tempStartTime = Convert.ToDateTime(item["StartTime"]).TimeOfDay;
        //                dailyTaskModel.StartTime = new DateTime(dailyTaskModel.TaskDate.Year, dailyTaskModel.TaskDate.Month, dailyTaskModel.TaskDate.Day, tempStartTime.Hours, tempStartTime.Minutes, tempStartTime.Seconds);
        //                dailyTaskModel.StartTimeToDisplay = item["StartTime"].ToString();

        //                if (!DBNull.Value.Equals(item["EndTime"]))
        //                {
        //                    var tempEndTime = Convert.ToDateTime(item["EndTime"]).TimeOfDay;

        //                    dailyTaskModel.EndTime = new DateTime(dailyTaskModel.TaskDate.Year, dailyTaskModel.TaskDate.Month, dailyTaskModel.TaskDate.Day, tempEndTime.Hours, tempEndTime.Minutes, tempEndTime.Seconds);
        //                    dailyTaskModel.EndTimeToDisplay = item["EndTime"].ToString();
        //                    dailyTaskModel.HoursToDisplay = CalculateTimeDiffrence(Convert.ToDateTime(item["StartTime"]), Convert.ToDateTime(item["EndTime"]));
        //                }

        //                dailyTaskModel.Description = item["Description"] == null ? "" : item["Description"].ToString();



        //                taskList.Add(dailyTaskModel);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return taskList;
        //}

        public List<DailyTaskModel> GetAllTaskByDateRange(int userID, DateTime startDate, DateTime endTime)
        {
            DataTable dt = null;
            List<DailyTaskModel> taskList = new List<DailyTaskModel>();
            try
            {
                SqlParameter[] param ={
                    new SqlParameter("@UserID",userID),
                    new SqlParameter("@STARTDT", startDate),
                    new SqlParameter("@ENDDT", endTime)
                };
                dt = objSQLHelper.ExecuteDataTable("[USPDailyTask_GetByDateRange]", param);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        DailyTaskModel dailyTaskModel = new DailyTaskModel();
                        DailyTaskViewModel dailyTaskViewModel = new DailyTaskViewModel();
                        dailyTaskModel.DailyTaskId = Convert.ToInt32(item["DailyTaskId"]);
                        dailyTaskModel.ProjectID = Convert.ToInt32(item["ProjectId"]);
                        dailyTaskModel.ProjectName = item["ProjectName"].ToString();
                        dailyTaskModel.TaskDate = Convert.ToDateTime(item["TaskDate"]);

                        dailyTaskViewModel.TaskDate = Convert.ToDateTime(item["TaskDate"]);

                        dailyTaskModel.TaskName = item["TaskName"] == null ? "" : item["TaskName"].ToString();

                        dailyTaskModel.CreateDate= Convert.ToDateTime(item["CreateDate"]);

                        dailyTaskModel.StartTime = Convert.ToDateTime(item["TaskDate"]);
                        dailyTaskModel.StartTimeToDisplay = string.Empty;
                        dailyTaskModel.Ticketno= item["ShortDesc"].ToString();
                        if (Convert.ToString(item["duration"]) == "")
                        {
                            dailyTaskModel.StartTimeToDisplay = string.Empty;
                        }
                        else
                        {
                            string duration = (item["duration"]).ToString();

                            string[] a = duration.Split('.');
                            string hour = "";
                            string minute = "";
                            if (a[0].Length == 1)
                            {
                                hour = "0" + a[0];
                            }
                            else
                            {
                                hour = a[0];
                            }
                            if (a[1].Length == 1)
                            {
                                minute = a[1] + "0";
                            }
                            else
                            {
                                minute = a[1];
                            }
                            dailyTaskModel.StartTimeToDisplay = hour + ":" + minute;
                            dailyTaskModel.Duration = Convert.ToDouble(item["duration"]);
                        }
                      
                        if (!DBNull.Value.Equals(item["ActualEndTime"]))
                        {
                            dailyTaskModel.EndTime = Convert.ToDateTime(item["ActualEndTime"]);
                            dailyTaskModel.EndTimeToDisplay = item["EndTime"].ToString();
                            dailyTaskModel.HoursToDisplay = CalculateTimeDiffrence(dailyTaskModel.StartTime, dailyTaskModel.EndTime);
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
                   // new SqlParameter("@StartDateTime", model.StartTime),
                   // new SqlParameter("@EndDateTime",model.EndTime),
                    new SqlParameter("@TaskName", model.TaskName.Trim()),
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@CreatedBy",model.CreatedBy),
                    new SqlParameter("@CreateDate",model.CreateDate),
                    new SqlParameter("@TaskDate",model.TaskDate),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@ProjectTaskID",model.ProjectTaskID), 
                    new SqlParameter("@Duration", model.Duration),
                    new SqlParameter("@ShortDesc", model.Ticketno)
                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDailyTask_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.DailyTaskId = id;
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
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured.";
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
                    //new SqlParameter("@StartDateTime", model.StartTime),
                   // new SqlParameter("@EndDateTime",model.EndTime),
                    new SqlParameter("@TaskName",model.TaskName),
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),
                    new SqlParameter("@Duration",model.Duration),
                    new SqlParameter("@ShortDesc",model.Ticketno==null?"":model.Ticketno),

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPDailyTask_UpdateForWeb]", param);
                if (dt != null)
                    result = true;
                else
                    result = false;
            }
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured.";
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
                //model.ErrString = "Error Occured.";
                result = false;
            }
            return result;

        }

        private string CalculateTimeDiffrence(DateTime startTime, DateTime endTime)
        {
            try
            {
                TimeSpan ts = endTime - startTime;
                return ts.ToString(@"hh\:mm");

                //return (endTime - startTime).ToString(@"hh\:mm");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      
        public DataTable GetIssueListByProject(int? Projectid =0, int? UserId =0, int? OrgId=0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@ProjectId",Projectid ==0? null: Projectid),
                                        new SqlParameter("@UserId",UserId ==0? null: UserId),
                                        new SqlParameter("@OrgId",OrgId ==0? null: OrgId)
                                       
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_GetIssueListByProject]", param);
              
            }
            catch(Exception ex)
            {

            }
            return dt;
        }
        

    }
}