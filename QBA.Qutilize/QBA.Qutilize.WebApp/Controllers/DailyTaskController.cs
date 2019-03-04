using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class DailyTaskController : Controller
    {
        // GET: DailyTask
        public ActionResult Index()
        {
            //DailyTaskModel.UserID = Convert.ToInt32(Session["sessUser"]);

            DailyTaskModel model = new DailyTaskModel();
            model.UserID = Convert.ToInt32(Session["sessUser"]);
            model.ProjecSelectList = new SelectList(model.GetAllProjects(model.UserID), "ProjectID", "ProjectName");


            //Get the data for the date range for the user id
            var dt = model.GetAllTaskByDateRange(model.UserID, model.WeekStartDate, model.WeekEndDate);
            List<DailyTaskModel> projectModels = new List<DailyTaskModel>();
            if(dt.Rows.Count> 0)
            {
                foreach  (DataRow dataRow in dt.Rows)
                {
                    DailyTaskModel tskModel = new DailyTaskModel();
                    tskModel.DailyTaskId = Convert.ToInt32(dataRow["DailyTaskId"]);
                    tskModel.Description = dataRow["Description"].ToString();
                    tskModel.TaskName = dataRow["TaskName"].ToString();
                    tskModel.ProjectName = dataRow["ProjectName"].ToString();
                    tskModel.TaskDate = Convert.ToDateTime(dataRow["TaskDate"].ToString());
                    tskModel.StartTimeToDisplay = dataRow["StartTime"].ToString();
                    tskModel.EndTimeToDisplay = dataRow["EndTime"].ToString();
                    tskModel.StartTime =Convert.ToDateTime(dataRow["StartTime"]);
                    tskModel.EndTime = Convert.ToDateTime(dataRow["EndTime"]);
                    projectModels.Add(tskModel);
                }
                
            }
         

            //ViewBag.Columns = columns;
            ////Converting datatable to dynamic list     
            //var dns = new List<dynamic>();
            //dns = ConvertDtToList(dt);
            //ViewBag.Total = dns;
            return View(projectModels);
        }


        //[HttpPost]
        //public ActionResult InsertDailyTask(DailyTaskModel model)
        //{
        //  return  RedirectToAction("Index", "DailyTask");
        //}

        [HttpPost]
        public ActionResult InsertDailyTask(int projectID, DateTime startTime, DateTime endTime, string taskName, string Description, DateTime taskDate)
        {
            string result = string.Empty;
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


        public List<dynamic> ConvertDtToList(DataTable dt)
        {
            var data = new List<dynamic>();
            foreach (var item in dt.AsEnumerable())
            {
                // Expando objects are IDictionary<string, object>  
                IDictionary<string, object> dn = new ExpandoObject();

                foreach (var column in dt.Columns.Cast<DataColumn>())
                {
                    dn[column.ColumnName] = item[column];
                }

                data.Add(dn);
            }
            return data;
        }
    }
}