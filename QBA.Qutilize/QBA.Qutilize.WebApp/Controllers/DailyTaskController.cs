using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class DailyTaskController : Controller
    {
        // GET: DailyTask
        public ActionResult Index()
        {
            DailyTaskModel model = new DailyTaskModel();
            return View(model);
        }

        //[HttpPost]
        //public ActionResult InsertDailyTask(DailyTaskModel model)
        //{
        //  return  RedirectToAction("Index", "DailyTask");
        //}

        [HttpPost]
        public ActionResult InsertDailyTask(int projectID, DateTime startTime, DateTime endTime, string taskName, string Description, DateTime taskDate)
        {
            string result= string.Empty;
            try
            {

                int userId = Convert.ToInt32(Session["sessUser"]);
                DailyTaskModel model = new DailyTaskModel
                {
                    UserID = userId,
                    ProjectID = projectID,
                    TaskName = taskName,
                    TaskDate = taskDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    Description = Description,
                    CreatedBy = userId.ToString(),
                    CreateDate = DateTime.Now,
                    IsActive = true
                };

                model.InsertDailyTaskdata(model, out int id);
                if (id > 0)
                    result = "Success";

            }
            catch (Exception)
            {
                result = "Error";
                
                throw;
            }
            return Json(result);
        }
        public ActionResult InsertDailyTask()
        {
            return RedirectToAction("Index", "DailyTask");
        }
    }
}