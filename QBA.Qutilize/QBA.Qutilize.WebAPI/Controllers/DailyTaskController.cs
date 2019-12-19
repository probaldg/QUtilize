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
        public decimal? UpdateStartTime(DailyTaskModel dailyTaskModel)
        {
            decimal? DailyTaskId = 0;
            if (dailyTaskModel == null)
            {
                return 0;
            }

            try
            {
                var queryResult =_dbContext.USPDailyTasks_InsertTaskStartTime(dailyTaskModel.UserId, dailyTaskModel.ProjectId, dailyTaskModel.StartTime.ToString(), dailyTaskModel.UserId.ToString(), true).ToList();

                _dbContext.SaveChanges();
                if (queryResult != null && queryResult.Count > 0)
                {
                    DailyTaskId = queryResult.First();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return DailyTaskId;
        }

        [HttpPost()]
        [Route("api/DailyTask/UpdateEndTime/")]
        [ResponseType(typeof(JObject))]
        public int UpdateEndTime(DailyTaskModel dailyTaskModel)
        {
            if (dailyTaskModel == null)
            {
                throw new ArgumentNullException(nameof(dailyTaskModel));
            }

            int queryResult;
            try
            {
                queryResult = _dbContext.USPDailyTask_UpdateEndTaskTime(dailyTaskModel.DailyTaskId, dailyTaskModel.EndTime.ToString());
                _dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return queryResult;
        }
    }
}
