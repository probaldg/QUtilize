using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.DataAccess.DataModel
{
   public class DailyTask
    {
        public int DailyTaskId { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
