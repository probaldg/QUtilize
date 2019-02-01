using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.DataAccess.DataModel
{
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
