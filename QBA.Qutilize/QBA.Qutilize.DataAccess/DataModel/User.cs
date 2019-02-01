using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.DataAccess.DataModel
{
    public class User
    {
        public User()
        {
            Roles = new HashSet<Role>();
            Projects = new HashSet<Project>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public  ICollection<Role> Roles { get; set; }

        public ICollection<Project> Projects { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }

        public bool IsActive { get; set; }

    }
}
