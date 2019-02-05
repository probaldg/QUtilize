using Newtonsoft.Json.Linq;
using QBA.Qutilize.DataAccess.DataModel;
using QBA.Qutilize.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace QBA.Qutilize.WebAPI.Controllers
{
    public class DailyTaskController : ApiController
    {
        QutilizeModel _dbContext;
        public DailyTaskController()
        {
            _dbContext = new QutilizeModel();
        }

        [HttpPost]
        [Route("api/DailyTask/UpdateStartTime/")]
        [ResponseType(typeof(JObject))]
        public DailyTask UpdateStartTime(DailyTaskModel dailyTaskModel)
        {
            DailyTask dt=  new DailyTask
            {
                ProjectID = dailyTaskModel.ProjectId,
                UserID = dailyTaskModel.UserId,
                StartDateTime = dailyTaskModel.StartTime,
              
                CreatedBy = dailyTaskModel.UserId.ToString(),
                CreateDate = dailyTaskModel.StartTime,
                IsActive = true
            };
            _dbContext.DailyTasks.Add(dt);
            _dbContext.SaveChanges();

            
            return dt;
        }



        [HttpPost()]
        [Route("api/DailyTask/UpdateEndTime/")]
        [ResponseType(typeof(JObject))]
        public DailyTask UpdateEndTime(DailyTaskModel dailyTaskModel)
        {
            DailyTask dailyTask= null;
            try
            {
                dailyTask = _dbContext.DailyTasks.FirstOrDefault(x => x.DailyTaskId == dailyTaskModel.DailyTaskId);

                if (dailyTask != null)
                    dailyTask.EndDateTime = DateTime.Now;

                _dbContext.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }

            return dailyTask;
        }
    }
}
