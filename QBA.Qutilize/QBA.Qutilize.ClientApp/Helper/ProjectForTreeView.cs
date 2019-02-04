using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.ClientApp.Helper
{
   public class ProjectForTreeView:ViewModelBase
    {

        public ProjectForTreeView()
        {

        }
        public ProjectForTreeView(int projectId, string projectName)
        {
            _projectId = projectId;
            _projectName = projectName;
        }
        private int _projectId;

        public int ProjectId
        {
            get { return _projectId; }
            set { _projectId = value;
                OnPropertyChanged("ProjectId");
            }
        }

        private string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; OnPropertyChanged("ProjectName"); }
        }

       
    }
}
