using QBA.Qutilize.Models;
using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
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

                if (startTime.ToShortDateString() != Convert.ToDateTime(taskDate).ToShortDateString())
                {
                    var tempStartTime = startTime.TimeOfDay;
                    var tempEndTime = endTime.TimeOfDay;
                    DateTime newStartTime = new DateTime(model.DailyTaskModel.TaskDate.Year, model.DailyTaskModel.TaskDate.Month, model.DailyTaskModel.TaskDate.Day, tempStartTime.Hours, tempStartTime.Minutes, startTime.Second);
                    DateTime newEndtTime = new DateTime(model.DailyTaskModel.TaskDate.Year, model.DailyTaskModel.TaskDate.Month, model.DailyTaskModel.TaskDate.Day, tempEndTime.Hours, tempEndTime.Minutes, tempEndTime.Seconds);
                    startTime = newStartTime;
                    endTime = newEndtTime;
                }

              if( ValidateMaxTaskTimeInsert(startTime, endTime))
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
                        result = "Success";
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
        //public ActionResult InsertDailyTask()
        //{
        //    return RedirectToAction("Index", "DailyTask");
        //}

        [HttpPost]
        public ActionResult UpdateDailyTask(DailyTaskModel model)
        {
            string result = string.Empty;
            DailyTaskViewModel dailyTaskViewModel = new DailyTaskViewModel();
            try
            {

                DailyTaskModel taskModel = new DailyTaskModel();
                taskModel.DailyTaskId = model.DailyTaskId;
                taskModel.TaskDate = model.TaskDate;
                taskModel.TaskName = model.TaskName;
                taskModel.StartTime = model.StartTime;
                taskModel.EndTime = model.EndTime;
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
                TaskName=model.TaskName,
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
        private decimal CalculateTimeDiffrence(DateTime startTime, DateTime endTime)
        {
            try
            {
                TimeSpan ts = endTime.TimeOfDay - startTime.TimeOfDay;
                if (ts.Ticks > 0)
                    return Convert.ToDecimal(ts.Hours + "." + ts.Minutes);
                else
                    return 0;
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
                decimal PreviousTaskSum = 0;

                foreach (var item in dateTaskList)
                {
                    var hour = CalculateTimeDiffrence(item.StartTime, item.EndTime);
                    PreviousTaskSum = PreviousTaskSum + hour;
                }

                decimal currentTaskSum = CalculateTimeDiffrence(startTime, endTime);

                if (PreviousTaskSum + currentTaskSum > 24)
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
    }
}