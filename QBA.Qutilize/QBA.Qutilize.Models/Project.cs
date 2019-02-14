﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.Models
{
    public class Project
    {
        public Project()
        {
            Users = new List<User>();
        }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }

        public string Description { get; set; }
        public Boolean   IsCurrentProject { get; set; }
        public int? ParentProjectID { get; set; }
        public int? MaxProjectTimeInHours { get; set; }
        public ICollection<User> Users { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
