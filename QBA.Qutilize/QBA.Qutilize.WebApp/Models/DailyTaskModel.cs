using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.Models
{
    public class DailyTaskModel
    {
        public int DailyTaskId { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public DateTime TaskDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TaskName { get; set; }
        public string Discription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion
    }
}