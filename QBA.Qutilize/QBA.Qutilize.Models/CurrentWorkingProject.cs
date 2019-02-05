using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.Models
{
   public class CurrentWorkingProject
    {
        //public Project Project { get; set; }
        public int ProjectID { get; set; }

        public string ProjectName { get; set; }
        public int DailyTaskId { get; set; }
        public DateTime StrartDateTime { get; set; }
        public int EndDateTime { get; set; }

    }
}
