using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class DailyTaskViewModel : ViewModelBase
    {
        DailyTask _dailyTask;
        public DailyTaskViewModel(DailyTask dailyTask, User user)
        {
            Projects = new ObservableCollection<Project>();
            _dailyTask = dailyTask;
           // CreateItemSourceforTreeView();
        }

        private int? _userId;

        public int? UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged("UserId");
            }
        }

        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        private ObservableCollection<Project> _parentProject;

        public ObservableCollection<Project> ParentProject

        {
            get { return GetOnlyParentProject(); }
            set
            {
                _parentProject = value;
                OnPropertyChanged("ParentProject");
            }
        }

        private ObservableCollection<Project> _childProject;

        public ObservableCollection<Project> ChildProject

        {
            get { return _childProject; }
            set
            {
                _childProject = value;
                OnPropertyChanged("ChildProject");
            }
        }

        private ObservableCollection<Project> _projects;

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                OnPropertyChanged("Projects");
            }
        }

        private Project _selectedProject;

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set { _selectedProject = value; }
        }


        private bool _taskStarted;

        public bool TaskStarted
        {
            get { return _taskStarted; }
            set { _taskStarted = value;
                OnPropertyChanged("TaskStarted"); }
        }


        private ObservableCollection<Project> GetOnlyParentProject()
        {
            ObservableCollection<Project> parentProjt = new ObservableCollection<Project>();
            if (Projects != null && Projects.Count > 0)
            {
                var projects = Projects.Where(x => x.ParentProjectID == null).ToArray();

                foreach (var item in projects)
                {
                    parentProjt.Add(item);
                }
            }

            return parentProjt;
        }

        private void GetChildProjectbyProjectID(int projectId)
        {
            ObservableCollection<Project> childProjt = new ObservableCollection<Project>();
            if (Projects != null && Projects.Count > 0)
            {
                var projects = Projects.Where(x => x.ParentProjectID == projectId).ToArray();

                foreach (var item in projects)
                {
                    childProjt.Add(item);
                }
            }

            ChildProject = childProjt;

        }


        public ICommand UpdateCommandFromSelectedProject
        {
            get
            {
                return new CommandHandler(_ => UpdateTask());
            }
        }

        private void UpdateTask()
        {
           if(SelectedProject != null)
            {
                MessageBox.Show(SelectedProject.ProjectName);
            }
        }

        //private void GetChiildNode()
        //{
        //    MessageBox.Show("test");
        //}
        //private void CreateItemSourceforTreeView()
        //{
        //    ObservableCollection<ProjectListForTreeView> projectListForTreeViews = new ObservableCollection<ProjectListForTreeView>();

        //    foreach (var item in Projects)
        //    {
        //        // extracting child element first
        //        ProjectListForTreeView projectListForTreeView = new ProjectListForTreeView();
        //        projectListForTreeView.ProjectName = item.ProjectName;

        //        var subProjectLis = Projects.Where(x => x.ParentProjectID == item.ProjectID).ToArray();
        //        ObservableCollection<ProjectForTreeView> projectForTrees = new ObservableCollection<ProjectForTreeView>();

        //        foreach (var project in subProjectLis)
        //        {
        //            ProjectForTreeView proj = new ProjectForTreeView();
        //            proj.ProjectName = project.ProjectName;
        //            proj.ProjectId = project.ProjectID;
        //            projectForTrees.Add(proj);
        //        }
        //        projectListForTreeView.SubProjectList= projectForTrees;
        //    }
        //}
    }
}
