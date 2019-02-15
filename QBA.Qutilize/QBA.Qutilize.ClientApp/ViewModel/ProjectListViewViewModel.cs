using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.Models;
using System.Collections.ObjectModel;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class ProjectListViewViewModel : ViewModelBase
    {
        public ProjectListViewViewModel()
        {
            Projects = new ObservableCollection<Project>();
        }
        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();
        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                OnPropertyChanged("Projects");
                
            }
        }

        private Project _seletedProject;

        public Project SelectedProject
        {
            get { return _seletedProject; }
            set { _seletedProject = value;
                OnPropertyChanged("SelectedProject");
            }
        }

    }
}
