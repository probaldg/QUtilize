﻿using Newtonsoft.Json;
using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DailyTaskModel = QBA.Qutilize.WebApp.Models.DailyTaskModel;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class DailyTaskController : Controller
    {
        readonly int loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
        // GET: DailyTask
        public ActionResult Index()
        {
            DailyTaskViewModel model = new DailyTaskViewModel();
            if (System.Web.HttpContext.Current.Session["sessUser"] != null)
            {
                model.UserId = Convert.ToInt32(Session["sessUser"]);

                if (ModuleMappingHelper.IsUserMappedToModule(model.UserId, Request.Url.AbsoluteUri))
                {
                    // model.GetAllProjects(model.UserId);
                    model.DailyTaskList = model.GetAllTaskByDateRange(model.UserId, model.WeekStartDate, model.WeekEndDate);
                    model.ProjectsList = model.GetAllProjects(model.UserId);

                    return View(model);
                }
                else
                    return RedirectToAction("DashBoard", "Home");
            }
            else
            {
                return RedirectToAction("DashBoard", "Home");
            }

        }


        [HttpPost]
        public ActionResult InsertDailyTask(string taskDate, int projectID,  string taskName, string descreption,int ProjectTaskID, double Duration,string Ticketno)

        {
            string result = string.Empty;
            CultureInfo provider = CultureInfo.InvariantCulture;
            DailyTaskViewModel model = new DailyTaskViewModel();
            try
            {
                 model.DailyTaskModel.TaskDate = DateTime.ParseExact(taskDate, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, provider, DateTimeStyles.None);

                //if (startTime.ToShortDateString() != Convert.ToDateTime(model.DailyTaskModel.TaskDate).ToShortDateString())
                //{
                //    var tempStartTime = startTime.TimeOfDay;
                //    var tempEndTime = endTime.TimeOfDay;

                //    //DateTime NewstartTimeAB = DateTime.Parse(startTime);

                //    DateTime newStartTime = new DateTime(model.DailyTaskModel.TaskDate.Year, model.DailyTaskModel.TaskDate.Month, model.DailyTaskModel.TaskDate.Day, tempStartTime.Hours, tempStartTime.Minutes, startTime.Second);
                //    DateTime newEndtTime = new DateTime(model.DailyTaskModel.TaskDate.Year, model.DailyTaskModel.TaskDate.Month, model.DailyTaskModel.TaskDate.Day, tempEndTime.Hours, tempEndTime.Minutes, tempEndTime.Seconds);
                //    startTime = newStartTime;
                //    endTime = newEndtTime;
                //}
               


                if (ValidateMaxTaskTimeInsert(Duration ,taskDate,0))  //endTime
                 
                {
                    int userId = Convert.ToInt32(Session["sessUser"]);

                    if (userId != 0)
                    model.DailyTaskModel.UserID = userId;
                    model.DailyTaskModel.ProjectTaskID = ProjectTaskID;  //create by malabika 14-11-2019
                    model.DailyTaskModel.ProjectID = projectID;
                    model.DailyTaskModel.TaskName = taskName;
                    //model.DailyTaskModel.StartTime = startTime;
                    //model.DailyTaskModel.EndTime = endTime;
                    model.DailyTaskModel.Duration = Duration;
                    model.DailyTaskModel.Ticketno = Ticketno;
                    model.DailyTaskModel.Description = descreption;
                    model.DailyTaskModel.CreatedBy = userId.ToString();
                    model.DailyTaskModel.CreateDate = DateTime.Now;
                    model.DailyTaskModel.IsActive = true;

                    model.InsertDailyTaskdata(model.DailyTaskModel, out int id);
                    if (id > 0)
                    {
                        result = "Success";
                        TempData["ErrStatus"] = result;

                    }

                    RedirectToAction("Index", "DailyTask");
                }
                else
                {
                    result = "Invalid";
                    
                }

            }
            catch (Exception ex)
            {
                result = "Error";

                throw;
            }
            return Json(result);
        }


        //[HttpPost]
        //public ActionResult UpdateDailyTask(DailyTaskModel model)
        //{
        //    string result = string.Empty;
        //    DailyTaskViewModel dailyTaskViewModel = new DailyTaskViewModel();
        //    try
        //    {

        //        DateTime startTime = model.StartTime;
        //        DateTime endTime = model.EndTime;

        //        if (model.StartTime.ToShortDateString() != Convert.ToDateTime(model.TaskDate).ToShortDateString())
        //        {
        //            var tempStartTime = model.StartTime.TimeOfDay;
        //            var tempEndTime = model.EndTime.TimeOfDay;

        //            //DateTime NewstartTimeAB = DateTime.Parse(startTime);

        //            DateTime newStartTime = new DateTime(model.TaskDate.Year, model.TaskDate.Month, model.TaskDate.Day, tempStartTime.Hours, tempStartTime.Minutes, model.StartTime.Second);
        //            DateTime newEndtTime = new DateTime(model.TaskDate.Year, model.TaskDate.Month, model.TaskDate.Day, tempEndTime.Hours, tempEndTime.Minutes, tempEndTime.Seconds);
        //            startTime = newStartTime;
        //            endTime = newEndtTime;
        //        }


        //        DailyTaskModel taskModel = new DailyTaskModel();
        //        taskModel.DailyTaskId = model.DailyTaskId;
        //        taskModel.TaskDate = model.TaskDate;
        //        taskModel.TaskName = model.TaskName;
        //        taskModel.StartTime = startTime;
        //        taskModel.EndTime = endTime;
        //        taskModel.Description = model.Description;
        //        taskModel.EditedBy = Session["sessUser"].ToString();
        //        taskModel.EditedDate = DateTime.Now;

        //        bool updateResut = dailyTaskViewModel.UpdateDailyTaskdata(taskModel);
        //        if (updateResut)
        //            result = "Success";

        //    }
        //    catch (Exception)
        //    {
        //        result = "Error";

        //        throw;
        //    }
        //    return Json(new
        //    {
        //        DailyTaskId = model.DailyTaskId,
        //        TaskDate = model.TaskDate.ToShortDateString(),
        //        //StartTime = model.StartTime.ToLongTimeString().Substring(0, 5),
        //        //EndTime = model.EndTime.ToLongTimeString().Substring(0, 5),

        //        StartTime = model.StartTime.ToString("HH:mm"),
        //        EndTime = model.EndTime.ToString("HH:mm"),
        //        Description = model.Description,
        //        TaskName = model.TaskName,
        //        Hours = CalculateTimeDiffrence(model.StartTime, model.EndTime)
        //    });

        //}

        [HttpPost]
        public ActionResult UpdateDailyTask(DailyTaskModel model)
        {
            string result = string.Empty;
            CultureInfo provider = CultureInfo.InvariantCulture;
            DailyTaskViewModel dailyTaskViewModel = new DailyTaskViewModel();
            try
            {
                 
                DateTime startTime = Convert.ToDateTime(model.StartTimeString);
               // string startTime1 = model.StartTime;
                model.StartTime = startTime;
                //  DateTime endTime = Convert.ToDateTime(model.EndTimeString);
                //  model.EndTime = endTime;
               

              if  (ValidateMaxTaskTimeInsert(model.Duration, model.CurrentTaskdate, model.DailyTaskId))
                {

                    DailyTaskModel taskModel = new DailyTaskModel();
                    taskModel.DailyTaskId = model.DailyTaskId;
                    //  taskModel.TaskDate = model.TaskDate;
                    taskModel.TaskDate = DateTime.ParseExact(model.CurrentTaskdate, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, provider, DateTimeStyles.None);
                    taskModel.TaskName = model.TaskName;
                    taskModel.StartTime = startTime;
                    // taskModel.EndTime = endTime;
                    taskModel.Description = model.Description;
                    taskModel.EditedBy = Session["sessUser"].ToString();
                    taskModel.EditedDate = DateTime.Now;
                    taskModel.Duration = model.Duration;
                    taskModel.Ticketno = model.Ticketno;


                    bool updateResut = dailyTaskViewModel.UpdateDailyTaskdata(taskModel);
                    if (updateResut)
                    {
                        result = "Success";
                    }

                    else
                    {
                        return null;
                    }
                }
                else{
                    result = "Invalid";
                    
                }
               

            }
            catch (Exception ex)
            {
                result = "Error";

                throw;
            }
            return Json(new
            {
                DailyTaskId = model.DailyTaskId,
                //  TaskDate = model.TaskDate.ToShortDateString(),
                Ticketno = model.Ticketno,
                StartTime = model.StartTime.ToString("HH:mm"),
                EndTime = model.EndTime.ToString("HH:mm"),
                TaskDescription = model.Description,
                TaskName = model.TaskName,
                ReturnResult = result
                // Hours = CalculateTimeDiffrence(model.StartTime, model.EndTime)

            });

        }
        [HttpPost]
        public ActionResult DeleteDailyTask(int ID)
        {
            string result = string.Empty;
            DailyTaskViewModel dailyTaskViewModel = new DailyTaskViewModel();
            try
            {
                //DailyTaskModel dailyTaskModel = new DailyTaskModel();
                //dailyTaskModel.DailyTaskId = model.DailyTaskId;
                var editedBy = Convert.ToInt32(Session["sessUser"]);
                var editedOn = DateTime.Now;
                bool updateResut = dailyTaskViewModel.DeleteDailyTaskByID(ID, editedBy.ToString(), editedOn);
                if (updateResut)
                    result = "Success";

            }
            catch (Exception)
            {
                result = "Error";

                throw;
            }
            return Json(result);

        }
        //private string CalculateTimeDiffrence(DateTime startTime, DateTime endTime)  
        //{
        //    try
        //    {
        //        TimeSpan ts = endTime - startTime;  

        //        return ts.ToString(@"hh\:mm");

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private string CalculateTimeDiffrence(double duration)
        {
            try
            {

                double Totalduration;
                //  string []hms = duration.ToString().Split('.'); // split it at the colons
                // minutes are worth 60 seconds. Hours are worth 60 minutes.
                //  double seconds = (Convert.ToDouble(hms[0])) * 60 * 60 + (Convert.ToDouble(hms[1])) * 60;
                // Totalduration = seconds / 3600;
                Totalduration = duration;     
                return Totalduration.ToString(); 

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool ValidateMaxTaskTimeInsert(double duration,string taskdate,int taskid)
        {
            bool result = false;
            CultureInfo provider = CultureInfo.InvariantCulture;
            DailyTaskViewModel model = new DailyTaskViewModel();


            try
            {
                model.UserId = Convert.ToInt32(Session["sessUser"]);
                string[] Array_Duration;
                model.DailyTaskList = model.GetAllTaskByDateRange(model.UserId, model.WeekStartDate, model.WeekEndDate);
                DateTime CurrentTaskdate = DateTime.ParseExact(taskdate, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, provider, DateTimeStyles.None);
                var TodayDate = CurrentTaskdate.Date;
                var task_duration_time = 0.0;
                string hour = "";
                string minute = "";
                for (var i = 0; i < model.DailyTaskList.Count; i++)
                {
                    var TodayCreateDate = model.DailyTaskList[i].TaskDate.Date;
                    if (model.DailyTaskList[i].DailyTaskId != taskid)
                    {
                        if (TodayCreateDate == TodayDate)
                        {
                            Array_Duration = model.DailyTaskList[i].Duration.ToString().Split('.');
                            if (Array_Duration.Length == 2)
                            {
                                if (Array_Duration[0].Length == 1)
                                {
                                    hour = "0" + Array_Duration[0].ToString();
                                }
                                else
                                {
                                    hour = Array_Duration[0].ToString();
                                }
                                if (Array_Duration[1].Length == 1)
                                {
                                    minute = Array_Duration[1].ToString() + "0";
                                }
                                else
                                {
                                    minute = Array_Duration[1].ToString();
                                }
                            }
                            else
                            {
                                if (Array_Duration[0].Length == 1)
                                {
                                    hour = "0" + Array_Duration[0].ToString();
                                }
                                else
                                {
                                    hour = Array_Duration[0].ToString();
                                }
                                minute = "0";
                            }

                            double Previous_duration_minute = (Convert.ToDouble(hour) * 60 ) + (Convert.ToDouble(minute));
                            double Previouse_task_duration_time = Previous_duration_minute / 60;
                            task_duration_time = task_duration_time + Previouse_task_duration_time;
                        }
                    }
                   
                }
      
                Array_Duration = duration.ToString().Split('.');

                if (Array_Duration.Length == 2)
                {
                    if (Array_Duration[0].Length == 1)
                    {
                        hour = "0" + Array_Duration[0].ToString();
                    }
                    else
                    {
                        hour = Array_Duration[0].ToString();
                    }
                    if (Array_Duration[1].Length == 1)
                    {
                        minute = Array_Duration[1].ToString() + "0";
                    }
                    else
                    {
                        minute = Array_Duration[1].ToString();
                    }
                }
                else
                {
                    if (Array_Duration[0].Length == 1)
                    {
                        hour = "0" + Array_Duration[0].ToString();
                    }
                    else
                    {
                        hour = Array_Duration[0].ToString();
                    }
                    minute = "0";
                }

                double duration_minute = (Convert.ToDouble(hour) * 60  )+ (Convert.ToDouble(minute));
               duration = duration_minute / 60;
               if (task_duration_time+duration >24)
              
                    result = false;
                else
                    result = true;
                 
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }

        //private bool ValidateMaxTaskTimeInsert(DateTime startTime, DateTime endTime) 
        //{
        //    bool result = false;
        //    DailyTaskViewModel model = new DailyTaskViewModel();
        //    try
        //    {
        //        model.UserId = Convert.ToInt32(Session["sessUser"]);
        //        model.DailyTaskList = model.GetAllTaskByDateRange(model.UserId, model.WeekStartDate, model.WeekEndDate);
        //        var dailyTasks = from r in model.DailyTaskList
        //                         where r.StartTime.ToShortDateString() == startTime.ToShortDateString()
        //                         select r;
        //        var dateTaskList = dailyTasks.ToList();
        //        TimeSpan PreviousTaskSum = TimeSpan.Zero;

        //        foreach (var item in dateTaskList)
        //        {
        //            var hour = CalculateTimeDiffrence(item.StartTime, item.EndTime);
        //            PreviousTaskSum = PreviousTaskSum.Add(TimeSpan.Parse(hour));
        //        }

        //        var currentTaskSum = CalculateTimeDiffrence(startTime, endTime);  

        //        if (PreviousTaskSum.Add(TimeSpan.Parse(currentTaskSum)).TotalHours > 24)
        //            result = false;
        //        else
        //            result = true;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    return result;
        //}

        public ActionResult SkillManagement()
        {
            if (System.Web.HttpContext.Current.Session["sessUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult LoadSkillManagementDetails()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                SkillManagementModel smm = new SkillManagementModel();
                DataSet dsUserDetail = (DataSet)Session["sessUserAllData"];
                DataSet ds = smm.GetSkillManagementDetailData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(dsUserDetail.Tables[2].Rows[0]["ORGID"]));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table class='table table-bordered' id='tblSkillManagement'  width='100%'>");
                    sbContent.Append("<thead>");
                    sbContent.Append("<tr>");
                    //sbContent.Append("<th class='text-center tblHeaderColor'>Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Category</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Skill Code</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Skill Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Proficiency Level</th>");
                    sbContent.Append("</tr>");
                    sbContent.Append("</thead>");
                    sbContent.Append("<tbody id='tbodySkillManagement'>");
                    int counter = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        //sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["userName"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["Category"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["SkillCode"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["SkillName"]) + "</span></td>");
                        string optionValToSelect = "0";
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow drMapSk in ds.Tables[2].Rows)
                            {
                                if (Convert.ToString(dr["ID"]) == Convert.ToString(drMapSk["skillID"]))
                                {
                                    optionValToSelect = Convert.ToString(drMapSk["skillratingID"]);break;
                                }
                            }
                        }
                        sbContent.Append("<td>" + GetDDlSkillRankype(ds, counter, "V", optionValToSelect) + "<input type='hidden' id='hidSkillID" + counter + "' name='hidSkillID[]' value='" + Convert.ToString(dr["ID"]) + "'></td>");
                        sbContent.Append("</tr>");
                        counter++;
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    //sbContent.Append("</div>");

                    sbContent.Append("<div class='row'>");
                    sbContent.Append("<div class='col-md-12'>");
                    sbContent.Append("<div class='dvBorder form-group'>");
                    sbContent.Append("<input type='submit' id='btnSkillSave' value='Save' name='Command' class='btn btn-default' onclick='saveSkillData()' />");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        private string GetDDlSkillRankype(DataSet ds, int rowNo, string byValOrText, string optionValToSelect = "0")
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<select class='form-control' id='ddlSkillPerspective" + rowNo.ToString() + "' name='ddlSkillPerspective[]'>");
                sb.Append("<option value='0'>Select</option>");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        if (byValOrText == "V")
                        {
                            if (optionValToSelect != "0" && Convert.ToString(dr["ID"]) == optionValToSelect)
                                sb.Append("<option value='" + Convert.ToString(dr["ID"]) + "' selected>" + Convert.ToString(dr["SkillLevel"]) + "</option>");
                            else
                                sb.Append("<option value='" + Convert.ToString(dr["ID"]) + "'>" + Convert.ToString(dr["SkillLevel"]) + "</option>");
                        }
                        else
                        {
                            if (Convert.ToString(dr["SkillLevel"]).Trim().ToLower().Contains(optionValToSelect.Trim().ToLower()))
                                sb.Append("<option value='" + Convert.ToString(dr["ID"]) + "' selected>" + Convert.ToString(dr["SkillLevel"]) + "</option>");
                            else
                                sb.Append("<option value='" + Convert.ToString(dr["ID"]) + "'>" + Convert.ToString(dr["SkillLevel"]) + "</option>");
                        }
                    }
                }
                sb.Append("</select>");
            }
            catch (Exception exE)
            {
                try
                {
                    using (ErrorHandle errH = new ErrorHandle())
                    { errH.WriteErrorLog(exE); }
                }
                catch (Exception exC) { }
            }
            return sb.ToString();
        }
        public ActionResult saveUserSkillData(string SkillID, string SkillRate)
        {
           
            StringBuilder sb = new StringBuilder();
            try
            {
                MapUserSkill mMUSkill = new MapUserSkill();
                int UserID = Convert.ToInt32(Session["sessUser"]);
                string[] arrSkillID = SkillID.Split('|');
                string[] arrSkillRate = SkillRate.Split('|');
                List<MapUserSkill> lstUserSkill = new List<MapUserSkill>();
                for (int i = 0; i < arrSkillID.Length; i++)
                {
                    try
                    {
                        if (arrSkillRate[i].Trim() != "0")
                        {
                            MapUserSkill mMUS = new MapUserSkill();
                            mMUS.userID = UserID;
                            mMUS.skillID = Convert.ToInt32(arrSkillID[i]);
                            mMUS.SkillRatingID = Convert.ToInt32(arrSkillRate[i]);
                            mMUS.CreatedBy = UserID;
                            mMUS.CreateDate = DateTime.Now;
                            lstUserSkill.Add(mMUS);
                        }
                    }
                    catch (Exception exx) { }
                }
                var returnStatus = mMUSkill.InsertUserSkillData(lstUserSkill);
                if (returnStatus)
                {
                    string strReturnMessage = "Your data has been saved successfully.";
                    sb.Append(strReturnMessage);
                }
                else sb.Append("Data could not save. Please try after some time.");
            }
            catch (Exception exE)
            {
                try
                {
                    using (ErrorHandle errH = new ErrorHandle())
                    { errH.WriteErrorLog(exE); }
                }
                catch (Exception exC) { }
            }
            return Json(sb.ToString());
        }


        //public ActionResult BindIssueList(int ProjectId)
        //{
          
        //    DailyTaskViewModel model = new DailyTaskViewModel();
        //    ProjectIssueModel issue = new ProjectIssueModel();
        //    try
        //    {
               
        //        model.IssueList = model.Get_IssueListByProject(ProjectId, model.UserId);
        //       // model.ProjectsList = model.GetAllProjects(model.UserId);
        //        //if (dt.Rows.Count > 0)
        //        //{
        //        //    foreach (DataRow item in dt.Rows)
        //        //    {
        //        //         issue = new ProjectIssueModel();
        //        //        issue.IssueId = Convert.ToInt32(item["IssueID"]);
        //        //        issue.IssueCode = item["IssueCode"].ToString();
        //        //        issue.IssueName = item["IssueName"].ToString();
        //        //        dailyViewTaskModel.IssueList.Add(issue);

        //        //    }
        //        //}


        //    }
        //    catch (Exception)
        //    {

        //        //throw;
        //    }
        //    return Json(JsonConvert.SerializeObject(model.IssueList));


        //}
    }
}