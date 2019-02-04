using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QBA.Qutilize.DataAccess.DataModel
{
    public class UserRoles
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order =1)]
        public int RoleId { get; set; }

    }
}
