using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.Models
{
    public class SelectedProject
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public DateTime StrartDateTime { get; set; }
        public int EndDateTime { get; set; }
    }
}
