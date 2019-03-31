using System;

namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectStatusModel
    {
        public int StatusID { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }

        public int StatusRank { get; set; }
        public int OrgID { get; set; }
        public Boolean IsActive { get; set; }

    }
}