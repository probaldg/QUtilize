using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.ClientApp.Helper.ViewHelper;
using QBA.Qutilize.ClientApp.Helper.WPFControlHelper;
using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class DTViewModel : ViewModelBase
    {
        string[] ColorArray = new string[] { "#262626", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7", "#194C66", "#1F5D7C", "#246E93", "#2A7FAA", "#3090C0", "#3E9ECE", "#55AAD4", "#6BB5DA", "#82C0DF" };

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

        NewDailyTask _dailyTaskView;
        DispatcherTimer checkMaxProjectTimeTimer = new DispatcherTimer();
        DispatcherTimer TimeElapsedCalculateTimer = new DispatcherTimer();

        private Project _selectedProject;

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged("SelectedProject");
            }
        }

        private ProjectTask _selectedTask;

        public ProjectTask SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                OnPropertyChanged("SelectedTask");
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
        private void ConfigureTimersForApplication()
        {
            checkMaxProjectTimeTimer.Interval = TimeSpan.FromMinutes(1);
            checkMaxProjectTimeTimer.Tick += CheckMaxProjectTimeTimer_Tick;
            checkMaxProjectTimeTimer.IsEnabled = true;
            checkMaxProjectTimeTimer.Start();

            TimeElapsedCalculateTimer.Interval = TimeSpan.FromSeconds(5);
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
        private void StopProjectTimeElapseShowTimer()
        {
            TimeElapsedCalculateTimer.IsEnabled = false;
            TimeElapsedCalculateTimer.Stop();
        }
        private void StartProjectTimeElapseShowTimer()
        {
            TimeElapsedCalculateTimer.IsEnabled = true;
            TimeElapsedCalculateTimer.Start();
        }


        private void CheckMaxProjectTimeTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private List<Project> _projectList;

        public List<Project> ProjectList
        {
            get { return _projectList; }
            set { _projectList = value; }
        }

        public DTViewModel(NewDailyTask dailyTask, User user)
        {
            _dailyTaskView = dailyTask;
            this.User = user;
            ProjectList = new List<Project>();
            ProjectList = User.Projects.ToList();

            // ConfigureTimersForApplication();

        }


        public void LoadAllProjects()
        {
            try
            {

                List<Project> projects = new List<Project>();
                var grid = (Grid)_dailyTaskView.FindName("grdProject");
                grid.Children.Clear();

                StackPanel stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(0);

                if (ProjectList.Count > 0)
                {

                    projects = ProjectList;

                    var defaulProjectID = Convert.ToInt32(ConfigurationManager.AppSettings["defaultProjectID"].ToString());
                    Project defaultProject;
                    defaultProject = projects.FirstOrDefault(x => x.ProjectID == defaulProjectID);
                    if (defaultProject == null)
                    {
                        defaultProject = projects.First();
                    }
                    SelectedProject = defaultProject;

                    if (SelectedProject != null)
                    {
                        projects.Remove(defaultProject);
                        projects.Insert(0, defaultProject);
                    }

                    CurrentWorkingProject = new CurrentWorkingProject
                    {
                        ProjectID = SelectedProject.ProjectID,
                        ProjectName = SelectedProject.ProjectName,
                        StartDateTime = DateTime.Now,
                        IsCurrentProject = true,
                        MaxProjectTimeInHours = SelectedProject.MaxProjectTimeInHours,
                        DifferenceInSecondsInCurrentDate = SelectedProject.DifferenceInSecondsInCurrentDate != null ? SelectedProject.DifferenceInSecondsInCurrentDate : 0,
                    };


                    //int rowCounter = 0;
                    foreach (var item in projects)
                    {
                        Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
                        Canvas canvas;
                        var Border = new Border();
                        var backColor = ColorArray[0].ToString();

                        if (item.ProjectID == CurrentWorkingProject.ProjectID)
                        {

                            canvas = projectViewHelper.CreateProjectViewControlForSelectedProject(item, backColor);
                            Border = BorderControlHelper.CreateBorderForSelectedControl();
                            Border.Child = canvas;

                            //TODO need to insert default project start time in database and set selected project.
                            SelectedProject = item;
                            InsertProjectStartTime();
                        }
                        else
                        {
                            //var backColor = ColorArray[0].ToString();

                            canvas = projectViewHelper.CreateProjectViewControl(item, backColor);
                            Border = BorderControlHelper.CreateBorder();
                            Border.Child = canvas;

                        }
                        //canvas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorArray[0].ToString()));

                        canvas.MouseLeftButtonDown += ProjectClickHandler;
                        canvas.MouseEnter += ProjectMouseEnterHanlder;
                        canvas.MouseLeave += ProjectMouseLeaveHanlder;

                        stackPanel.Children.Add(Border);
                        // rowCounter++;
                    }
                    grid.Children.Add(stackPanel);
                }

                ProjectList = projects;
                //ConfigureTimersForApplication();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //private void DisplayTimeElapsed()
        //{

        //    if (CurrentWorkingProject != null)
        //    {
        //        List<Project> projects = new List<Project>();
        //        projects = ProjectList;

        //        Project currProject = projects.FirstOrDefault(x => x.ProjectID == CurrentWorkingProject.ProjectID);
        //        var grid = (Grid)_dailyTaskView.FindName("grdProject");


        //        StackPanel stackPanel = new StackPanel();
        //        stackPanel.Margin = new Thickness(0);

        //        if (currProject != null && CurrentWorkingProject.ProjectTaskID == null)
        //        {
        //            grid.Children.Clear();

        //            foreach (var item in projects)
        //            {
        //                Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
        //                Canvas canvas;
        //                var Border = new Border();
        //                var backColor = ColorArray[0].ToString();

        //                if (item.ProjectID == currProject.ProjectID)
        //                {
        //                    if (item.Tasks.Count == 0)
        //                    {
        //                        TimeSpan diffrenceInTime = DateTime.Now - CurrentWorkingProject.StartDateTime;

        //                        if (item.PreviousElapsedTime != TimeSpan.Zero)
        //                        {
        //                            item.TimeElapsedValue = currProject.PreviousElapsedTime.Add(diffrenceInTime).ToString(@"hh\:mm\:ss");
        //                        }
        //                        else
        //                        {
        //                            item.TimeElapsedValue = diffrenceInTime.ToString(@"hh\:mm\:ss");
        //                            item.PreviousElapsedTime = diffrenceInTime;
        //                        }

        //                    }

        //                    canvas = projectViewHelper.CreateProjectViewControlForSelectedProject(item, backColor);
        //                    Border = BorderControlHelper.CreateBorderForSelectedControl();
        //                    Border.Child = canvas;

        //                }
        //                else
        //                {
        //                    canvas = projectViewHelper.CreateProjectViewControl(item, backColor);
        //                    Border = BorderControlHelper.CreateBorder();
        //                    Border.Child = canvas;

        //                }

        //                canvas.MouseLeftButtonDown += ProjectClickHandler;
        //                canvas.MouseEnter += ProjectMouseEnterHanlder;
        //                canvas.MouseLeave += ProjectMouseLeaveHanlder;

        //                stackPanel.Children.Add(Border);

        //            }
        //            grid.Children.Add(stackPanel);


        //            ProjectList = projects;
        //        }

        //    }

        //}


        private void DisplayTimeElapsed()
        {

            //if (CurrentWorkingProject != null)
            //{
            //    List<Project> projects = new List<Project>();
            //    projects = ProjectList;

            //    Project currProject = projects.FirstOrDefault(x => x.ProjectID == CurrentWorkingProject.ProjectID);
            //    var grid = (Grid)_dailyTaskView.FindName("grdProject");


            //    StackPanel stackPanelMain = new StackPanel();
            //    stackPanelMain.Margin = new Thickness(0);

            //    if (currProject != null)
            //    {
            //        grid.Children.Clear();

            //        foreach (var item in projects)
            //        {
            //            Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
            //            Canvas canvas;
            //            var Border = new Border();
            //            var backColor = ColorArray[0].ToString();

            //            if (item.ProjectID == currProject.ProjectID)
            //            {
            //                if (item.Tasks.Count == 0)
            //                {
            //                    TimeSpan diffrenceInTime = DateTime.Now - CurrentWorkingProject.StartDateTime;

            //                    if (item.PreviousElapsedTime != TimeSpan.Zero)
            //                    {
            //                        item.TimeElapsedValue = currProject.PreviousElapsedTime.Add(diffrenceInTime).ToString(@"hh\:mm\:ss");
            //                    }
            //                    else
            //                    {
            //                        item.TimeElapsedValue = diffrenceInTime.ToString(@"hh\:mm\:ss");
            //                        item.PreviousElapsedTime = diffrenceInTime;
            //                    }

            //                    canvas = projectViewHelper.CreateProjectViewControlForSelectedProject(item, backColor);
            //                    Border = BorderControlHelper.CreateBorderForSelectedControl();
            //                    Border.Child = canvas;
            //                    canvas.MouseLeftButtonDown += ProjectClickHandler;
            //                    canvas.MouseEnter += ProjectMouseEnterHanlder;
            //                    canvas.MouseLeave += ProjectMouseLeaveHanlder;
            //                }
            //                else
            //                {
            //                    ProjectTask selectedTask = item.Tasks.FirstOrDefault(x => x.TaskId == CurrentWorkingProject.ProjectTaskID);
            //                    var tasks = item.Tasks;
            //                    StackPanel stackPanelForParentTask = new StackPanel();

            //                    TaskViewHelper taskViewHelper = new TaskViewHelper();
            //                    var projectHeading = taskViewHelper.CreateProjectHeadingControl(item);
            //                    stackPanelForParentTask.Children.Add(projectHeading);

            //                    if (selectedTask != null)
            //                    {

            //                        foreach (ProjectTask pTask in tasks)
            //                        {
            //                            if (pTask.ParentTaskId == 0)
            //                            {
            //                                //var CurrentTaskID = pTask.TaskId;
            //                                ProjectTask IsContainTask = IsTaskContainSelectedTaskID(selectedTask.TaskId, pTask);
            //                                StackPanel stackPanelForSubTask = new StackPanel();
            //                                if (IsContainTask != null)
            //                                {



            //                                    if (pTask.SubTaskCount > 0)
            //                                    {
            //                                        var taskHeading = GetTaskAndParenOfTheTask(pTask);
            //                                        stackPanelForParentTask.Children.Add(taskHeading);
            //                                        //extract the subtask
            //                                        var subTaskList = User.Tasks.Where(x => x.ParentTaskId == pTask.TaskId).ToList();
            //                                        foreach (ProjectTask subTask in subTaskList)
            //                                        {
            //                                            var taskbackColor = ColorArray[subTask.TaskDepthLevel + 1].ToString();
            //                                            if (selectedTask.TaskId == subTask.TaskId)
            //                                            {
            //                                                TimeSpan diffrenceInTime = DateTime.Now - CurrentWorkingProject.StartDateTime;
            //                                                if (subTask.PreviousElapsedTime != TimeSpan.Zero)
            //                                                {
            //                                                    subTask.TimeElapsedValue = currProject.PreviousElapsedTime.Add(diffrenceInTime).ToString(@"hh\:mm\:ss");
            //                                                }
            //                                                else
            //                                                {
            //                                                    subTask.TimeElapsedValue = diffrenceInTime.ToString(@"hh\:mm\:ss");
            //                                                    subTask.PreviousElapsedTime = diffrenceInTime;
            //                                                }

            //                                                var subSelectedTaskCanvas = taskViewHelper.CreateTaskViewControlForSelectedTask(subTask, taskbackColor);
            //                                                var TaskBorder = BorderControlHelper.CreateBorderForTask();
            //                                                TaskBorder.Child = subSelectedTaskCanvas;
            //                                                subSelectedTaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
            //                                                stackPanelForSubTask.Children.Add(TaskBorder);

            //                                            }
            //                                            else
            //                                            {
            //                                                TimeSpan diffrenceInTime = DateTime.Now - CurrentWorkingProject.StartDateTime;
            //                                                if (subTask.PreviousElapsedTime != TimeSpan.Zero)
            //                                                {
            //                                                    subTask.TimeElapsedValue = currProject.PreviousElapsedTime.Add(diffrenceInTime).ToString(@"hh\:mm\:ss");
            //                                                }
            //                                                else
            //                                                {
            //                                                    subTask.TimeElapsedValue = diffrenceInTime.ToString(@"hh\:mm\:ss");
            //                                                    subTask.PreviousElapsedTime = diffrenceInTime;
            //                                                }
            //                                                var subTaskCanvas = taskViewHelper.CreateTaskViewControl(subTask, backColor);
            //                                                var TaskBorder = BorderControlHelper.CreateBorderForTask();
            //                                                TaskBorder.Child = subTaskCanvas;
            //                                                subTaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
            //                                                stackPanelForSubTask.Children.Add(TaskBorder);
            //                                            }
            //                                        }
            //                                        stackPanelForParentTask.Children.Add(stackPanelForSubTask);
            //                                    }
            //                                    else
            //                                    {
            //                                        if (selectedTask.TaskId == pTask.TaskId)
            //                                        {
            //                                            TimeSpan diffrenceInTime = DateTime.Now - CurrentWorkingProject.StartDateTime;
            //                                            if (pTask.PreviousElapsedTime != TimeSpan.Zero)
            //                                            {
            //                                                pTask.TimeElapsedValue = currProject.PreviousElapsedTime.Add(diffrenceInTime).ToString(@"hh\:mm\:ss");
            //                                            }
            //                                            else
            //                                            {
            //                                                pTask.TimeElapsedValue = diffrenceInTime.ToString(@"hh\:mm\:ss");
            //                                                pTask.PreviousElapsedTime = diffrenceInTime;
            //                                            }
            //                                            var taskbackColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();
            //                                            var subTaskCanvas = taskViewHelper.CreateTaskViewControlForSelectedTask(pTask, taskbackColor);
            //                                            var subTaskBorder = BorderControlHelper.CreateBorderForTask();
            //                                            subTaskBorder.Child = subTaskCanvas;
            //                                            subTaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
            //                                            stackPanelForSubTask.Children.Add(subTaskBorder);
            //                                            stackPanelForParentTask.Children.Add(stackPanelForSubTask);
            //                                        }
            //                                        else
            //                                        {
            //                                            var taskbackColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();
            //                                            var subTaskCanvas = taskViewHelper.CreateTaskViewControl(pTask, taskbackColor);
            //                                            var subTaskBorder = BorderControlHelper.CreateBorderForTask();
            //                                            subTaskBorder.Child = subTaskCanvas;
            //                                            subTaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
            //                                            stackPanelForSubTask.Children.Add(subTaskBorder);
            //                                            stackPanelForParentTask.Children.Add(stackPanelForSubTask);
            //                                        }


            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    Canvas subTaskCanvas;
            //                                    var taskbackColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();
            //                                    if (selectedTask.TaskId == pTask.TaskId)
            //                                    {
            //                                        TimeSpan diffrenceInTime = DateTime.Now - CurrentWorkingProject.StartDateTime;
            //                                        if (pTask.PreviousElapsedTime != TimeSpan.Zero)
            //                                        {
            //                                            pTask.TimeElapsedValue = currProject.PreviousElapsedTime.Add(diffrenceInTime).ToString(@"hh\:mm\:ss");
            //                                        }
            //                                        else
            //                                        {
            //                                            pTask.TimeElapsedValue = diffrenceInTime.ToString(@"hh\:mm\:ss");
            //                                            pTask.PreviousElapsedTime = diffrenceInTime;
            //                                        }
            //                                        subTaskCanvas = taskViewHelper.CreateTaskViewControlForSelectedTask(pTask, taskbackColor);

            //                                    }
            //                                    else
            //                                    {
            //                                        subTaskCanvas = taskViewHelper.CreateTaskViewControl(pTask, taskbackColor);
            //                                    }
            //                                    var subTaskBorder = BorderControlHelper.CreateBorderForTask();
            //                                    subTaskBorder.Child = subTaskCanvas;
            //                                    subTaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
            //                                    stackPanelForSubTask.Children.Add(subTaskBorder);
            //                                    stackPanelForParentTask.Children.Add(stackPanelForSubTask);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    Border = BorderControlHelper.CreateBorderForSelectedControl();
            //                    Border.Child = stackPanelForParentTask;

            //                }

            //            }
            //            else
            //            {
            //                canvas = projectViewHelper.CreateProjectViewControl(item, backColor);
            //                Border = BorderControlHelper.CreateBorder();
            //                Border.Child = canvas;

            //                canvas.MouseLeftButtonDown += ProjectClickHandler;
            //                canvas.MouseEnter += ProjectMouseEnterHanlder;
            //                canvas.MouseLeave += ProjectMouseLeaveHanlder;
            //            }



            //            stackPanelMain.Children.Add(Border);

            //        }
            //        grid.Children.Add(stackPanelMain);


            //        ProjectList = projects;
            //    }

            //}

        }

        private void ProjectMouseLeaveHanlder(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void ProjectMouseEnterHanlder(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ProjectClickHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StopProjectTimeElapseShowTimer();
            Canvas canvas = (Canvas)sender;
            try
            {
                var typeOfData = canvas.DataContext.GetType().Name;

                if (typeOfData.ToString().ToLower() == "project".ToLower())
                {
                    var project = (Project)canvas.DataContext;
                    SelectedProject = project;

                    var selectedproject = ProjectList.FirstOrDefault(x => x.ProjectID == SelectedProject.ProjectID);
                    if (selectedproject != null)
                    {
                        ProjectList.Remove(selectedproject);
                        ProjectList.Insert(0, selectedproject);
                    }

                    //int rowCounter = 0;
                    var grid = (Grid)_dailyTaskView.FindName("grdProject");
                    grid.Children.Clear();

                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Margin = new Thickness(0);

                    foreach (Project item in ProjectList)
                    {
                        if (item.ProjectID == SelectedProject.ProjectID)
                        {
                            //var selectedProjectCanvas = CanvasControlHelper.CreateCanvas();
                            //StackPanel stackPanelWithTasks = new StackPanel();

                            //TaskViewHelper taskViewHelper = new TaskViewHelper();
                            //var projectHeading = taskViewHelper.CreateProjectHeadingControl(item);
                            //stackPanelWithTasks.Children.Add(projectHeading);

                            //TODO 1)Check for the subTaskCount.

                            if (item.TaskCount > 0)
                            {
                                var selectedProjectCanvas = CanvasControlHelper.CreateCanvas();
                                StackPanel stackPanelWithTasks = new StackPanel();

                                TaskViewHelper taskViewHelper = new TaskViewHelper();
                                var projectHeading = taskViewHelper.CreateProjectHeadingControl(item);
                                stackPanelWithTasks.Children.Add(projectHeading);
                                var tasks = item.Tasks;
                                Canvas taskCanvas = new Canvas();
                                foreach (ProjectTask task in tasks)
                                {
                                    if (task.ParentTaskId == 0)
                                    {
                                        var backColor = ColorArray[task.TaskDepthLevel + 1].ToString();
                                        taskCanvas = taskViewHelper.CreateTaskViewControl(task, backColor);
                                        var TaskBorder = BorderControlHelper.CreateBorderForTask();
                                        TaskBorder.Child = taskCanvas;

                                        taskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
                                        stackPanelWithTasks.Children.Add(TaskBorder);
                                    }

                                }
                                var Border = BorderControlHelper.CreateBorderForSelectedControl();

                                Border.Child = stackPanelWithTasks;
                                stackPanel.Children.Add(Border);
                                StopProjectTimeElapseShowTimer();
                            }
                            else
                            {
                                //Update the end time for the current working project.
                                //Update the previous elaspsed time of the project which are previosly selected selected project. project
                                //Insert end time in database.
                                // Set new selectedProject = clickedProject
                                //Set new current working project and insert the start time datainto database.

                                //Insert the selected Project details in database in dailytask table.


                                if (item.ProjectID != CurrentWorkingProject.ProjectID)
                                {
                                    UpdateProjectElapsedTime();

                                    SelectedTask = null;

                                    int NewDailyTaskID = SaveProjectDailyTaskInDB(item);
                                    if (NewDailyTaskID > 0)
                                    {
                                        SelectedProject = item;

                                        CurrentWorkingProject = new CurrentWorkingProject
                                        {
                                            ProjectID = SelectedProject.ProjectID,
                                            ProjectName = SelectedProject.ProjectName,
                                            StartDateTime = DateTime.Now,
                                            IsCurrentProject = true,
                                            DailyTaskId = NewDailyTaskID,
                                            MaxProjectTimeInHours = SelectedProject.MaxProjectTimeInHours,
                                            DifferenceInSecondsInCurrentDate = SelectedProject.DifferenceInSecondsInCurrentDate != null ? SelectedProject.DifferenceInSecondsInCurrentDate : 0,
                                        };

                                        Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
                                        Canvas canvasProject;
                                        var Border = new Border();
                                        var backColor = ColorArray[0].ToString();
                                        if (item.ProjectID == CurrentWorkingProject.ProjectID)
                                        {

                                            canvasProject = projectViewHelper.CreateProjectViewControlForSelectedProject(item, backColor);
                                            Border = BorderControlHelper.CreateBorderForSelectedControl();
                                            Border.Child = canvasProject;

                                        }
                                        stackPanel.Children.Add(Border);
                                        StartProjectTimeElapseShowTimer();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Some error occured");
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("This project is already selected.");
                                }

                            }
                        }
                        else
                        {
                            Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
                            Canvas projectCanvas;

                            var backColor = ColorArray[0].ToString();
                            var Border = BorderControlHelper.CreateBorder();
                            projectCanvas = projectViewHelper.CreateProjectViewControl(item, backColor);

                            Border.Child = projectCanvas;
                            stackPanel.Children.Add(Border);

                            projectCanvas.MouseLeftButtonDown += ProjectClickHandler;
                            projectCanvas.MouseEnter += ProjectMouseEnterHanlder;
                            projectCanvas.MouseLeave += ProjectMouseLeaveHanlder;

                        }
                    }
                    grid.Children.Add(stackPanel);

                }
                SetScrollBarToTop();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error occured." + ex.ToString());

            }
        }

        private void UpdateProjectElapsedTime()
        {
            CurrentWorkingProject.EndDateTime = DateTime.Now;

            var previosSelectedProject = ProjectList.FirstOrDefault(x => x.ProjectID == CurrentWorkingProject.ProjectID);

            if (previosSelectedProject.PreviousElapsedTime == TimeSpan.Zero)
            {
                previosSelectedProject.PreviousElapsedTime = DateTime.Now - CurrentWorkingProject.StartDateTime;
                previosSelectedProject.TimeElapsedValue = TimeSpan.FromSeconds(Convert.ToDouble(previosSelectedProject.PreviousElapsedTime.TotalSeconds)).ToString(@"hh\:mm\:ss");
            }
            else
            {
                TimeSpan diffrenceInTime = CurrentWorkingProject.EndDateTime - CurrentWorkingProject.StartDateTime;
                previosSelectedProject.PreviousElapsedTime = previosSelectedProject.PreviousElapsedTime.Add(diffrenceInTime);
                previosSelectedProject.TimeElapsedValue = TimeSpan.FromSeconds(Convert.ToDouble(previosSelectedProject.PreviousElapsedTime.TotalSeconds)).ToString(@"hh\:mm\:ss");
            }
        }

        private void UpdatePreviousProjectEndTime(Project item)
        {
            //CurrentWorkingProject.EndDateTime = DateTime.Now;

            //var previosSelectedProject = ProjectList.FirstOrDefault(x => x.ProjectID == CurrentWorkingProject.ProjectID);

            //if (previosSelectedProject.PreviousElapsedTime == TimeSpan.Zero)
            //{
            //    previosSelectedProject.PreviousElapsedTime = DateTime.Now - CurrentWorkingProject.StartDateTime;
            //    previosSelectedProject.TimeElapsedValue = TimeSpan.FromSeconds(Convert.ToDouble(previosSelectedProject.PreviousElapsedTime)).ToString(@"hh\:mm\:ss");
            //}
            //else
            //{
            //    TimeSpan diffrenceInTime = CurrentWorkingProject.EndDateTime - CurrentWorkingProject.StartDateTime;
            //    previosSelectedProject.PreviousElapsedTime = previosSelectedProject.PreviousElapsedTime.Add(diffrenceInTime);
            //    previosSelectedProject.TimeElapsedValue = TimeSpan.FromSeconds(Convert.ToDouble(previosSelectedProject.PreviousElapsedTime.TotalSeconds)).ToString(@"hh\:mm\:ss");
            //}
            //SaveProjectDailyTaskInDB(item);
        }

        private void SetScrollBarToTop()
        {
            var scrollBar = (ScrollViewer)_dailyTaskView.FindName("MainScrollBar");
            scrollBar.ScrollToTop();
        }

        private void TaskClickEventHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StopProjectTimeElapseShowTimer();
            try
            {
                Canvas canvas = (Canvas)sender;
                var typeOfData = canvas.DataContext.GetType().Name;

                if (typeOfData.ToString().ToLower() == "projecttask".ToLower())
                {
                    var selectedTask = (ProjectTask)canvas.DataContext;
                    var grid = (Grid)_dailyTaskView.FindName("grdProject");
                    grid.Children.Clear();

                    StackPanel stackPanelForAllProjects = new StackPanel();
                    stackPanelForAllProjects.Margin = new Thickness(0);


                    LoopThrougAllProjects(selectedTask, stackPanelForAllProjects);

                    grid.Children.Add(stackPanelForAllProjects);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error occurred." + ex.ToString());

            }
            //StartProjectTimeElapseShowTimer();
        }





        //private void LoopThrougAllProjects(ProjectTask selectedTask, StackPanel stackPanelForAllProjects)
        //{
        //    try
        //    {


        //        foreach (Project project in ProjectList)
        //        {
        //            if (project.ProjectID == selectedTask.ProjectID)
        //            {
        //                SelectedProject = project;

        //                StackPanel stackPanelForParentTask = new StackPanel();

        //                TaskViewHelper taskViewHelper = new TaskViewHelper();
        //                var projectHeading = taskViewHelper.CreateProjectHeadingControl(project);
        //                stackPanelForParentTask.Children.Add(projectHeading);

        //                var tasks = project.Tasks;

        //                Canvas taskCanvas = new Canvas();

        //                foreach (ProjectTask pTask in tasks)
        //                {
        //                    if (pTask.ParentTaskId == 0)
        //                    {
        //                        // var CurrentTaskID = pTask.TaskId;
        //                        ProjectTask IsContainTask = IsTaskContainSelectedTaskID(selectedTask.TaskId, pTask);
        //                        if (IsContainTask != null)
        //                        {
        //                            StackPanel stackPanelForSubTask = new StackPanel();
        //                            if (pTask.SubTaskCount > 0)
        //                            {
        //                                //var taskHeading = GetTaskAndParenOfTheTask(pTask);
        //                                var taskHeading = GetTaskAndParenOfTheTask(IsContainTask);
        //                                stackPanelForParentTask.Children.Add(taskHeading);

        //                                //extract the subtask
        //                                var subTaskList = User.Tasks.Where(x => x.ParentTaskId == pTask.TaskId).ToList();
        //                                StackPanel stackPanelForNestedSubTask = new StackPanel();
        //                                foreach (ProjectTask subTask in subTaskList)
        //                                {
        //                                    var backColor = ColorArray[subTask.TaskDepthLevel + 1].ToString();

        //                                    if (selectedTask.TaskId == subTask.TaskId)
        //                                    {
        //                                        var subSelectedTaskCanvas = taskViewHelper.CreateTaskViewControlForSelectedTask(subTask, backColor);
        //                                        var TaskBorder = BorderControlHelper.CreateBorderForTask();
        //                                        TaskBorder.Child = subSelectedTaskCanvas;
        //                                        subSelectedTaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
        //                                        stackPanelForNestedSubTask.Children.Add(TaskBorder);
        //                                        stackPanelForSubTask.Children.Add(stackPanelForNestedSubTask);

        //                                        if (subTask.TaskId != CurrentWorkingProject.ProjectTaskID)
        //                                        {
        //                                            SelectedTask = subTask;

        //                                            CurrentWorkingProject.EndDateTime = DateTime.Now;
        //                                            SetPreviousTaskElapsedTimeValue();

        //                                            int newDailyTaskID = UpdatedProjectTask();
        //                                            if (newDailyTaskID > 0)
        //                                            {
        //                                                CurrentWorkingProject = new CurrentWorkingProject
        //                                                {
        //                                                    ProjectID = SelectedProject.ProjectID,
        //                                                    ProjectName = SelectedProject.ProjectName,
        //                                                    StartDateTime = DateTime.Now,
        //                                                    IsCurrentProject = true,
        //                                                    DailyTaskId = newDailyTaskID,
        //                                                    MaxProjectTimeInHours = SelectedProject.MaxProjectTimeInHours,
        //                                                    DifferenceInSecondsInCurrentDate = SelectedProject.DifferenceInSecondsInCurrentDate != null ? SelectedProject.DifferenceInSecondsInCurrentDate : 0,
        //                                                    ProjectTaskID = selectedTask.TaskId,
        //                                                };

        //                                                StartProjectTimeElapseShowTimer();
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            MessageBox.Show("This task is already selected.");
        //                                        }

        //                                    }
        //                                    else
        //                                    {
        //                                        var subTaskCanvas = taskViewHelper.CreateTaskViewControl(subTask, backColor);
        //                                        var TaskBorder = BorderControlHelper.CreateBorderForTask();
        //                                        TaskBorder.Child = subTaskCanvas;
        //                                        subTaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
        //                                        stackPanelForSubTask.Children.Add(TaskBorder);
        //                                    }

        //                                }

        //                                stackPanelForParentTask.Children.Add(stackPanelForSubTask);
        //                            }
        //                            else
        //                            {
        //                                var backColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();
        //                                var subTaskCanvas = taskViewHelper.CreateTaskViewControlForSelectedTask(pTask, backColor);
        //                                var subTaskBorder = BorderControlHelper.CreateBorderForTask();
        //                                subTaskBorder.Child = subTaskCanvas;
        //                                subTaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
        //                                stackPanelForSubTask.Children.Add(subTaskBorder);
        //                                stackPanelForParentTask.Children.Add(stackPanelForSubTask);

        //                                if (pTask.TaskId != CurrentWorkingProject.ProjectTaskID)
        //                                {
        //                                    SelectedTask = pTask;
        //                                    CurrentWorkingProject.EndDateTime = DateTime.Now;
        //                                    SetPreviousTaskElapsedTimeValue();

        //                                    int newDailyTaskID = UpdatedProjectTask();
        //                                    if (newDailyTaskID > 0)
        //                                    {
        //                                        CurrentWorkingProject = new CurrentWorkingProject
        //                                        {
        //                                            ProjectID = SelectedProject.ProjectID,
        //                                            ProjectName = SelectedProject.ProjectName,
        //                                            StartDateTime = DateTime.Now,
        //                                            IsCurrentProject = true,
        //                                            DailyTaskId = newDailyTaskID,
        //                                            MaxProjectTimeInHours = SelectedProject.MaxProjectTimeInHours,
        //                                            DifferenceInSecondsInCurrentDate = SelectedProject.DifferenceInSecondsInCurrentDate != null ? SelectedProject.DifferenceInSecondsInCurrentDate : 0,
        //                                            ProjectTaskID = selectedTask.TaskId,
        //                                        };

        //                                        StartProjectTimeElapseShowTimer();
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    MessageBox.Show("This task is already selected.");
        //                                }


        //                                // MessageBox.Show("task clicked");


        //                            }
        //                        }
        //                        else
        //                        {
        //                            var backColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();
        //                            taskCanvas = taskViewHelper.CreateTaskViewControl(pTask, backColor);
        //                            var TaskBorder = BorderControlHelper.CreateBorderForTask();
        //                            TaskBorder.Child = taskCanvas;
        //                            taskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
        //                            stackPanelForParentTask.Children.Add(TaskBorder);
        //                        }
        //                    }

        //                }


        //                var Border = BorderControlHelper.CreateBorderForSelectedControl();
        //                Border.Child = stackPanelForParentTask;

        //                stackPanelForAllProjects.Children.Add(Border);
        //            }
        //            else
        //            {
        //                Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
        //                Canvas projectCanvas;

        //                string backcolor = ColorArray[0].ToString();
        //                projectCanvas = projectViewHelper.CreateProjectViewControl(project, backcolor);
        //                projectCanvas.MouseLeftButtonDown += ProjectClickHandler;
        //                projectCanvas.MouseEnter += ProjectMouseEnterHanlder;
        //                projectCanvas.MouseLeave += ProjectMouseLeaveHanlder;

        //                var Border = BorderControlHelper.CreateBorder();
        //                Border.Child = projectCanvas;
        //                stackPanelForAllProjects.Children.Add(Border);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        private void LoopThrougAllProjects(ProjectTask selectedTask, StackPanel stackPanelForAllProjects)
        {
            try
            {


                foreach (Project project in ProjectList)
                {
                    if (project.ProjectID == selectedTask.ProjectID)
                    {
                        SelectedProject = project;

                        StackPanel stackPanelForParentTask = new StackPanel();

                        TaskViewHelper taskViewHelper = new TaskViewHelper();
                        var projectHeading = taskViewHelper.CreateProjectHeadingControl(project);
                        stackPanelForParentTask.Children.Add(projectHeading);

                        var tasks = project.Tasks;

                        Canvas taskCanvas = new Canvas();

                        foreach (ProjectTask pTask in tasks)
                        {
                            if (pTask.ParentTaskId == 0)
                            {
                                // var CurrentTaskID = pTask.TaskId;
                                ProjectTask IsContainTask = IsTaskContainSelectedTaskID(selectedTask.TaskId, pTask);
                                if (IsContainTask != null)
                                {
                                    var taskHeading = GetParenControlOfTheTask(IsContainTask);
                                    if (taskHeading != null)
                                    {
                                        stackPanelForParentTask.Children.Add(taskHeading);
                                    }


                                    if (IsContainTask.SubTaskCount == 0)
                                    {
                                        var backColor = ColorArray[IsContainTask.TaskDepthLevel + 1].ToString();
                                        taskCanvas = taskViewHelper.CreateTaskViewControlForSelectedTask(IsContainTask, backColor);
                                        var TaskBorder = BorderControlHelper.CreateBorderForTask();
                                        TaskBorder.Child = taskCanvas;
                                        taskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
                                        stackPanelForParentTask.Children.Add(TaskBorder);

                                        if (pTask.TaskId != CurrentWorkingProject.ProjectTaskID && selectedTask.TaskId == pTask.TaskId)
                                        {
                                            SelectedTask = pTask;
                                            CurrentWorkingProject.EndDateTime = DateTime.Now;
                                            SetPreviousTaskElapsedTimeValue();
                                            int newDailyTaskID = UpdatedProjectTask();

                                            if (newDailyTaskID > 0)
                                            {
                                                CurrentWorkingProject = new CurrentWorkingProject
                                                {
                                                    ProjectID = SelectedProject.ProjectID,
                                                    ProjectName = SelectedProject.ProjectName,
                                                    StartDateTime = DateTime.Now,
                                                    IsCurrentProject = true,
                                                    DailyTaskId = newDailyTaskID,
                                                    MaxProjectTimeInHours = SelectedProject.MaxProjectTimeInHours,
                                                    DifferenceInSecondsInCurrentDate = SelectedProject.DifferenceInSecondsInCurrentDate != null ? SelectedProject.DifferenceInSecondsInCurrentDate : 0,
                                                    ProjectTaskID = selectedTask.TaskId,
                                                };

                                                StartProjectTimeElapseShowTimer();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var taskbackColor = ColorArray[IsContainTask.TaskDepthLevel + 1].ToString();
                                        taskCanvas = taskViewHelper.CreateTaskHeadingControl(IsContainTask, taskbackColor);
                                        var TaskBorder = BorderControlHelper.CreateBorderForTask();
                                        TaskBorder.Child = taskCanvas;
                                        stackPanelForParentTask.Children.Add(TaskBorder);

                                        var nestedSubTaskList = User.Tasks.Where(x => x.ParentTaskId == IsContainTask.TaskId).ToList();

                                        StackPanel nestedSubTaskStackPanel = new StackPanel();
                                        foreach (ProjectTask nestedProjectTask in nestedSubTaskList)
                                        {
                                            var backColor = ColorArray[nestedProjectTask.TaskDepthLevel + 1].ToString();

                                            if (nestedProjectTask.TaskId == selectedTask.TaskId)
                                            {
                                                var nestedtaskCanvas = taskViewHelper.CreateTaskViewControlForSelectedTask(nestedProjectTask, backColor);
                                                var nestedTaskBorder = BorderControlHelper.CreateBorderForTask();
                                                nestedTaskBorder.Child = nestedtaskCanvas;
                                                nestedtaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;

                                                if (nestedProjectTask.TaskId != CurrentWorkingProject.ProjectTaskID && selectedTask.TaskId == nestedProjectTask.TaskId)
                                                {
                                                    {
                                                        SelectedTask = nestedProjectTask;
                                                        CurrentWorkingProject.EndDateTime = DateTime.Now;
                                                        SetPreviousTaskElapsedTimeValue();
                                                        int newDailyTaskID = UpdatedProjectTask();

                                                        if (newDailyTaskID > 0)
                                                        {
                                                            CurrentWorkingProject = new CurrentWorkingProject
                                                            {
                                                                ProjectID = SelectedProject.ProjectID,
                                                                ProjectName = SelectedProject.ProjectName,
                                                                StartDateTime = DateTime.Now,
                                                                IsCurrentProject = true,
                                                                DailyTaskId = newDailyTaskID,
                                                                MaxProjectTimeInHours = SelectedProject.MaxProjectTimeInHours,
                                                                DifferenceInSecondsInCurrentDate = SelectedProject.DifferenceInSecondsInCurrentDate != null ? SelectedProject.DifferenceInSecondsInCurrentDate : 0,
                                                                ProjectTaskID = selectedTask.TaskId,
                                                            };

                                                            StartProjectTimeElapseShowTimer();
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                var nestedtaskCanvas = taskViewHelper.CreateTaskViewControl(nestedProjectTask, backColor);
                                                var nestedTaskBorder = BorderControlHelper.CreateBorderForTask();
                                                nestedTaskBorder.Child = nestedtaskCanvas;
                                                nestedtaskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
                                                nestedSubTaskStackPanel.Children.Add(nestedTaskBorder);
                                            }

                                        }
                                        stackPanelForParentTask.Children.Add(nestedSubTaskStackPanel);
                                    }
                                }
                                else
                                {
                                    var backColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();
                                    taskCanvas = taskViewHelper.CreateTaskViewControl(pTask, backColor);
                                    var TaskBorder = BorderControlHelper.CreateBorderForTask();
                                    TaskBorder.Child = taskCanvas;
                                    taskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
                                    stackPanelForParentTask.Children.Add(TaskBorder);
                                }
                            }

                        }


                        var Border = BorderControlHelper.CreateBorderForSelectedControl();
                        Border.Child = stackPanelForParentTask;

                        stackPanelForAllProjects.Children.Add(Border);
                    }
                    else
                    {
                        Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
                        Canvas projectCanvas;

                        string backcolor = ColorArray[0].ToString();
                        projectCanvas = projectViewHelper.CreateProjectViewControl(project, backcolor);
                        projectCanvas.MouseLeftButtonDown += ProjectClickHandler;
                        projectCanvas.MouseEnter += ProjectMouseEnterHanlder;
                        projectCanvas.MouseLeave += ProjectMouseLeaveHanlder;

                        var Border = BorderControlHelper.CreateBorder();
                        Border.Child = projectCanvas;
                        stackPanelForAllProjects.Children.Add(Border);

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private int UpdatedProjectTask()
        {

            return SaveProjectDailyTaskInDB(SelectedProject);
        }

        private void SetPreviousTaskElapsedTimeValue()
        {
            var previosSelectedProject = ProjectList.FirstOrDefault(x => x.ProjectID == CurrentWorkingProject.ProjectID);
            if (previosSelectedProject != null)
            {
                if (previosSelectedProject.TaskCount == 0)
                {
                    if (previosSelectedProject.PreviousElapsedTime == TimeSpan.Zero)
                    {
                        previosSelectedProject.PreviousElapsedTime = DateTime.Now - CurrentWorkingProject.StartDateTime;
                        previosSelectedProject.TimeElapsedValue = TimeSpan.FromSeconds(Convert.ToDouble(previosSelectedProject.PreviousElapsedTime.TotalSeconds)).ToString(@"hh\:mm\:ss");
                    }
                    else
                    {
                        TimeSpan diffrenceInTime = CurrentWorkingProject.EndDateTime - CurrentWorkingProject.StartDateTime;
                        previosSelectedProject.PreviousElapsedTime = previosSelectedProject.PreviousElapsedTime.Add(diffrenceInTime);
                        previosSelectedProject.TimeElapsedValue = TimeSpan.FromSeconds(Convert.ToDouble(previosSelectedProject.PreviousElapsedTime.TotalSeconds)).ToString(@"hh\:mm\:ss");
                    }

                }
                else
                {
                    var previousTask = previosSelectedProject.Tasks.FirstOrDefault(x => x.TaskId == CurrentWorkingProject.ProjectTaskID);
                    if (previousTask != null)
                    {
                        if (previousTask.PreviousElapsedTime == TimeSpan.Zero)
                        {
                            previousTask.PreviousElapsedTime = DateTime.Now - CurrentWorkingProject.StartDateTime;
                            previousTask.TimeElapsedValue = TimeSpan.FromSeconds(Convert.ToDouble(previousTask.PreviousElapsedTime.TotalSeconds)).ToString(@"hh\:mm\:ss");
                        }
                        else
                        {
                            TimeSpan diffrence = CurrentWorkingProject.EndDateTime - CurrentWorkingProject.StartDateTime;
                            previousTask.PreviousElapsedTime = previousTask.PreviousElapsedTime.Add(diffrence);
                            previousTask.TimeElapsedValue = TimeSpan.FromSeconds(Convert.ToDouble(previousTask.PreviousElapsedTime.TotalSeconds)).ToString(@"hh\:mm\:ss");
                        }
                    }
                }

            }
        }

        private ProjectTask IsTaskContainSelectedTaskID(int selectedTaskID, ProjectTask projectTask)
        {
            ProjectTask result = null;
            try
            {
                if (selectedTaskID == projectTask.TaskId)
                {
                    result = projectTask;
                }
                else
                {
                    if (projectTask.SubTaskCount > 0)
                    {
                        var subTaskList = User.Tasks.Where(x => x.ParentTaskId == projectTask.TaskId).ToList();
                        if (subTaskList.Count > 0)
                        {
                            foreach (ProjectTask subTask in subTaskList)
                            {
                                var nestedTask = IsTaskContainSelectedTaskID(selectedTaskID, subTask);
                                if (nestedTask != null)
                                {
                                    result = nestedTask;
                                }
                                else
                                {
                                    result = null;
                                }
                            }
                        }
                        else
                        {
                            result = null;
                        }
                    }
                    else
                    {
                        result = null;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }


        //private StackPanel GetParenOfTheTask(ProjectTask projectTask)
        //{
        //    StackPanel stackPanel = new StackPanel();
        //    if (projectTask.TaskDepthLevel == 0)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        List<ProjectTask> parentList = new List<ProjectTask>();
        //        for (int i = projectTask.TaskDepthLevel; i >= 0; i--)
        //        {
        //            var parentTask = User.Tasks.FirstOrDefault(x => x.TaskId == projectTask.ParentTaskId);

        //            TaskViewHelper taskViewHelper = new TaskViewHelper();
        //            if (parentTask != null)
        //            {
        //                //parentList.Add(parentTask);
        //                var backColor = ColorArray[parentTask.TaskDepthLevel + 1].ToString();
        //                var canvas = taskViewHelper.CreateTaskViewControl(parentTask, backColor);
        //                stackPanel.Children.Add(canvas);
        //            }
        //            projectTask = parentTask;
        //        }



        //        //TODO need to revers the order of the stackpanel
        //    }

        //    return stackPanel;
        //}
        private StackPanel GetParenControlOfTheTask(ProjectTask projectTask)
        {
            StackPanel stackPanel = new StackPanel();
            if (projectTask.TaskDepthLevel == 0)
            {
                return null;
            }
            else
            {
                List<ProjectTask> parentList = new List<ProjectTask>();

                parentList = GetParentTask(projectTask);
                parentList.Reverse();

                //for (int i = projectTask.TaskDepthLevel; i >= 0; i--)
                //{
                //    var parentTask = User.Tasks.FirstOrDefault(x => x.TaskId == projectTask.ParentTaskId);

                //    TaskViewHelper taskViewHelper = new TaskViewHelper();
                //    if (parentTask != null)
                //    {
                //        //parentList.Add(parentTask);
                //        var backColor = ColorArray[parentTask.TaskDepthLevel + 1].ToString();
                //        var canvas = taskViewHelper.CreateTaskViewControl(parentTask, backColor);
                //        stackPanel.Children.Add(canvas);
                //    }
                //    projectTask = parentTask;
                //}
                foreach (var item in parentList)
                {
                    TaskViewHelper taskViewHelper = new TaskViewHelper();
                    var backColor = ColorArray[item.TaskDepthLevel + 1].ToString();
                    var canvas = taskViewHelper.CreateTaskViewControl(item, backColor);
                    stackPanel.Children.Add(canvas);
                }


                //TODO need to revers the order of the stackpanel
            }

            return stackPanel;
        }
        private List<ProjectTask> GetParentTask(ProjectTask task)
        {
            ProjectTask parentTask = null;
            List<ProjectTask> parentList = new List<ProjectTask>();
            try
            {
                if (task != null)
                {
                    parentTask = User.Tasks.FirstOrDefault(x => x.TaskId == task.ParentTaskId);
                    if (parentTask != null)
                    {
                        parentList.Add(parentTask);

                        if (parentTask.SubTaskCount != 0)
                        {
                            var result = GetParentTask(parentTask);
                            if (result != null)
                            {
                                parentList.AddRange(result);
                            }
                        }
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }

            return parentList;
        }
        //private StackPanel GetTaskAndParenOfTheTask(ProjectTask projectTask)
        //{
        //    if (projectTask == null)
        //    {
        //        throw new ArgumentNullException(nameof(projectTask));
        //    }

        //    StackPanel stackPanel = new StackPanel();

        //    try
        //    {
        //        if (projectTask.TaskDepthLevel == 0)
        //        {

        //            var backColor = ColorArray[projectTask.TaskDepthLevel + 1].ToString();
        //            TaskViewHelper taskViewHelper = new TaskViewHelper();

        //            var canvas = taskViewHelper.CreateTaskHeadingControl(projectTask, backColor);
        //            stackPanel.Children.Add(canvas);
        //        }
        //        else
        //        {
        //            for (int i = projectTask.TaskDepthLevel; i >= 0; i--)
        //            {
        //                var parentTask = User.Tasks.FirstOrDefault(x => x.TaskId == projectTask.ParentTaskId);

        //                TaskViewHelper taskViewHelper = new TaskViewHelper();
        //                if (parentTask != null)
        //                {
        //                    var backColor = ColorArray[parentTask.TaskDepthLevel + 1].ToString();
        //                    var canvas = taskViewHelper.CreateTaskHeadingControl(parentTask, backColor);
        //                    stackPanel.Children.Add(canvas);
        //                    projectTask = parentTask;
        //                    //if (projectTask != null)
        //                    //{
        //                    //    stackPanel.Children.Add(GetTaskAndParenOfTheTask(projectTask));
        //                    //}
        //                }


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return stackPanel;
        //}

        private void RefreshUI()
        {
            //CollectionViewSource.GetDefaultView(this._dailyTaskView).Refresh();

        }

        private void InsertProjectStartTime()
        {
            if (CurrentWorkingProject == null)
                return;
            string conStr = ConfigurationManager.ConnectionStrings["QBADBConnetion"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(conStr);


            DailyTaskModel dtm = new DailyTaskModel
            {
                ProjectId = CurrentWorkingProject.ProjectID,
                UserId = User.ID,
                StartTime = CurrentWorkingProject.StartDateTime,

            };
            try
            {
                Logger.Log("InsertProjectStartTime", "Info", "Calling insert start time API");

                DataTable dt = new DataTable();


                var sqlCmd = new SqlCommand("USP_SaveDailyTask", sqlCon)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCmd.Parameters.AddWithValue("@UserID", dtm.UserId);
                sqlCmd.Parameters.AddWithValue("@ProjectId", dtm.ProjectId);
                if (dtm.ProjectTaskID != null)
                {
                    sqlCmd.Parameters.AddWithValue("@ProjectTaskID", dtm.ProjectTaskID);
                }

                sqlCmd.Parameters.AddWithValue("@TaskStart_EndTime", dtm.StartTime);
                sqlCmd.Parameters.AddWithValue("@Createdby", dtm.UserId.ToString());
                sqlCmd.Parameters.AddWithValue("@IsActive", true);
                sqlCon.Open();

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                da.Fill(dt);

                sqlCon.Close();
                if (dt.Rows.Count > 0)
                {
                    CurrentWorkingProject.DailyTaskId = Convert.ToInt32(dt.Rows[0]["DailyTaskId"]);
                }
                else
                {
                    CurrentWorkingProject.DailyTaskId = 0;
                }



            }
            catch (Exception ex)
            {
                Logger.Log("InsertProjectStartTime", "Error", ex.ToString());
                sqlCon.Close();
                throw ex;


            }

        }

        //private int SaveProjectDailyTaskInDB(Project project)
        //{

        //    string conStr = ConfigurationManager.ConnectionStrings["QBADBConnetion"].ConnectionString;
        //    SqlConnection sqlCon = new SqlConnection(conStr);

        //    int previousTaskID = CurrentWorkingProject.DailyTaskId;


        //    try
        //    {
        //        DailyTaskModel dtm = new DailyTaskModel
        //        {
        //            ProjectId = project.ProjectID,
        //            UserId = User.ID,
        //            EndTime = DateTime.Now

        //        };
        //        DataTable dt = new DataTable();

        //        var sqlCmd = new SqlCommand("USP_SaveDailyTask", sqlCon)
        //        {
        //            CommandType = System.Data.CommandType.StoredProcedure
        //        };

        //        sqlCmd.Parameters.AddWithValue("@PrevDailyTaskID", previousTaskID);
        //        sqlCmd.Parameters.AddWithValue("@TaskStart_EndTime", dtm.EndTime);
        //        sqlCmd.Parameters.AddWithValue("@UserID", dtm.UserId);
        //        sqlCmd.Parameters.AddWithValue("@ProjectId", dtm.ProjectId);
        //        if (dtm.ProjectTaskID != null)
        //        {
        //            sqlCmd.Parameters.AddWithValue("@ProjectTaskID", dtm.ProjectTaskID);
        //        }


        //        sqlCmd.Parameters.AddWithValue("@Createdby", dtm.UserId.ToString());
        //        sqlCmd.Parameters.AddWithValue("@IsActive", true);
        //        sqlCon.Open();

        //        SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
        //        da.Fill(dt);

        //        sqlCon.Close();

        //        if (dt.Rows.Count > 0)
        //        {
        //            return Convert.ToInt32(dt.Rows[0]["DailyTaskId"]);
        //        }
        //        else
        //        {
        //            return 0;
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log("SaveProjectDailyTaskInDB", "Error", ex.ToString());
        //        sqlCon.Close();
        //        throw ex;


        //    }
        //}

        private int SaveProjectDailyTaskInDB(Project project)
        {

            string conStr = ConfigurationManager.ConnectionStrings["QBADBConnetion"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(conStr);

            int previousTaskID = CurrentWorkingProject.DailyTaskId;

            try
            {
                DailyTaskModel dtm = new DailyTaskModel
                {
                    ProjectId = project.ProjectID,
                    UserId = User.ID,
                    EndTime = DateTime.Now,
                    ProjectTaskID = (SelectedTask != null) ? SelectedTask.TaskId : (int?)null,
                };



                DataTable dt = new DataTable();

                var sqlCmd = new SqlCommand("USP_SaveDailyTask", sqlCon)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                sqlCmd.Parameters.AddWithValue("@PrevDailyTaskID", previousTaskID);
                sqlCmd.Parameters.AddWithValue("@TaskStart_EndTime", dtm.EndTime);
                sqlCmd.Parameters.AddWithValue("@UserID", dtm.UserId);
                sqlCmd.Parameters.AddWithValue("@ProjectId", dtm.ProjectId);
                if (dtm.ProjectTaskID != null)
                {
                    sqlCmd.Parameters.AddWithValue("@ProjectTaskID", dtm.ProjectTaskID);
                }


                sqlCmd.Parameters.AddWithValue("@Createdby", dtm.UserId.ToString());
                sqlCmd.Parameters.AddWithValue("@IsActive", true);
                sqlCon.Open();

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                da.Fill(dt);

                sqlCon.Close();

                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["DailyTaskId"]);
                }
                else
                {
                    return 0;
                }



            }
            catch (Exception ex)
            {
                Logger.Log("SaveProjectDailyTaskInDB", "Error", ex.ToString());
                sqlCon.Close();
                throw ex;


            }
        }
    }
}
