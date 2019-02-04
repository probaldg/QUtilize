using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.ClientApp.Helper
{
    class ParentProjects: ViewModelBase
    {
        private ObservableCollection<ProjectListForTreeView> _parentProjets;
           

        public ObservableCollection<ProjectListForTreeView> Projets
        {
            get { return _parentProjets; }
            set { _parentProjets = value;
                OnPropertyChanged("ParentProjects"); }
        }

    }
}
