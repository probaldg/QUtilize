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
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class DailyTaskViewModel : ViewModelBase
    {
        DailyTask _dailyTaskView;
        public DailyTaskViewModel(DailyTask dailyTask, User user)
        {
            _dailyTaskView = dailyTask;
            Projects = new ObservableCollection<Project>();

         
            foreach (var item in user.Projects)
            {
                Projects.Add(item);
            }
            SetDefaultProjectAsCurrentProject();
            InsertProjectStartTime();

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
            get {
               
                return _selectedProject;
            }
            set { _selectedProject = value; }
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
            if (CurrentWorkingProject != null)
            {
                DailyTaskModel dtm = new DailyTaskModel
                {
                    DailyTaskId = CurrentWorkingProject.DailyTaskId,
                    ProjectId = CurrentWorkingProject.ProjectID,
                    UserId = User.ID,
                    StartTime = CurrentWorkingProject.StrartDateTime,
                    EndTime = DateTime.Now

                };
                var response = WebAPIHelper.UpdateEndTimeForTheCurrentWorkingProject(dtm).Result;
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

                if (SelectedProject.ProjectName == CurrentWorkingProject.ProjectName)
                {
                  
                    MessageBox.Show("This is your current project and the start time was " + CurrentWorkingProject.StrartDateTime);
                }
                else
                {
                    UpdateCurrentTask();
                }
            }
        }

        private void UpdateCurrentTask()
        {
            try
            {
                if (CurrentWorkingProject != null)
                {

                    var NewSelectedProject = SelectedProject;
                    var result = MessageBox.Show("You will be logged off from your current Project " + CurrentWorkingProject.ProjectName, "Task update", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    if (MessageBoxResult.OK == result)
                    {
                        DailyTaskModel dtm = new DailyTaskModel
                        {
                            DailyTaskId = CurrentWorkingProject.DailyTaskId,
                            ProjectId = CurrentWorkingProject.ProjectID,
                            UserId = User.ID,
                            StartTime = CurrentWorkingProject.StrartDateTime,
                            EndTime = DateTime.Now

                        };
                        var response =WebAPIHelper.UpdateEndTimeForTheCurrentWorkingProject(dtm).Result;
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
                dt.UserId = User.ID;
                dt.StartTime = DateTime.Now;
                InsertProjectStartTime();
            }
            else
                MessageBox.Show("No project is selected..");

        }
        
        private void SetDefaultProjectAsCurrentProject()
        {

            var defaultProj = Projects.FirstOrDefault(x => x.ProjectName.ToLower() == "Idle Time".ToLower());
            
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
                UserId = User.ID,
                StartTime = CurrentWorkingProject.StrartDateTime
            };

            var response =WebAPIHelper.CallInserStartTimeWebApi(dtm);

            if (response != null)
            {
                CurrentWorkingProject.DailyTaskId = response.Result.DailyTaskId;
            }
        }

       

      
    }
}
