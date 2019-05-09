using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.ClientApp.Helper.ViewHelper;
using QBA.Qutilize.ClientApp.Helper.WPFControlHelper;
using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class DTViewModel : ViewModelBase
    {
        string[] ColorArray = new string[] { "#262626", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7", "#194C66", "#1F5D7C", "#246E93", "#2A7FAA", "#3090C0", "#3E9ECE", "#55AAD4", "#6BB5DA", "#82C0DF" };

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

            // user.Projects
        }

        public void LoadAllProjects(NewDailyTask view, User user)
        {
            try
            {


                if (view == null)
                {
                    throw new ArgumentNullException(nameof(view));
                }

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                var grid = (Grid)view.FindName("grdProject");
                grid.Children.Clear();

                StackPanel stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(0);

                if (user.Projects.Count > 0)
                {
                    List<Project> projects = new List<Project>();
                    projects = user.Projects.ToList();

                    var defaulProjectID = Convert.ToInt32(ConfigurationManager.AppSettings["defaultProjectID"].ToString());

                    var defaultProject = projects.FirstOrDefault(x => x.ProjectID == defaulProjectID);
                    SelectedProject = defaultProject;
                    if (defaultProject != null)
                    {
                        projects.Remove(defaultProject);
                        projects.Insert(0, defaultProject);
                    }

                    int rowCounter = 0;
                    foreach (var item in projects)
                    {
                        Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
                        Canvas canvas;
                        var Border = new Border();

                        if (rowCounter == 0)
                        {
                            canvas = projectViewHelper.CreateProjectViewControlForSelectedProject(item);
                            Border = BorderControlHelper.CreateBorderForSelectedControl();
                        }
                        else
                        {
                            canvas = projectViewHelper.CreateProjectViewControlForSelectedProject(item);
                            Border = BorderControlHelper.CreateBorder();
                        }

                        canvas.MouseLeftButtonDown += ProjectClickHandler;
                        canvas.MouseEnter += ProjectMouseEnterHanlder;
                        canvas.MouseLeave += ProjectMouseLeaveHanlder;

                        Border.Child = canvas;
                        stackPanel.Children.Add(Border);
                        rowCounter++;
                    }
                    grid.Children.Add(stackPanel);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //private Canvas CreateProjectCanvas(Project item, Helper.ViewHelper.ProjectViewHelper projectViewHelper)
        //{
        //    var canvas = new Canvas();
        //    canvas = projectViewHelper.CreateProjectViewControl(item);

        //canvas.MouseLeftButtonDown += ProjectClickHandler;
        //    canvas.MouseEnter += ProjectMouseEnterHanlder;
        //    canvas.MouseLeave += ProjectMouseLeaveHanlder;
        //    return canvas;
        //}

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
                            var selectedProjectCanvas = CanvasControlHelper.CreateCanvas();
                            StackPanel stackPanelWithTasks = new StackPanel();

                            TaskViewHelper taskViewHelper = new TaskViewHelper();
                            var projectHeading = taskViewHelper.CreateProjectHeadingControl(item);
                            stackPanelWithTasks.Children.Add(projectHeading);

                            //TODO 1)Check for the subTaskCount.

                            if (item.TaskCount > 0)
                            {
                                //Get the task List by projectId with parentTaskID is zero.
                                // Loop through taskList
                                // Check subTaskCount.
                                //If Zero Create TaskDetailsTemplate.
                                //if not zero create ProjetTaskTemplate
                                //MessageBox.Show(item.TaskCount.ToString());

                                //item.Tasks

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
                                var Border = BorderControlHelper.CreateBorder();
                                // projectCanvas = projectViewHelper.CreateProjectViewControl(item);

                                Border.Child = stackPanelWithTasks;
                                stackPanel.Children.Add(Border);

                            }
                            else
                            {
                                //Insert the selected Project details in database in dailytask table.
                                //Create selected project Panel
                                MessageBox.Show(item.TaskCount.ToString());

                            }
                        }
                        else
                        {
                            Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
                            Canvas projectCanvas;


                            var Border = BorderControlHelper.CreateBorder();
                            projectCanvas = projectViewHelper.CreateProjectViewControl(item);

                            Border.Child = projectCanvas;
                            stackPanel.Children.Add(Border);

                            //stackPanel.Children.Add(Border);
                            projectCanvas.MouseLeftButtonDown += ProjectClickHandler;
                            projectCanvas.MouseEnter += ProjectMouseEnterHanlder;
                            projectCanvas.MouseLeave += ProjectMouseLeaveHanlder;

                        }
                    }
                    grid.Children.Add(stackPanel);

                }
                // for bring the scrollbar to the top of the screen..
                //Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(delegate
                //{
                //    var Grid = (Grid)_dailyTaskView.FindName("grdProject");

                //    //listBox.ScrollIntoView(listBox.Items[0]);
                //    Grid.s

                //}));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error occured.");

            }
        }

        private void TaskClickEventHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
            Canvas canvas = (Canvas)sender;
            var typeOfData = canvas.DataContext.GetType().Name;

            if (typeOfData.ToString().ToLower() == "projecttask".ToLower())
            {
                var selectedTask = (ProjectTask)canvas.DataContext;
                var grid = (Grid)_dailyTaskView.FindName("grdProject");
                grid.Children.Clear();

                StackPanel stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(0);

                LoopThrougAllProjects(selectedTask, stackPanel);

                grid.Children.Add(stackPanel);

            }
        }

        private void LoopThrougAllProjects(ProjectTask selectedTask, StackPanel stackPanel)
        {
            foreach (Project project in ProjectList)
            {
                if (project.ProjectID == selectedTask.ProjectID)
                {

                    var selectedProjectCanvas = CanvasControlHelper.CreateCanvas();
                    StackPanel stackPanelWithTasks = new StackPanel();

                    TaskViewHelper taskViewHelper = new TaskViewHelper();
                    var projectHeading = taskViewHelper.CreateProjectHeadingControl(project);

                    stackPanelWithTasks.Children.Add(projectHeading);
                    stackPanelWithTasks.Height = projectHeading.Height;

                    var tasks = project.Tasks;
                    Canvas taskCanvas = new Canvas();
                    //Looping through all the tasks
                    foreach (ProjectTask pTask in tasks)
                    {
                        var CurrentTaskID = pTask.TaskId;

                        //Checking if selectedtaskID is contained by task or any sub task
                        ProjectTask IsContainTask = IsTaskContainSelectedTaskID(selectedTask.TaskId, pTask);
                        if (IsContainTask != null)
                        {
                            var backColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();
                            // taskCanvas = taskViewHelper.CreateTaskViewControl(pTask, backColor);
                            var parentCanvas = GetParenOfTheTask(pTask);
                            if (parentCanvas != null)
                            {
                                stackPanelWithTasks.Children.Add(parentCanvas);
                                stackPanelWithTasks.Height += projectHeading.Height;

                            }

                            //ToDo Add current task item into the stackpanel
                            var currentTaskCanvas = taskViewHelper.CreateTaskViewControl(pTask, backColor);
                            stackPanelWithTasks.Children.Add(currentTaskCanvas);
                            stackPanelWithTasks.Height += currentTaskCanvas.Height;


                            if (pTask.SubTaskCount > 0)
                            {
                                //Get the subTask list loop throuhg the subtask and create the panel for all the subtask.

                                var subTaskList = User.Tasks.Where(x => x.ParentTaskId == pTask.TaskId).ToList();
                                if (subTaskList.Count > 0)
                                {
                                    foreach (var pt in subTaskList)
                                    {
                                        var bColor = ColorArray[pt.TaskDepthLevel + 1].ToString();
                                        taskCanvas = taskViewHelper.CreateTaskViewControl(pTask, bColor);
                                        var TaskBorder = BorderControlHelper.CreateBorderForTask();
                                        TaskBorder.Child = taskCanvas;

                                        taskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
                                        stackPanelWithTasks.Children.Add(TaskBorder);
                                        stackPanelWithTasks.Height += taskCanvas.Height;

                                    }


                                    //Add Stackpanel to the main stackpanel
                                    //selectedProjectCanvas.Children.Add(stackPanelWithTasks);
                                    //selectedProjectCanvas.Height = stackPanelWithTasks.Height;
                                    //var selectedSubTaskBorder = BorderControlHelper.CreateBorderForSelectedControl();
                                    //selectedSubTaskBorder.Child = selectedProjectCanvas;
                                    //stackPanel.Children.Add(selectedSubTaskBorder);
                                }
                                else
                                {
                                    var bColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();
                                    taskCanvas = taskViewHelper.CreateTaskViewControl(pTask, bColor);
                                    var TaskBorder = BorderControlHelper.CreateBorderForTask();
                                    TaskBorder.Child = taskCanvas;

                                    taskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
                                    stackPanelWithTasks.Children.Add(TaskBorder);
                                    stackPanelWithTasks.Height += taskCanvas.Height;

                                }

                            }
                            else
                            {
                                //Add subtask details panel
                                var bColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();

                                taskCanvas = taskViewHelper.CreateTaskViewControl(pTask, bColor);
                                var TaskBorder = BorderControlHelper.CreateBorderForTask();
                                TaskBorder.Child = taskCanvas;
                                taskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
                                stackPanelWithTasks.Children.Add(TaskBorder);
                            }
                        }
                        else
                        {

                            var backColor = ColorArray[pTask.TaskDepthLevel + 1].ToString();
                            taskCanvas = taskViewHelper.CreateTaskViewControl(pTask, backColor);
                            var TaskBorder = BorderControlHelper.CreateBorderForTask();
                            TaskBorder.Child = taskCanvas;

                            taskCanvas.MouseLeftButtonDown += TaskClickEventHandler;
                            stackPanelWithTasks.Children.Add(TaskBorder);
                        }


                    }
                    selectedProjectCanvas.Children.Add(stackPanelWithTasks);
                    selectedProjectCanvas.Height = stackPanelWithTasks.Height;
                    var selectedTaskBorder = BorderControlHelper.CreateBorderForSelectedControl();
                    selectedTaskBorder.Child = selectedProjectCanvas;
                    stackPanel.Children.Add(selectedTaskBorder);
                }
                else
                {
                    Helper.ViewHelper.ProjectViewHelper projectViewHelper = new Helper.ViewHelper.ProjectViewHelper();
                    Canvas projectCanvas;


                    projectCanvas = projectViewHelper.CreateProjectViewControl(project);
                    projectCanvas.MouseLeftButtonDown += ProjectClickHandler;
                    projectCanvas.MouseEnter += ProjectMouseEnterHanlder;
                    projectCanvas.MouseLeave += ProjectMouseLeaveHanlder;

                    var Border = BorderControlHelper.CreateBorder();
                    Border.Child = projectCanvas;
                    stackPanel.Children.Add(Border);

                    //stackPanel.Children.Add(Border);

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
                    //resultreturn projectTask;
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


        private StackPanel GetParenOfTheTask(ProjectTask projectTask)
        {
            StackPanel stackPanel = new StackPanel();

            if (projectTask.TaskDepthLevel == 0)
            {
                return null;
            }
            else
            {
                for (int i = projectTask.TaskDepthLevel; i >= 0; i--)
                {
                    var parentTask = User.Tasks.FirstOrDefault(x => x.TaskId == projectTask.ParentTaskId);

                    var backColor = ColorArray[parentTask.TaskDepthLevel + 1].ToString();
                    TaskViewHelper taskViewHelper = new TaskViewHelper();

                    if (parentTask != null)
                    {
                        var canvas = taskViewHelper.CreateTaskViewControl(parentTask, backColor);
                        stackPanel.Children.Add(canvas);

                    }
                    projectTask = parentTask;
                }
            }
            return stackPanel;
        }
    }
}
