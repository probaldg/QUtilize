using Newtonsoft.Json;
using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class DailyTaskViewModel : ViewModelBase
    {
        DailyTask _dailyTaskView;
        public DailyTaskViewModel(DailyTask dailyTask, User user)
        {
            _dailyTaskView = dailyTask;
            Projects = new ObservableCollection<Project>();

            UserId = user.ID;
            foreach (var item in user.Projects)
            {
                Projects.Add(item);
            }
            SetDefaultProjectAsCurrentProject();
            InsertProjectStartTime();

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

        //private ObservableCollection<Project> _parentProject;

        //public ObservableCollection<Project> ParentProject

        //{
        //    get { return GetOnlyParentProject(); }
        //    set
        //    {
        //        _parentProject = value;
        //        OnPropertyChanged("ParentProject");
        //    }
        //}

        //private ObservableCollection<Project> _childProject;

        //public ObservableCollection<Project> ChildProject

        //{
        //    get { return _childProject; }
        //    set
        //    {
        //        _childProject = value;
        //        OnPropertyChanged("ChildProject");
        //    }
        //}

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
            set
            {
                _taskStarted = value;
                OnPropertyChanged("TaskStarted");
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private CurrentWorkingProject _currentWorkingProject;

        public CurrentWorkingProject CurrentWorkingProject
        {
            get { return _currentWorkingProject; }
            set
            {
                _currentWorkingProject = value;
                OnPropertyChanged("CurrentWorkingProject");
            }
        }
        //private ObservableCollection<Project> GetOnlyParentProject()
        //{
        //    ObservableCollection<Project> parentProjt = new ObservableCollection<Project>();
        //    if (Projects != null && Projects.Count > 0)
        //    {
        //        var projects = Projects.Where(x => x.ParentProjectID == null).ToArray();

        //        foreach (var item in projects)
        //        {
        //            parentProjt.Add(item);
        //        }
        //    }

        //    return parentProjt;
        //}

        //private void GetChildProjectbyProjectID(int projectId)
        //{
        //    ObservableCollection<Project> childProjt = new ObservableCollection<Project>();
        //    if (Projects != null && Projects.Count > 0)
        //    {
        //        var projects = Projects.Where(x => x.ParentProjectID == projectId).ToArray();

        //        foreach (var item in projects)
        //        {
        //            childProjt.Add(item);
        //        }
        //    }

        //    ChildProject = childProjt;

        //}


        public ICommand UpdateCommandFromSelectedProject
        {
            get
            {
                return new CommandHandler(_ => UpdateTask());
            }
        }
        public ICommand Logout
        {
            get
            {
                return new CommandHandler(_ => LogoutUser());
            }
        }

        private void LogoutUser()
        {
            if(CurrentWorkingProject != null)
            {
                DailyTaskModel dtm = new DailyTaskModel
                {
                    DailyTaskId = CurrentWorkingProject.DailyTaskId,
                    ProjectId = CurrentWorkingProject.ProjectID,
                    UserId = UserId,
                    StartTime = CurrentWorkingProject.StrartDateTime,
                    EndTime = DateTime.Now

                };
                var response = UpdateEndTimeForTheCurrentWorkingProject(dtm).Result;
                CurrentWorkingProject = null;
                User = null;
                SelectedProject = null;
                Login loginView = new Login();
                loginView.Show();

                _dailyTaskView.Close();

               
            }
        }

        private void UpdateTask()
        {
            if (SelectedProject != null)
            {
                // MessageBox.Show(SelectedProject.ProjectName);
                CheckCurrentProject();
            }
        }

        private void CheckCurrentProject()
        {
            try
            {
                if (CurrentWorkingProject != null)
                {
                    if (SelectedProject.ProjectName == CurrentWorkingProject.ProjectName)
                    {
                        IsSelected = true;
                        MessageBox.Show("This is your current project and the start time was " + CurrentWorkingProject.StrartDateTime);
                    }
                    else
                    {
                        var NewSelectedProject = SelectedProject;
                        var result = MessageBox.Show("You will be logged off from your current Project " + CurrentWorkingProject.ProjectName, "Task update", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        if (MessageBoxResult.OK == result)
                        {
                            DailyTaskModel dtm = new DailyTaskModel
                            {
                                DailyTaskId=CurrentWorkingProject.DailyTaskId,
                                ProjectId = CurrentWorkingProject.ProjectID,
                                UserId = UserId,
                                StartTime = CurrentWorkingProject.StrartDateTime,
                                EndTime = DateTime.Now
                                
                            };
                            var response = UpdateEndTimeForTheCurrentWorkingProject(dtm).Result;
                            if (response != null)
                            {
                                SetNewCurrentProjectAndInsertStartTime(NewSelectedProject);
                            }
                            else
                            {
                                MessageBox.Show("Some error occured..");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SetNewCurrentProjectAndInsertStartTime(Project newSelectedProject)
        {
            if (newSelectedProject != null)
            {
                CurrentWorkingProject.ProjectID = newSelectedProject.ProjectID;
                CurrentWorkingProject.ProjectName = newSelectedProject.ProjectName;
                CurrentWorkingProject.StrartDateTime = DateTime.Now;

                DailyTaskModel dt = new DailyTaskModel();
                dt.ProjectId = CurrentWorkingProject.ProjectID;
                dt.UserId = UserId;
                dt.StartTime = DateTime.Now;
                InsertProjectStartTime();
            }
            else
                MessageBox.Show("No project is selected..");
          
        }

        private async Task<DailyTaskModel> UpdateEndTimeForTheCurrentWorkingProject(DailyTaskModel dailyTaskModel)
        {
            //throw new NotImplementedException();
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApibaseAddress"])
            };

            var completeApiAddress = ConfigurationManager.AppSettings["WebApibaseAddress"] + Properties.Resources.UpdateEndTimeAPIRoutePath;

            var myContent = JsonConvert.SerializeObject(dailyTaskModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.PostAsync(completeApiAddress, byteContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var JsonString = await response.Content.ReadAsStringAsync();

                var deserialized = JsonConvert.DeserializeObject<DailyTaskModel>(JsonString);
                return deserialized;
            }
            else
                return null;
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

        private void SetDefaultProjectAsCurrentProject()
        {
            var defaultProj = Projects.FirstOrDefault(x => x.ProjectName.ToLower() == Properties.Resources.DefaultProjectName.ToLower());
            IsSelected = true;
            if (defaultProj != null)
            {
                CurrentWorkingProject = new CurrentWorkingProject
                {
                    ProjectID = defaultProj.ProjectID,
                    ProjectName = defaultProj.ProjectName,
                    StrartDateTime = DateTime.Now
                };
            }

        }

        private void InsertProjectStartTime()
        {
            if (CurrentWorkingProject == null)
                return;

            DailyTaskModel dtm = new DailyTaskModel
            {
                ProjectId = CurrentWorkingProject.ProjectID,
                UserId = UserId,
                StartTime = CurrentWorkingProject.StrartDateTime
            };

            var response = CallInserStartTimeWebApi(dtm);

            if (response != null)
            {
                CurrentWorkingProject.DailyTaskId = response.Result.DailyTaskId;
            }
        }

        public async Task<DailyTaskModel> CallInserStartTimeWebApi(DailyTaskModel dailyTaskModel)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApibaseAddress"])
            };

            var completeApiAddress = ConfigurationManager.AppSettings["WebApibaseAddress"] + Properties.Resources.InsertStartTimeAPIRoutePath;

            var myContent = JsonConvert.SerializeObject(dailyTaskModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.PostAsync(completeApiAddress, byteContent).Result;


            if (response.IsSuccessStatusCode)
            {
                var JsonString = await response.Content.ReadAsStringAsync();

                var deserialized = JsonConvert.DeserializeObject<DailyTaskModel>(JsonString);
                return deserialized;
            }
            else
                return null;

        }

        private void FindPrjectButton()
        {
            ListBox control = (ListBox)_dailyTaskView.FindName("lstProject");
           
        }
    }
}
