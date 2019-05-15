using System;


namespace QBA.Qutilize.Models
{
    public class DailyTaskModel
    {
        public int DailyTaskId { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectTaskID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }



    }
}
