using Newtonsoft.Json.Linq;
using QBA.Qutilize.DataAccess.DAL;
using QBA.Qutilize.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace QBA.Qutilize.WebAPI.Controllers
{
    public class DailyTaskController : ApiController
    {
        QUtilizeDBCon _dbContext;
        public DailyTaskController()
        {
            _dbContext = new QUtilizeDBCon();
        }

        [HttpPost]
        [Route("api/DailyTask/UpdateStartTime/")]
        [ResponseType(typeof(JObject))]
        public int UpdateStartTime(DailyTaskModel dailyTaskModel)
        {
            var result = _dbContext.USPDailyTasks_InsertTaskStartTime(dailyTaskModel.UserId, dailyTaskModel.ProjectId, DateTime.Now, dailyTaskModel.UserId.ToString(), true);
            return result;
        }



        [HttpPost()]
        [Route("api/DailyTask/UpdateEndTime/")]
        [ResponseType(typeof(JObject))]
        public DailyTask UpdateEndTime(DailyTaskModel dailyTaskModel)
        {
            DailyTask dailyTask = null;
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
