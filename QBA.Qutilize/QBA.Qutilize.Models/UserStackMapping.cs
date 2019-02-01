using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.Models
{
   public class UserStackMapping
    {
        public int ID { get; set; }

       
        public string UserID { get; set; }

        public int Stack { get; set; }

        public  Project Project { get; set; }

        public virtual User User { get; set; }
    }
}
