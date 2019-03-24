using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
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
        DispatcherTimer TimeElapsedCalculateTimer = new DispatcherTimer();

        public DailyTaskViewModel(DailyTask dailyTask, User user)
        {
            _dailyTaskView = dailyTask;

            User = user;
            CreateHeader();
            CreateListViewViewModel(user);

            SetDefaultProjectAsCurrentProject();
            InsertProjectStartTime();

            Logger.Log("DailyTaskViewModel", "Info", "Timer started.");

            ConfigureTimersForApplication();

        }

        private void ConfigureTimersForApplication()
        {
            checkMaxProjectTimeTimer.Interval = TimeSpan.FromMinutes(1);
            checkMaxProjectTimeTimer.Tick += CheckMaxProjectTimeTimer_Tick;
            checkMaxProjectTimeTimer.IsEnabled = true;
            checkMaxProjectTimeTimer.Start();

            TimeElapsedCalculateTimer.Interval = TimeSpan.FromSeconds(10);
            TimeElapsedCalculateTimer.Tick += TimeElapsedCalculateTimer_Tick;
            TimeElapsedCalculateTimer.IsEnabled = true;
            TimeElapsedCalculateTimer.Start();
        }

        private void TimeElapsedCalculateTimer_Tick(object sender, EventArgs e)
        {
            StopProjectTimeElapseShowTimer();

            DisplayTimeElapsed();

            RefreshUI();
            StartProjectTimeElapseShowTimer();
        }

        private void StartProjectTimeElapseShowTimer()
        {
            TimeElapsedCalculateTimer.IsEnabled = true;
            TimeElapsedCalculateTimer.Start();
        }

        private void StopProjectTimeElapseShowTimer()
        {
            TimeElapsedCalculateTimer.IsEnabled = false;
            TimeElapsedCalculateTimer.Stop();
        }

        private void DisplayTimeElapsed()
        {
            if (CurrentWorkingProject != null)
            {
                TimeSpan diffrenceInTime = DateTime.Now - CurrentWorkingProject.StrartDateTime;
                Project currProject = ProjectListViewViewModel.Projects.FirstOrDefault(x => x.IsCurrentProject == true);

                if (currProject != null)
                {
                    if (currProject.PreviousElapsedTime != TimeSpan.Zero)
                    {
                        currProject.TimeElapsedValue = currProject.PreviousElapsedTime.Add(diffrenceInTime).ToString(@"hh\:mm\:ss");
                    }
                    else
                    {
                        currProject.TimeElapsedValue = diffrenceInTime.ToString(@"hh\:mm\:ss");
                    }

                }

            }
        }

        private void CheckMaxProjectTimeTimer_Tick(object sender, EventArgs e)
        {

            if (checkMaxProjectTimeTimer.IsEnabled)
            {

                try
                {
                    StopProjectMaxTimeCheckingTimer();
                    if (CurrentWorkingProject != null)
                    {
                        TimeSpan diffrenceInHours = DateTime.Now - CurrentWorkingProject.StrartDateTime;

                        if (diffrenceInHours.Hours > CurrentWorkingProject?.MaxProjectTimeInHours)
                        {
                            MessageBox.Show(Application.Current.MainWindow, "Time consumtion for this project is more than maximum time.", "Project Time excced", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel);
                        }
                    }
                    StartProjectMaxTimeCheckingTimer();
                }
                catch (Exception)
                {
                    throw;
                }


            }
        }

        private void StartProjectMaxTimeCheckingTimer()
        {
            checkMaxProjectTimeTimer.IsEnabled = true;
            checkMaxProjectTimeTimer.Start();
        }

        private void StopProjectMaxTimeCheckingTimer()
        {
            checkMaxProjectTimeTimer.IsEnabled = false;
            checkMaxProjectTimeTimer.Stop();
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

                if (item.DifferenceInSecondsInCurrentDate == null)
                {
                    TimeSpan ts = TimeSpan.FromSeconds(0);
                    item.TimeElapsedValue = ts.ToString(@"hh\:mm\:ss");
                }
                else
                {
                    TimeSpan ts = TimeSpan.FromSeconds(Convert.ToDouble(item.DifferenceInSecondsInCurrentDate));
                    item.TimeElapsedValue = ts.ToString(@"hh\:mm\:ss");
                    item.PreviousElapsedTime = ts;
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
                return new CommandHandler(_ => OpenInBrowser());
            }
        }

        private void OpenInBrowser()
        {
            string userName, password;
            if (User != null)
            {
                userName = User.UserName;
                password = User.Password;
            }
            EncryptionHelper encryptionHelper = new EncryptionHelper();

            string url = ConfigurationManager.AppSettings["WebSiteBaseAddress"] + encryptionHelper.Encryptdata(User.UserName) + "&P=" + encryptionHelper.Encryptdata(User.Password);

            if (!IsValidUri(url))
                return;
            System.Diagnostics.Process.Start(url);

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

                    Logger.Log("LogoutUser", "Info", "Calling update end time API when user logout");

                    var response = WebAPIHelper.UpdateEndTimeForTheCurrentWorkingProject(dtm).Result;

                    Logger.Log("LogoutUser", "Info", "successfully called update end time API when user logout");

                    CurrentWorkingProject = null;
                    User = null;
                    //ProjectListViewViewModel = null;
                    StopProjectMaxTimeCheckingTimer();
                    StopProjectTimeElapseShowTimer();

                    Login loginView = new Login();
                    Application.Current.MainWindow = loginView;

                    loginView.Show();
                    loginView.Activate();
                    _dailyTaskView.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("LogoutUser", "Error", ex.ToString());
                throw ex;
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
                    RefreshUI();
                }
                else
                {
                    CurrentWorkingProject.EndDateTime = DateTime.Now;
                    var currProj = ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectID == CurrentWorkingProject.ProjectID);

                    // Save current time elapsed of the project.
                    if (currProj.PreviousElapsedTime == TimeSpan.Zero)
                    {
                        currProj.PreviousElapsedTime = DateTime.Now - CurrentWorkingProject.StartDateTime;
                    }
                    else
                    {
                        TimeSpan diffrenceInTime = CurrentWorkingProject.EndDateTime - CurrentWorkingProject.StrartDateTime;
                        currProj.PreviousElapsedTime = currProj.PreviousElapsedTime.Add(diffrenceInTime);
                    }
                    ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectID == CurrentWorkingProject.ProjectID).IsCurrentProject = false;

                    ////Making the selecedindex to the selected project...
                    ProjectListViewViewModel.SelectedIndex = ProjectListViewViewModel.Projects.ToList().FindIndex(x => x.ProjectID == Convert.ToInt32(ProjectID));
                    ProjectListViewViewModel.SelectedProject = ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectID == Convert.ToInt32(ProjectID));
                    RefreshUI();
                    UpdateCurrentTask();

                }
            }
            catch (Exception ex)
            {
                Logger.Log("UpdateTask", "Error", ex.ToString());
                //throw;
            }
        }

        private void RefreshUI()
        {
            CollectionViewSource.GetDefaultView(this.ProjectListViewViewModel.Projects).Refresh();
        }

        private async void UpdateCurrentTask()
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
                        EndTime = CurrentWorkingProject.EndDateTime
                    };

                    Logger.Log("UpdateCurrentTask", "Info", "Calling API to update end time for the current project .");
                    int response = 0;
                    await Task.Run(() =>
                    {
                        response = WebAPIHelper.UpdateEndTimeForTheCurrentWorkingProject(dtm).Result;

                    });

                    Logger.Log("UpdateCurrentTask", "Info", "successfully called API to update end time for the current project .");
                    if (response > 0)
                    {
                        CurrentWorkingProject = null;

                        SetNewCurrentProjectAndInsertStartTime(ProjectListViewViewModel.SelectedProject);
                    }
                    else
                    {
                        MessageBox.Show("Some error occured..");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("UpdateCurrentTask", "Error", ex.ToString());
                MessageBox.Show("Some error occured.");
                // throw ex;
            }
        }

        private void SetNewCurrentProjectAndInsertStartTime(Project selectedProject)
        {
            try
            {

                if (selectedProject != null)
                {
                    CurrentWorkingProject = new CurrentWorkingProject
                    {
                        ProjectID = selectedProject.ProjectID,
                        ProjectName = selectedProject.ProjectName,
                        StrartDateTime = DateTime.Now,
                        MaxProjectTimeInHours = selectedProject.MaxProjectTimeInHours,
                        DifferenceInSecondsInCurrentDate = selectedProject.DifferenceInSecondsInCurrentDate
                    };
                    // CurrentWorkingProject.EndDateTime =null;

                    DailyTaskModel dt = new DailyTaskModel
                    {
                        ProjectId = CurrentWorkingProject.ProjectID,
                        UserId = User.ID,
                        StartTime = DateTime.Now,
                    };


                    InsertProjectStartTime();

                    if (ProjectListViewViewModel.Projects != null)
                    {
                        var curProj = ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower());

                        if (ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower()) != null)
                        {

                            ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower()).IsCurrentProject = true;
                            // int SelectedIndex = ProjectListViewViewModel.Projects.ToList().FindIndex(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower());

                            //Removing the project to insert it at the top of the list..
                            ProjectListViewViewModel.Projects.Remove(curProj);
                            ProjectListViewViewModel.Projects.Insert(0, curProj);

                            ProjectListViewViewModel.SelectedProject = ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower());
                            ProjectListViewViewModel.SelectedIndex = ProjectListViewViewModel.Projects.ToList().FindIndex(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower());
                        }
                    }

                    RefreshUI();
                }
                else
                    MessageBox.Show("No project is selected..");
            }
            catch (Exception ex)
            {
                Logger.Log("SetNewCurrentProjectAndInsertStartTime", "Error", ex.ToString());
                throw ex;
            }

        }

        private void SetDefaultProjectAsCurrentProject()
        {
            try
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
                        MaxProjectTimeInHours = defaultProj.MaxProjectTimeInHours,
                        DifferenceInSecondsInCurrentDate = defaultProj.DifferenceInSecondsInCurrentDate != null ? defaultProj.DifferenceInSecondsInCurrentDate : 0,
                    };

                    ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == "Idle Time".ToLower()).TimeElapsedValue = TimeSpan.FromSeconds(Convert.ToDouble(CurrentWorkingProject.DifferenceInSecondsInCurrentDate)).ToString(@"hh\:mm\:ss");
                }

                if (ProjectListViewViewModel.Projects != null)
                {
                    if (ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower()) != null)
                    {
                        ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower()).IsCurrentProject = true;

                        //TODO chnge the index of the current project.
                        ProjectListViewViewModel.Projects.Remove(defaultProj);
                        ProjectListViewViewModel.Projects.Insert(0, defaultProj);

                        ProjectListViewViewModel.SelectedProject = ProjectListViewViewModel.Projects.FirstOrDefault(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower());
                        ProjectListViewViewModel.SelectedIndex = ProjectListViewViewModel.Projects.ToList().FindIndex(x => x.ProjectName.ToLower() == CurrentWorkingProject.ProjectName.ToLower());


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("SetDefaultProjectAsCurrentProject", "Error", $"{ex.ToString()}");

                throw ex;
            }




            RefreshUI();
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
            try
            {
                Logger.Log("InsertProjectStartTime", "Info", "Calling insert start time API");


                var response = WebAPIHelper.CallInserStartTimeWebApi(dtm);

                Logger.Log("InsertProjectStartTime", "Info", "successfully called insert start time API ");
                if (response != null)
                {
                    CurrentWorkingProject.DailyTaskId = Convert.ToInt32(response.Result.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("InsertProjectStartTime", "Error", ex.ToString());
                throw ex;
            }

        }


    }
}
