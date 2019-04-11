using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;
using System;
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

        // GET: DailyTask
        public ActionResult Index()
        {
            DailyTaskViewModel model = new DailyTaskViewModel();
            if (System.Web.HttpContext.Current.Session["sessUser"] != null)
            {
                model.UserId = Convert.ToInt32(Session["sessUser"]);
                // string url = Request.Url.AbsoluteUri;
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
        public ActionResult InsertDailyTask(string taskDate, int projectID, DateTime startTime, DateTime endTime, string taskName, string descreption)

        {
            string result = string.Empty;
            CultureInfo provider = CultureInfo.InvariantCulture;
            DailyTaskViewModel model = new DailyTaskViewModel();
            try
            {
                model.DailyTaskModel.TaskDate = DateTime.ParseExact(taskDate, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, provider, DateTimeStyles.None);

                if (startTime.ToShortDateString() != Convert.ToDateTime(model.DailyTaskModel.TaskDate).ToShortDateString())
                {
                    var tempStartTime = startTime.TimeOfDay;
                    var tempEndTime = endTime.TimeOfDay;

                    //DateTime NewstartTimeAB = DateTime.Parse(startTime);

                    DateTime newStartTime = new DateTime(model.DailyTaskModel.TaskDate.Year, model.DailyTaskModel.TaskDate.Month, model.DailyTaskModel.TaskDate.Day, tempStartTime.Hours, tempStartTime.Minutes, startTime.Second);
                    DateTime newEndtTime = new DateTime(model.DailyTaskModel.TaskDate.Year, model.DailyTaskModel.TaskDate.Month, model.DailyTaskModel.TaskDate.Day, tempEndTime.Hours, tempEndTime.Minutes, tempEndTime.Seconds);
                    startTime = newStartTime;
                    endTime = newEndtTime;
                }

                if (ValidateMaxTaskTimeInsert(startTime, endTime))
                {
                    int userId = Convert.ToInt32(Session["sessUser"]);

                    if (userId != 0)
                        model.DailyTaskModel.UserID = userId;

                    model.DailyTaskModel.ProjectID = projectID;
                    model.DailyTaskModel.TaskName = taskName;

                    model.DailyTaskModel.StartTime = startTime;
                    model.DailyTaskModel.EndTime = endTime;

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
            catch (Exception)
            {
                result = "Error";

                throw;
            }
            return Json(result);
        }


        [HttpPost]
        public ActionResult UpdateDailyTask(DailyTaskModel model)
        {
            string result = string.Empty;
            DailyTaskViewModel dailyTaskViewModel = new DailyTaskViewModel();
            try
            {

                DateTime startTime = model.StartTime;
                DateTime endTime = model.EndTime;

                if (model.StartTime.ToShortDateString() != Convert.ToDateTime(model.TaskDate).ToShortDateString())
                {
                    var tempStartTime = model.StartTime.TimeOfDay;
                    var tempEndTime = model.EndTime.TimeOfDay;

                    //DateTime NewstartTimeAB = DateTime.Parse(startTime);

                    DateTime newStartTime = new DateTime(model.TaskDate.Year, model.TaskDate.Month, model.TaskDate.Day, tempStartTime.Hours, tempStartTime.Minutes, model.StartTime.Second);
                    DateTime newEndtTime = new DateTime(model.TaskDate.Year, model.TaskDate.Month, model.TaskDate.Day, tempEndTime.Hours, tempEndTime.Minutes, tempEndTime.Seconds);
                    startTime = newStartTime;
                    endTime = newEndtTime;
                }


                DailyTaskModel taskModel = new DailyTaskModel();
                taskModel.DailyTaskId = model.DailyTaskId;
                taskModel.TaskDate = model.TaskDate;
                taskModel.TaskName = model.TaskName;
                taskModel.StartTime = startTime;
                taskModel.EndTime = endTime;
                taskModel.Description = model.Description;
                taskModel.EditedBy = Session["sessUser"].ToString();
                taskModel.EditedDate = DateTime.Now;

                bool updateResut = dailyTaskViewModel.UpdateDailyTaskdata(taskModel);
                if (updateResut)
                    result = "Success";

            }
            catch (Exception)
            {
                result = "Error";

                throw;
            }
            return Json(new
            {
                DailyTaskId = model.DailyTaskId,
                TaskDate = model.TaskDate.ToShortDateString(),
                StartTime = model.StartTime.ToLongTimeString().Substring(0, 5),
                EndTime = model.EndTime.ToLongTimeString().Substring(0, 5),
                Description = model.Description,
                TaskName = model.TaskName,
                Hours = CalculateTimeDiffrence(model.StartTime, model.EndTime)
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
        private string CalculateTimeDiffrence(DateTime startTime, DateTime endTime)
        {
            try
            {
                TimeSpan ts = endTime.TimeOfDay - startTime.TimeOfDay;

                return ts.ToString(@"hh\:mm");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool ValidateMaxTaskTimeInsert(DateTime startTime, DateTime endTime)
        {
            bool result = false;
            DailyTaskViewModel model = new DailyTaskViewModel();
            try
            {
                model.UserId = Convert.ToInt32(Session["sessUser"]);
                model.DailyTaskList = model.GetAllTaskByDateRange(model.UserId, model.WeekStartDate, model.WeekEndDate);
                var dailyTasks = from r in model.DailyTaskList
                                 where r.StartTime.ToShortDateString() == startTime.ToShortDateString()
                                 select r;
                var dateTaskList = dailyTasks.ToList();
                TimeSpan PreviousTaskSum = TimeSpan.Zero;

                foreach (var item in dateTaskList)
                {
                    var hour = CalculateTimeDiffrence(item.StartTime, item.EndTime);
                    PreviousTaskSum = PreviousTaskSum.Add(TimeSpan.Parse(hour));
                }

                var currentTaskSum = CalculateTimeDiffrence(startTime, endTime);

                if (PreviousTaskSum.Add(TimeSpan.Parse(currentTaskSum)).TotalHours > 24)
                    result = false;
                else
                    result = true;

            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public ActionResult PMSDetails()
        {
            if (System.Web.HttpContext.Current.Session["sessUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index","Home");
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
                    sbContent.Append("<table class='table table-bordered' id='tblSessionHistory'  width='100%'>");
                    sbContent.Append("<thead>");
                    sbContent.Append("<tr>");
                    //sbContent.Append("<th class='text-center tblHeaderColor'>Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>IP Address</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Application</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Start Date Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>End Date Time</th>");
                    sbContent.Append("</tr>");
                    sbContent.Append("</thead>");
                    sbContent.Append("<tbody id='tbodySessionHistoryData'>");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        //sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["userName"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["IPAddress"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["Application"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["StartDateTime"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["EndDateTime"]) + "</span></td>");
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    //sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        private string GetDDlSkillRankype( DataSet ds,int rowNo, string byValOrText, string optionValToSelect = "0")
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<select class='form-control' id='ddlSkillPerspective" + rowNo.ToString() + "' name='ddlSkillPerspective[]'>");
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
    }
}