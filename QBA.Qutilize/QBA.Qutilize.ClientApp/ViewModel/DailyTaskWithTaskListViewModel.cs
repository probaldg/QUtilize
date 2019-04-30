using QBA.Qutilize.ClientApp.Helper;
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
using System.Windows.Input;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class DailyTaskWithTaskListViewModel : ViewModelBase
    {
        //public void CreateProjectListControls(NewDailyTask view, User user)
        //{
        //    if (user == null)
        //    {
        //        throw new System.ArgumentNullException(nameof(user));
        //    }

        //    var grid = (Grid)view.FindName("grdProject");
        //    if (user.Projects.Count > 0)
        //    {
        //        RowDefinition newRow;
        //        //ColumnDefinition newCol;

        //        int rowIndex = 0;
        //        foreach (var item in user.Projects)
        //        {
        //            //var myBorder = new Border();

        //            //add a new row to the grid
        //            newRow = new RowDefinition
        //            {
        //                MinHeight = 30,
        //                Height = new GridLength(0, GridUnitType.Auto),

        //            };

        //            //newRow.Height = new GridLength(40);
        //            //newCol.Width = new GridLength(0, GridUnitType.Star);

        //            grid.RowDefinitions.Add(newRow);
        //            // grid.ColumnDefinitions.Add(newCol);

        //            Canvas myCanvas = CreateCanvas(item);


        //            Grid.SetRow(myCanvas, rowIndex);
        //            //Grid.SetRow(myBorder, rowIndex);
        //            Grid.SetColumn(myCanvas, 0);

        //            grid.Children.Add(myCanvas);

        //            rowIndex++;
        //        }
        //    }


        //}
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
        List<ProjectTask> taskList = new List<ProjectTask>();
        public DailyTaskWithTaskListViewModel(NewDailyTask dailyTask, User user)
        {
            _dailyTaskView = dailyTask;
            this.User = user;
        }

        public void CreateProjectListControls(NewDailyTask view, User user)
        {
            if (user == null)
            {
                throw new System.ArgumentNullException(nameof(user));
            }

            var grid = (Grid)view.FindName("grdProject");
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(5, 5, 5, 10);

            if (user.Projects.Count > 0)
            {

                List<Project> projects = new List<Project>();
                projects = user.Projects.ToList();

                var defaulProjectID = Convert.ToInt32(ConfigurationManager.AppSettings["defaultProjectID"].ToString());

                var defaultProject = projects.FirstOrDefault(x => x.ProjectID == defaulProjectID);
                if (defaultProject != null)
                {
                    projects.Remove(defaultProject);
                    projects.Insert(0, defaultProject);
                }

                int rowCounter = 0;
                foreach (var item in projects)
                {
                    Border border = new Border();
                    if (rowCounter == 0)
                    {
                        SelectedProjectViewHelper selectedProjectView = new SelectedProjectViewHelper();
                        border = selectedProjectView.CreateTemplateForProjectAtLastLevel(item);
                    }
                    else
                    {
                        border = CreateControlForProjectCanvas(item);
                    }
                    rowCounter++;

                    stackPanel.Children.Add(border);

                }
                grid.Children.Add(stackPanel);
            }


        }

        private Border CreateControlForProjectCanvas(Project item)
        {
            Canvas myCanvas = CreateCanvasPanel(item);
            myCanvas.Width = 291;

            myCanvas.MouseLeftButtonDown += ShowProject_Task;
            myCanvas.MouseEnter += MyCanvas_MouseEnter;
            myCanvas.MouseLeave += MyCanvas_MouseLeave;

            Grid grid = CreateGridForProjects(item);

            Canvas.SetTop(grid, 5);
            Canvas.SetLeft(grid, 10);

            myCanvas.Height = grid.Height;

            myCanvas.Children.Add(grid);

            Border brdr = CreateBorderForProjectCanvas();
            brdr.Child = myCanvas;

            return brdr;
        }
        private Canvas CreateControlForProjectHeading(Project item)
        {
            Canvas myCanvas = CreateCanvasPanel(item);
            myCanvas.Width = 270;
            myCanvas.Background = new SolidColorBrush(Colors.Black);
            //myCanvas.MouseLeftButtonDown += ShowProject_Task;
            myCanvas.MouseEnter += MyCanvas_MouseEnter;
            myCanvas.MouseLeave += MyCanvas_MouseLeave;

            Grid grid = CreateGridForProjects(item);

            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;

            myCanvas.Children.Add(grid);


            return myCanvas;
        }

        private Canvas CreateCanvasPanel(Project item)
        {
            return new Canvas
            {
                Margin = new Thickness(0),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorArray[0].ToString())),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 281,
                MinHeight = 30,
                DataContext = item

            };
        }
        private static Canvas CreateCanvasPanelForTask(ProjectTask item)
        {
            return new Canvas
            {
                Margin = new Thickness(0),
                Background = new SolidColorBrush(Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 280,
                MinHeight = 50,
                DataContext = item

            };
        }
        private static Grid CreateGridForProjects(Project item)
        {
            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(30);
            ColumnDefinition column1 = new ColumnDefinition();

            column1.Width = new GridLength(280, GridUnitType.Pixel);
            grid.RowDefinitions.Add(row1);

            grid.ColumnDefinitions.Add(column1);

            TextBlock txtName = new TextBlock
            {
                FontSize = 16,
                Text = item.ProjectName,
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(5, 1, 1, 1),
                // FontWeight = FontWeights.Bold
            };


            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);

            grid.Height = row1.Height.Value + 1;
            grid.Children.Add(txtName);

            return grid;
        }

        //private Border CreateControlForSelectedProjctItem(Project item)
        //{
        //    Canvas myCanvas = CreateCanvasPanel(item);
        //    myCanvas.Background = new SolidColorBrush(Colors.White);

        //    //myCanvas.MouseLeftButtonDown += ShowProject_Task;
        //    //myCanvas.MouseEnter += MyCanvas_MouseEnter;
        //    //myCanvas.MouseLeave += MyCanvas_MouseLeave;

        //    Grid grid = CreateGridForSelectedProject(item);

        //    Canvas.SetTop(grid, 0);
        //    Canvas.SetLeft(grid, 0);

        //    myCanvas.Height = grid.Height;
        //    myCanvas.Children.Add(grid);

        //    Border brdr = CreateBorderForProjectCanvas();
        //    brdr.Margin = new Thickness(0);
        //    brdr.Child = myCanvas;
        //    return brdr;

        //}

        //private static Grid CreateGridForSelectedProject(Project item)
        //{
        //    Grid grid = new Grid();
        //    grid.Margin = new Thickness(0);

        //    RowDefinition row1 = new RowDefinition();

        //    row1.Height = new GridLength(30);
        //    RowDefinition row2 = new RowDefinition();

        //    row2.Height = new GridLength(30);

        //    ColumnDefinition column1 = new ColumnDefinition();
        //    column1.MinWidth = 200;
        //    column1.Width = new GridLength(200, GridUnitType.Pixel);
        //    ColumnDefinition column2 = new ColumnDefinition();
        //    column1.MinWidth = 80;
        //    column2.Width = new GridLength(80, GridUnitType.Pixel);



        //    grid.RowDefinitions.Add(row1);
        //    grid.RowDefinitions.Add(row2);
        //    grid.ColumnDefinitions.Add(column1);
        //    grid.ColumnDefinitions.Add(column2);


        //    //TextBlock txtName = new TextBlock
        //    //{
        //    //    FontSize = 14,
        //    //    Text = item.ProjectName,
        //    //    Foreground = new SolidColorBrush(Colors.White),
        //    //    TextWrapping = TextWrapping.Wrap
        //    //};

        //    TextBlock txtProjectDesc = new TextBlock
        //    {
        //        FontSize = 12,
        //        Text = item.Description,
        //        Foreground = new SolidColorBrush(Colors.White),
        //        Margin = new Thickness(0, 0, 0, 5),
        //        VerticalAlignment = VerticalAlignment.Bottom,
        //        TextWrapping = TextWrapping.Wrap

        //    };

        //    ResourceDictionary rd = new ResourceDictionary
        //    {
        //        Source = new Uri("../SwitchTypeToggleButton.xaml", UriKind.Relative)
        //    };

        //    ToggleButton toggleButton = new ToggleButton
        //    {
        //        Width = 45,
        //        Height = 22,
        //        Style = (Style)rd["SwitchTypeToggleButton"],

        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        VerticalAlignment = VerticalAlignment.Top,
        //        Margin = new Thickness(0, 2, 5, 0)
        //    };

        //    if (item.DifferenceInSecondsInCurrentDate == null)
        //    {
        //        TimeSpan ts = TimeSpan.FromSeconds(0);
        //        item.TimeElapsedValue = ts.ToString(@"hh\:mm\:ss");
        //        item.PreviousElapsedTime = ts;
        //    }
        //    else
        //    {
        //        TimeSpan ts = TimeSpan.FromSeconds(Convert.ToDouble(item.DifferenceInSecondsInCurrentDate));
        //        item.TimeElapsedValue = ts.ToString(@"hh\:mm\:ss");
        //        item.PreviousElapsedTime = ts;
        //    }


        //    TextBlock txtElapsedTime = new TextBlock
        //    {
        //        FontSize = 12,
        //        Text = item.TimeElapsedValue,
        //        Foreground = new SolidColorBrush(Colors.Black),
        //        Margin = new Thickness(0, 0, 0, 5),
        //        TextWrapping = TextWrapping.Wrap,
        //        VerticalAlignment = VerticalAlignment.Bottom,
        //    };

        //    //Grid.SetRow(txtName, 0);
        //    //Grid.SetColumn(txtName, 0);

        //    Grid.SetRow(txtProjectDesc, 0);
        //    Grid.SetColumn(txtProjectDesc, 0);

        //    Grid.SetRow(toggleButton, 0);
        //    Grid.SetColumn(toggleButton, 1);

        //    Grid.SetRow(txtElapsedTime, 1);
        //    Grid.SetColumn(txtElapsedTime, 1);

        //    //grid.Children.Add(txtName);
        //    grid.Children.Add(txtProjectDesc);
        //    grid.Children.Add(toggleButton);
        //    grid.Children.Add(txtElapsedTime);

        //    double GridHeight = row1.Height.Value + row2.Height.Value;
        //    grid.Height = GridHeight;
        //    return grid;
        //}

        private void MyCanvas_MouseLeave(object sender, MouseEventArgs e)
        {

            Canvas canvas = (Canvas)sender;

            var backColor = canvas.Background;
            if (backColor.ToString().ToLower() == "#FF8A2BE2".ToLower())
            {
                canvas.Background = new SolidColorBrush(Colors.Black);
            }
            else if (backColor.ToString().ToLower() == "#FFADFF2F".ToLower())
                canvas.Background = new SolidColorBrush(Colors.Tan);

        }

        private void MyCanvas_MouseEnter(object sender, MouseEventArgs e)
        {

            Canvas canvas = (Canvas)sender;
            var backColor = canvas.Background;

            if (backColor.ToString().ToLower() != "#FFD2B48C".ToLower())
            {
                canvas.Background = new SolidColorBrush(Colors.BlueViolet);
            }
            else
            {
                canvas.Background = new SolidColorBrush(Colors.GreenYellow);

            }
        }

        private void ShowProject_Task(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            try
            {
                var typeOfData = canvas.DataContext.GetType().Name;

                if (typeOfData.ToString().ToLower() == "project".ToLower())
                {
                    var project = (Project)canvas.DataContext;

                    GetTaskByProjectIDAndGenrateView(project, User.ID);
                }
                else if (typeOfData.ToString().ToLower() == "projecttask".ToLower())
                {
                    var task = (ProjectTask)canvas.DataContext;

                    //TODO need to Implement sub task view.

                    GetSubTaskByTaskIDAndGenrateView(User.Projects, task);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some error occured.");
                //throw;
            }

        }

        private void GetSubTaskByTaskIDAndGenrateView(ICollection<Project> projects, ProjectTask task)
        {
            // throw new NotImplementedException();
            //TODO need to implement subtask view.
            // DataTable taskData = GetTaskListByTaskIDFromDB(task, UserID);

            if (taskList.Count > 0)
            {

                // Getting tasklist which have parentid as selected project.

                var subTaskList = taskList.Where(x => x.ParentTaskId == task.TaskId).ToList();

                if (subTaskList.Count > 0)
                {
                    //TODO Create TaskSubTaskTemplate
                }
                else
                {
                    //Create subtask Details Template.
                }

                //var grid = (Grid)_dailyTaskView.FindName("grdProject");
                //grid.Children.Clear();

                StackPanel stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(5, 5, 5, 10);

                //if (projects.Count > 0)
                //{

                //    foreach (var item in projects)
                //    {
                //        Border border = new Border();
                //        if (item.Tasks.Count > 0)
                //        {
                //            //if (item.ProjectID == seletedProjectID)
                //            //{
                //            //    border = CreateControlForSelectedProjectTask(item);

                //            //}
                //            //else
                //            //{
                //            //    border = CreateControlForProjectCanvas(item);
                //            //}

                //        }
                //        else
                //        {
                //            border = CreateControlForProjectCanvas(item);
                //        }


                //        stackPanel.Children.Add(border);

                //    }
                //    grid.Children.Add(stackPanel);
                //}
            }
        }


        private void GetTaskByProjectIDAndGenrateView(Project project, int userID)
        {
            DataTable taskData = GetTaskListByProjectIDFromDB(project, userID);
            taskList.Clear();
            try
            {
                if (taskData.Rows.Count > 0)
                {
                    //List<ProjectTask> taskList = new List<ProjectTask>();

                    foreach (DataRow item in taskData.Rows)
                    {
                        ProjectTask task = new ProjectTask();
                        task.TaskId = Convert.ToInt32(item["TaskID"]);
                        task.ProjectID = Convert.ToInt32(item["ProjectID"]);
                        task.TaskCode = item["TaskCode"].ToString();
                        task.TaskName = item["TaskName"].ToString();
                        task.ParentTaskId = Convert.ToInt32(item["ParentTaskID"]);
                        task.TaskStatusID = Convert.ToInt32(item["StatusID"]);
                        task.TaskStartDate = Convert.ToDateTime(item["TaskStartDate"]);
                        task.TaskEndDate = Convert.ToDateTime(item["TaskEndDate"]);
                        task.ActualTaskStartDate = item["TaskStartDateActual"] != DBNull.Value ? Convert.ToDateTime(item["TaskStartDateActual"]) : (DateTime?)null;
                        task.ActualTaskEndDate = item["TaskEndDateActual"] != DBNull.Value ? Convert.ToDateTime(item["TaskEndDateActual"]) : (DateTime?)null;
                        task.IsActive = Convert.ToBoolean(item["isACTIVE"]);
                        task.IsMilestone = item["isMilestone"] != DBNull.Value ? Convert.ToBoolean(item["isMilestone"]) : (Boolean?)null;
                        task.CompletePercent = Convert.ToInt32(item["CompletePercent"]);
                        task.TaskDepthLevel = Convert.ToInt32(item["lvl"]);
                        taskList.Add(task);

                    }

                    //Adding tasklist to the project
                    var projectList = User.Projects;
                    var selectedProject = projectList.FirstOrDefault(x => x.ProjectID == project.ProjectID);
                    var taskList0Level = taskList.Where(x => x.ParentTaskId == 0).ToList();
                    //selectedProject.Tasks = taskList.Select(x=> x.ParentTaskId ==0).ToList();
                    selectedProject.Tasks = taskList0Level;

                    GenrateViewControl(projectList, project.ProjectID);
                }
                else
                {
                    // GenrateViewForProjectDetails(project);
                    //Border border = new Border();
                    //SelectedProjectViewHelper selectedProjectView = new SelectedProjectViewHelper();
                    // border = selectedProjectView.CreateTemplateForProjectAtLastLevel(project);
                    //GenrateViewControl(projectList, project.ProjectID);
                    var projectList = User.Projects;
                    GenrateViewControl(projectList, project.ProjectID);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void GenrateViewForProjectDetails(Project project)
        {
            try
            {
                Canvas headingControlWithBorder = CreateControlForProjectHeading(project);

                SetHeaderPanelStyle(headingControlWithBorder);
                Border border = new Border
                {
                    BorderThickness = new Thickness()
                    {
                        Bottom = 0,
                        Left = 0,
                        Right = 1,
                        Top = 1
                    },
                    BorderBrush = new SolidColorBrush(Colors.Green),
                    Margin = new Thickness(0)
                };
                border.Child = headingControlWithBorder;



            }
            catch (Exception)
            {

                throw;
            }

        }

        //private void GenrateViewControl(ICollection<Project> projectList, int? seletedProjectID)
        //{

        //    try
        //    {
        //        var grid = (Grid)_dailyTaskView.FindName("grdProject");
        //        grid.Children.Clear();

        //        StackPanel stackPanel = new StackPanel();
        //        stackPanel.Margin = new Thickness(5, 5, 5, 10);

        //        if (projectList.Count > 0)
        //        {

        //            List<Project> projects = new List<Project>();
        //            projects = projectList.ToList();


        //            foreach (var item in projects)
        //            {
        //                Border border = new Border();
        //                if (item.ProjectID == seletedProjectID)
        //                {
        //                    border = CreateControlForSelectedProjectTask(item);

        //                }
        //                else
        //                {
        //                    border = CreateControlForProjectCanvas(item);
        //                }

        //                stackPanel.Children.Add(border);

        //            }
        //            grid.Children.Add(stackPanel);
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        private void GenrateViewControl(ICollection<Project> projectList, int? seletedProjectID)
        {

            try
            {
                var grid = (Grid)_dailyTaskView.FindName("grdProject");
                grid.Children.Clear();

                StackPanel stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(5, 5, 5, 10);

                if (projectList.Count > 0)
                {

                    List<Project> projects = new List<Project>();
                    projects = projectList.ToList();

                    foreach (var item in projects)
                    {
                        Border border = new Border();
                        if (item.Tasks.Count > 0)
                        {
                            if (item.ProjectID == seletedProjectID)
                            {
                                border = CreateControlForSelectedProjectTask(item);

                            }
                            else
                            {
                                border = CreateControlForProjectCanvas(item);
                            }

                        }
                        else
                        {
                            if (item.ProjectID == seletedProjectID)
                            {

                                SelectedProjectViewHelper selectedProjectView = new SelectedProjectViewHelper();
                                border = selectedProjectView.CreateTemplateForProjectAtLastLevel(item);
                            }
                            else
                            {
                                border = CreateControlForProjectCanvas(item);
                            }
                        }

                        stackPanel.Children.Add(border);

                    }
                    grid.Children.Add(stackPanel);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private Border CreateControlForSelectedProjectTask(Project item)
        {

            //TODO I need to implement two canvas
            //1) first for project heading
            //2) for task

            //TODO this canvas should not have postion from top 5 and left 10
            Canvas headingControlWithBorder = CreateControlForProjectHeading(item);

            SetHeaderPanelStyle(headingControlWithBorder);
            Border border = new Border
            {
                BorderThickness = new Thickness()
                {
                    Bottom = 0,
                    Left = 0,
                    Right = 1,
                    Top = 1
                },
                BorderBrush = new SolidColorBrush(Colors.Green),

                Margin = new Thickness(0)
            };
            border.Child = headingControlWithBorder;
            StackPanel tasks = CreateControlForTaskList(item.Tasks.ToList());

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(border);
            stackPanel.Children.Add(tasks);

            //Canvas to contain both Project and task List.
            Canvas myCanvas = CreateCanvasPanel(item);
            myCanvas.Width = 281;
            myCanvas.Height = headingControlWithBorder.Height + tasks.Height + 2;
            myCanvas.Background = new SolidColorBrush(Colors.Tan);

            //myCanvas.MouseLeftButtonDown += ShowProject_Task;
            myCanvas.MouseEnter += MyCanvas_MouseEnter;
            myCanvas.MouseLeave += MyCanvas_MouseLeave;

            //TODO Add margin 5 and 10 this is the main panel displayed in the application.
            Canvas.SetTop(stackPanel, 0);
            Canvas.SetLeft(stackPanel, 0);

            myCanvas.Children.Add(stackPanel);
            //myCanvas.Height = 150;
            Border brdr = CreateBorderForSelectedProjectCanvas();


            brdr.Child = myCanvas;
            return brdr;

        }

        private StackPanel CreateControlForTaskList(List<ProjectTask> taskList)
        {

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            if (taskList.Count > 0)
            {
                double stackHeight = 0;
                foreach (var item in taskList)
                {
                    Canvas panel = new Canvas();
                    panel = CreateControlForTaskList(item); //ConvertHexToColor("#FF000000")


                    // 0 index is for project, so increasing task level by +1
                    var taskDepth = item.TaskDepthLevel + 1;
                    panel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorArray[taskDepth].ToString()));

                    Border border = CreateBorderForProjectCanvas();
                    border.Margin = new Thickness(0);
                    border.Child = panel;
                    stackHeight += panel.Height;
                    stackPanel.Height = stackHeight;
                    stackPanel.Children.Add(border);
                }

            }
            return stackPanel;
        }

        private Canvas CreateControlForTaskList(ProjectTask item)
        {
            Canvas myCanvas = CreateCanvasPanelForTask(item);
            myCanvas.Width = 270;

            myCanvas.MouseLeftButtonDown += ShowProject_Task;
            //myCanvas.MouseEnter += MyCanvas_MouseEnter;
            //myCanvas.MouseLeave += MyCanvas_MouseLeave;

            Grid grid = CreateGridForProjectsTasks(item);


            var gridSize = grid.RenderSize;
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);


            myCanvas.Height = grid.Height;
            myCanvas.Children.Add(grid);

            return myCanvas;
        }
        private static Border CreateBorderForProjectCanvas()
        {
            return new Border()
            {
                BorderThickness = new Thickness()
                {
                    Bottom = 1,
                    Left = 1,
                    Right = 1,
                    Top = 1
                },
                BorderBrush = new SolidColorBrush(Colors.Green),

                Margin = new Thickness(10, 5, 0, 5)
            };
        }
        private static Border CreateBorderForSelectedProjectCanvas()
        {
            return new Border()
            {
                BorderThickness = new Thickness()
                {
                    Bottom = 1,
                    Left = 10,
                    Right = 1,
                    Top = 1
                },
                BorderBrush = new SolidColorBrush(Colors.Green),

                Margin = new Thickness(10, 5, 0, 5)
            };
        }
        private static Grid CreateGridForProjectsTasks(ProjectTask item)
        {
            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(30);

            ColumnDefinition column1 = new ColumnDefinition();

            column1.Width = new GridLength(280, GridUnitType.Pixel);

            grid.RowDefinitions.Add(row1);

            grid.ColumnDefinitions.Add(column1);

            TextBlock txtName = new TextBlock
            {
                FontSize = 14,
                Text = item.TaskName,
                Padding = new Thickness(5, 1, 1, 1),
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center
            };

            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);


            grid.Height = row1.Height.Value + 10;
            grid.Children.Add(txtName);

            return grid;
        }
        private void CreateGridForSelectedProjectTask(Project item)
        {

        }


        private DataTable GetTaskListByProjectIDFromDB(Project project, int userID)
        {
            DataTable dt = new DataTable();
            try
            {

                string conStr = ConfigurationManager.ConnectionStrings["QBADBConnetion"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(conStr);

                var sqlCmd = new SqlCommand("USPtblMasterProjectTask_GetByProjectID", sqlCon)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCmd.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                sqlCmd.Parameters.AddWithValue("@UserID", userID);

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        private DataTable GetTaskListByTaskIDFromDB(ProjectTask task, int userID)
        {
            DataTable dt = new DataTable();
            try
            {

                string conStr = ConfigurationManager.ConnectionStrings["QBADBConnetion"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(conStr);

                var sqlCmd = new SqlCommand("USPtblMasterProjectTask_GetByTaskID", sqlCon)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                //sqlCmd.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                //sqlCmd.Parameters.AddWithValue("@UserID", userID);

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        private void SetHeaderPanelStyle(Canvas canvas)
        {
            canvas.Height = 30;

        }



    }


}
