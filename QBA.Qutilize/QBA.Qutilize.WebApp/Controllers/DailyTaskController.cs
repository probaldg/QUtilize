using QBA.Qutilize.Models;
using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
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
                // int userId = Convert.ToInt32(Session["sessUser"]);
                model.UserId = Convert.ToInt32(Session["sessUser"]);
                model.GetAllProjects(model.UserId);
                model.DailyTaskList = model.GetAllTaskByDateRange(model.UserId, model.WeekStartDate, model.WeekEndDate);
                model.ProjectsList = model.GetAllProjects(model.UserId);
                return View(model);
            }
            else
            {
                return Redirect("/Home/Index");
            }

        }


        [HttpPost]
        public ActionResult InsertDailyTask(int projectID, DateTime startTime, DateTime endTime, string taskName, string Description, string taskDate)
        {
            string result = string.Empty;
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {

                int userId = Convert.ToInt32(Session["sessUser"]);
                DailyTaskViewModel model = new DailyTaskViewModel();
                if (userId != 0)
                    model.DailyTaskModel.UserID = userId;

                model.DailyTaskModel.ProjectID = projectID;
                model.DailyTaskModel.TaskName = taskName;
               // model.DailyTaskModel.TaskDate =Convert.ToDateTime(taskDate,);
                model.DailyTaskModel.TaskDate = DateTime.ParseExact(taskDate, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" },provider, DateTimeStyles.None);

                model.DailyTaskModel.StartTime = startTime;
                model.DailyTaskModel.EndTime = endTime;
                model.DailyTaskModel.Description = Description;
                model.DailyTaskModel.CreatedBy = userId.ToString();
                model.DailyTaskModel.CreateDate = DateTime.Now;
                model.DailyTaskModel.IsActive = true;

                model.InsertDailyTaskdata(model.DailyTaskModel, out int id);
                if (id > 0)
                    result = "Success";
                RedirectToAction("Index", "DailyTask");
               

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
                Hours = CalculateTimeDiffrence(model.StartTime, model.EndTime)
            });

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