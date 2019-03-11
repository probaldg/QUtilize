using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.Models
{
    public class User
    {
        public User()
        {
            Projects = new List<Project>();
            Roles = new List<Roles>();
        }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }

        public string EmailId { get; set; }
        public ICollection<Roles> Roles { get; set; }
        public ICollection<Project> Projects { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }

    }
}
