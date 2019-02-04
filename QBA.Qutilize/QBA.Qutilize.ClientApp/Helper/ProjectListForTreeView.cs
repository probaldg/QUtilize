using System.Collections.ObjectModel;

namespace QBA.Qutilize.ClientApp.Helper
{
    public class ProjectListForTreeView: ViewModelBase
    {
        public ProjectListForTreeView()
        {
            SubProjectList = new ObservableCollection<ProjectForTreeView>();
        }
        private string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; OnPropertyChanged("ProjectName"); }
        }

        private int _projectId;

        public int ProjectId
        {
            get { return _projectId; }
            set { _projectId = value;
                OnPropertyChanged("ProjectId");
            }
        }

        private ObservableCollection<ProjectForTreeView> _subProjectList;

        public ObservableCollection<ProjectForTreeView> SubProjectList
        {
            get { return _subProjectList; }
            set { _subProjectList = value;
                OnPropertyChanged("SubProjectList");
            }
        }

    }
}
