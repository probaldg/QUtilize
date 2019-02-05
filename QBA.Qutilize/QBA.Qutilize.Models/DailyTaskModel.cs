using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace QBA.Qutilize.Models
{
   public class DailyTaskModel
    {
        public int DailyTaskId { get; set; }
        public int? UserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


      
    }
}
