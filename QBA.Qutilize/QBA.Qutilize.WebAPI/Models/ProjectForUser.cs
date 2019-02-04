using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebAPI.Models
{
    public class ProjectForUser
    {
        public int UserId { get; set; }
        public string ProjectName { get; set; }
    }
}