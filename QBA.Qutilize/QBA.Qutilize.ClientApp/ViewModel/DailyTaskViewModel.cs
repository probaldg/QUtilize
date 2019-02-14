using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class DailyTaskViewModel : ViewModelBase
    {
        DailyTask _dailyTaskView;

        DispatcherTimer checkMaxProjectTimeTimer = new DispatcherTimer();
       
        public DailyTaskViewModel(DailyTask dailyTask, User user)
        {
            _dailyTaskView = dailyTask;

            checkMaxProjectTimeTimer.Interval = TimeSpan.FromMinutes(1);
            checkMaxProjectTimeTimer.Tick += CheckMaxProjectTimeTimer_Tick;

            User = user;
            CreateHeader();
            CreateListViewViewModel(user);
            SetDefaultProjectAsCurrentProject();
            InsertProjectStartTime();

            checkMaxProjectTimeTimer.IsEnabled = true;
            checkMaxProjectTimeTimer.Start();
        }

        private void CheckMaxProjectTimeTimer_Tick(object sender, EventArgs e)
        {
           
            if(checkMaxProjectTimeTimer.IsEnabled)
            {
                checkMaxProjectTimeTimer.IsEnabled = false;
                checkMaxProjectTimeTimer.Stop();
                try
                {
                    if (CurrentWorkingProject != null)
                    {
                        TimeSpan diffrenceInHours = DateTime.Now - CurrentWorkingProject.StrartDateTime;

                        if (diffrenceInHours.Hours >= CurrentWorkingProject?.MaxProjectTimeInHours)
                        {
                            MessageBox.Show("Time consumtion for this project  is more than maximum time.");
                        }
                    }
                    checkMaxProjectTimeTimer.IsEnabled = true;
                    checkMaxProjectTimeTimer.Start();
                }
                catch (Exception)
                {

                    throw;
                }

               
            }
        }

        private void CreateHeader()
        {
            CurrDate = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            CurrUser = "Welcome, " + User.Name.Substring(0, User.Name.IndexOf(' '));
        }

        private void CreateListViewViewModel(User user)
        {
            ProjectListViewViewModel projectListViewViewModel = new ProjectListViewViewModel();
            foreach (var item in user.Projects)
            {
                if (item.Description == "" || item.Description == null)
                {
                    item.Description = "Description not available";
                }
                projectListViewViewModel.Projects.Add(item);
            }
            ProjectListViewViewModel = projectListViewViewModel;
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

        private ProjectListViewViewModel _projectListViewViewModel;

        public ProjectListViewViewModel ProjectListViewViewModel
        {
            get { return _projectListViewViewModel; }
            set
            {
                _projectListViewViewModel = value;
                OnPropertyChanged("ProjectListViewViewModel");

            }
        }

        private Project _selectedProject;

        public Project SelectedProject
        {
            get
            {

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
        public string CurrDate { get; set; }
        public string CurrUser { get; set; }
        public ICommand UpdateCommandFromSelectedProject
        {
            get
            {
                return new CommandHandler((ProjectID) => UpdateTask(ProjectID));
            }
        }

        public ICommand Logout
        {
            get
            {
                return new CommandHandler(_ => LogoutUser());
            }
        }

        public ICommand OpenURLCommand
        {
            get
            {
                return new CommandHandler(_ => OpenBrowser());
            }
        }

        private void OpenBrowser()
        {
            // MessageBox.Show("Command clicked");
            if (!IsValidUri("https://www.google.com/"))
                return;
            System.Diagnostics.Process.Start("https://www.google.com/");
            // return true;
        }

        public static bool IsValidUri(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                return false;
            Uri tmp;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out tmp))
                return false;
            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps;
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

        private void UpdateTask(object ProjectID)
        {
            try
            {
                if (ProjectID == null)
                {
                    throw new ArgumentNullException(nameof(ProjectID));
                }
                if (CurrentWorkingProject.ProjectID == (int)ProjectID)
                {
                    ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectID == CurrentWorkingProject.ProjectID).IsCurrentProject = true;
                    CollectionViewSource.GetDefaultView(this.ProjectListViewViewModel.Projects).Refresh();
                }
                else
                {
                    ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectID == CurrentWorkingProject.ProjectID).IsCurrentProject = false;

                    ////Making the selecedindex to the selected project...
                    ProjectListViewViewModel.SelectedIndex = ProjectListViewViewModel.Projects.ToList()
                        .FindIndex(x => x.ProjectID == Convert.ToInt32(ProjectID));

                    CollectionViewSource.GetDefaultView(this.ProjectListViewViewModel.Projects).Refresh();
                    UpdateCurrentTask();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void UpdateCurrentTask()
        {
            try
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
                    if (response > 0)
                    {
                        SetNewCurrentProjectAndInsertStartTime(SelectedProject);
                    }
                    else
                    {
                        MessageBox.Show("Some error occured..");
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SetNewCurrentProjectAndInsertStartTime(Project selectedProject)
        {
            if (selectedProject != null)
            {
                CurrentWorkingProject.ProjectID = selectedProject.ProjectID;
                CurrentWorkingProject.ProjectName = selectedProject.ProjectName;
                CurrentWorkingProject.StrartDateTime = DateTime.Now;

                DailyTaskModel dt = new DailyTaskModel
                {
                    ProjectId = CurrentWorkingProject.ProjectID,
                    UserId = User.ID,
                    StartTime = DateTime.Now,
                };
                InsertProjectStartTime();
            }
            else
                MessageBox.Show("No project is selected..");

        }

        private void SetDefaultProjectAsCurrentProject()
        {

            var defaultProj = ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == "Idle Time".ToLower());

            if (defaultProj != null)
            {
                CurrentWorkingProject = new CurrentWorkingProject
                {
                    ProjectID = defaultProj.ProjectID,
                    ProjectName = defaultProj.ProjectName,
                    StrartDateTime = DateTime.Now,
                    IsCurrentProject = true,
                    MaxProjectTimeInHours=defaultProj.MaxProjectTimeInHours
                };
            }

            ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower()).IsCurrentProject = true;
            ProjectListViewViewModel.SelectedIndex = ProjectListViewViewModel.Projects.ToList().FindIndex(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower());
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

            var response = WebAPIHelper.CallInserStartTimeWebApi(dtm);

            if (response != null)
            {
                CurrentWorkingProject.DailyTaskId = Convert.ToInt32(response.Result.Value);
            }
        }

    }
}
