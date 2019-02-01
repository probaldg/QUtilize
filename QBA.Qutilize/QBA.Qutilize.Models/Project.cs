using System;
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
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public int? ParentProjectID { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
