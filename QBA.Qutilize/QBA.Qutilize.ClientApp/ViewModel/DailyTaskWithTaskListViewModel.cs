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
using System.Windows.Controls.Primitives;
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
        string[] ColorArray = new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7", "#194C66", "#1F5D7C", "#246E93", "#2A7FAA", "#3090C0", "#3E9ECE", "#55AAD4", "#6BB5DA", "#82C0DF" };
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
                        border = CreateControlForSelectedItem(item);
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


            var gridSize = grid.RenderSize;
            Canvas.SetTop(grid, 5);
            Canvas.SetLeft(grid, 10);



            myCanvas.Children.Add(grid);
            Border brdr = CreateBorderForSelectedProjectCanvas();
            brdr.Child = myCanvas;

            // var gridsize= grid.re
            myCanvas.Height = 30;
            return brdr;
        }

        //private static Border CreateBorderForCanvas()
        //{
        //    return new Border()
        //    {
        //        BorderThickness = new Thickness()
        //        {
        //            Bottom = 1,
        //            Left = 1,
        //            Right = 1,
        //            Top = 1
        //        },
        //        BorderBrush = new SolidColorBrush(Colors.Black),
        //        Margin = new Thickness(10, 5, 0, 5)
        //    };
        //}

        private static Canvas CreateCanvasPanel(Project item)
        {
            return new Canvas
            {
                Margin = new Thickness(0),
                Background = new SolidColorBrush(Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 281,
                MinHeight = 50,
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
                MinWidth = 281,
                MinHeight = 50,
                DataContext = item

            };
        }
        private static Grid CreateGridForProjects(Project item)
        {
            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(0, GridUnitType.Auto);
            //RowDefinition row2 = new RowDefinition();
            //row2.Height = new GridLength(0, GridUnitType.Auto);

            ColumnDefinition column1 = new ColumnDefinition();

            column1.Width = new GridLength(280, GridUnitType.Pixel);
            //ColumnDefinition column2 = new ColumnDefinition();
            //column1.Width = new GridLength(80, GridUnitType.Pixel);

            grid.RowDefinitions.Add(row1);
            //grid.RowDefinitions.Add(row2);
            grid.ColumnDefinitions.Add(column1);
            //grid.ColumnDefinitions.Add(column2);


            TextBlock txtName = new TextBlock
            {
                FontSize = 14,
                Text = item.ProjectName,

                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap
            };

            //TextBlock txtProjectDesc = new TextBlock
            //{
            //    FontSize = 12,
            //    Text = item.Description,
            //    Foreground = new SolidColorBrush(Colors.Black),
            //    Margin = new Thickness(0, 0, 0, 5),
            //    TextWrapping = TextWrapping.Wrap
            //};

            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);

            //Grid.SetRow(txtProjectDesc, 1);
            //Grid.SetColumn(txtProjectDesc, 0);

            grid.Children.Add(txtName);
            // grid.Children.Add(txtProjectDesc);
            return grid;
        }

        private Border CreateControlForSelectedItem(Project item)
        {
            Canvas myCanvas = CreateCanvasPanel(item);
            myCanvas.Background = new SolidColorBrush(Colors.Tan);

            myCanvas.MouseLeftButtonDown += ShowProject_Task;
            myCanvas.MouseEnter += MyCanvas_MouseEnter;
            myCanvas.MouseLeave += MyCanvas_MouseLeave;

            Grid grid = CreateGridForSelectedProject(item);

            Canvas.SetTop(grid, 5);
            Canvas.SetLeft(grid, 10);

            myCanvas.Children.Add(grid);

            Border brdr = new Border()
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

            brdr.Child = myCanvas;
            return brdr;

        }

        private static Grid CreateGridForSelectedProject(Project item)
        {
            Grid grid = new Grid();
            grid.Margin = new Thickness(0);
            //grid.ShowGridLines = true;
            RowDefinition row1 = new RowDefinition();
            //row1.MinHeight = 30;
            row1.Height = new GridLength(0, GridUnitType.Auto);

            RowDefinition row2 = new RowDefinition();
            //row2.MinHeight = 30;
            row2.Height = new GridLength(0, GridUnitType.Auto);

            ColumnDefinition column1 = new ColumnDefinition();
            column1.MinWidth = 200;
            column1.Width = new GridLength(200, GridUnitType.Pixel);
            ColumnDefinition column2 = new ColumnDefinition();
            column1.MinWidth = 80;
            column2.Width = new GridLength(80, GridUnitType.Pixel);



            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);


            TextBlock txtName = new TextBlock
            {
                FontSize = 14,
                Text = item.ProjectName,
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap
            };

            TextBlock txtProjectDesc = new TextBlock
            {
                FontSize = 12,
                Text = item.Description,
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(0, 0, 0, 5),
                VerticalAlignment = VerticalAlignment.Bottom,
                TextWrapping = TextWrapping.Wrap

            };

            ResourceDictionary rd = new ResourceDictionary
            {
                Source = new Uri("../SwitchTypeToggleButton.xaml", UriKind.Relative)
            };

            ToggleButton toggleButton = new ToggleButton
            {
                Width = 45,
                Height = 22,
                Style = (Style)rd["SwitchTypeToggleButton"],

                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 2, 5, 0)
            };

            if (item.DifferenceInSecondsInCurrentDate == null)
            {
                TimeSpan ts = TimeSpan.FromSeconds(0);
                item.TimeElapsedValue = ts.ToString(@"hh\:mm\:ss");
                item.PreviousElapsedTime = ts;
            }
            else
            {
                TimeSpan ts = TimeSpan.FromSeconds(Convert.ToDouble(item.DifferenceInSecondsInCurrentDate));
                item.TimeElapsedValue = ts.ToString(@"hh\:mm\:ss");
                item.PreviousElapsedTime = ts;
            }


            TextBlock txtElapsedTime = new TextBlock
            {
                FontSize = 12,
                Text = item.TimeElapsedValue,
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Bottom,
            };

            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);

            Grid.SetRow(txtProjectDesc, 1);
            Grid.SetColumn(txtProjectDesc, 0);

            Grid.SetRow(toggleButton, 0);
            Grid.SetColumn(toggleButton, 1);

            Grid.SetRow(txtElapsedTime, 1);
            Grid.SetColumn(txtElapsedTime, 1);

            grid.Children.Add(txtName);
            grid.Children.Add(txtProjectDesc);
            grid.Children.Add(toggleButton);
            grid.Children.Add(txtElapsedTime);
            return grid;
        }

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
            var typeOfData = canvas.DataContext.GetType();

            if (typeOfData.ToString().ToLower().Contains("project"))
            {
                var project = (Project)canvas.DataContext;

                GetTaskByProjectID(project, User.ID);
            }
            else
            {

            }

        }

        // SqlHelper objSQLHelper = new SqlHelper();
        private void GetTaskByProjectID(Project project, int userID)
        {
            DataTable taskData = GetTaskListByProjectDIFromDB(project, userID);
            if (taskData.Rows.Count > 0)
            {
                List<ProjectTask> taskList = new List<ProjectTask>();

                foreach (DataRow item in taskData.Rows)
                {
                    ProjectTask task = new ProjectTask();
                    task.TaskId = Convert.ToInt32(item["TaskID"]);
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

                    taskList.Add(task);

                }

                //Adding tasklist to the project
                var projectList = User.Projects;
                var selectedProject = projectList.FirstOrDefault(x => x.ProjectID == project.ProjectID);
                selectedProject.Tasks = taskList;

                GenrateViewControl(projectList, project.ProjectID);

            }

        }

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
                        if (item.ProjectID == seletedProjectID)
                        {
                            border = CreateControlForSelectedProjectTask(item);
                            // border = CreateControlForProjectCanvas(item);
                            break;
                        }
                        else
                        {
                            border = CreateControlForProjectCanvas(item);
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


            Border headeingControlWithBorder = CreateControlForProjectCanvas(item);
            StackPanel tasks = CreateControlForTaskList(item.Tasks.ToList());

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(headeingControlWithBorder);
            stackPanel.Children.Add(tasks);

            Canvas myCanvas = CreateCanvasPanel(item);
            myCanvas.Width = 291;
            // myCanvas.Height = 100;

            myCanvas.MouseLeftButtonDown += ShowProject_Task;
            myCanvas.MouseEnter += MyCanvas_MouseEnter;
            myCanvas.MouseLeave += MyCanvas_MouseLeave;

            Canvas.SetTop(stackPanel, 0);
            Canvas.SetLeft(stackPanel, 0);

            myCanvas.Children.Add(stackPanel);

            Border brdr = CreateBorderForProjectCanvas();
            brdr.Margin = new Thickness(0);
            brdr.Child = myCanvas;
            return brdr;

        }

        private StackPanel CreateControlForTaskList(List<ProjectTask> taskList)
        {
            // List<Border> taskControlsWithBorder = new List<Border>();
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical; if (taskList.Count > 0)
            {
                foreach (var item in taskList)
                {
                    Border border = new Border();
                    border = CreateControlForTaskList(item);
                    stackPanel.Children.Add(border);
                }

            }
            return stackPanel;
        }

        private Border CreateControlForTaskList(ProjectTask item)
        {
            Canvas myCanvas = CreateCanvasPanelForTask(item);
            myCanvas.Width = 291;

            myCanvas.MouseLeftButtonDown += ShowProject_Task;
            myCanvas.MouseEnter += MyCanvas_MouseEnter;
            myCanvas.MouseLeave += MyCanvas_MouseLeave;

            Grid grid = CreateGridForProjectsTasks(item);


            var gridSize = grid.RenderSize;
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);



            myCanvas.Children.Add(grid);
            Border brdr = CreateBorderForProjectCanvas();
            brdr.Child = myCanvas;

            // var gridsize= grid.re
            myCanvas.Height = 30;
            return brdr;
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
                    Left = 1,
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
            row1.Height = new GridLength(0, GridUnitType.Auto);
            //RowDefinition row2 = new RowDefinition();
            //row2.Height = new GridLength(0, GridUnitType.Auto);

            ColumnDefinition column1 = new ColumnDefinition();

            column1.Width = new GridLength(280, GridUnitType.Pixel);
            //ColumnDefinition column2 = new ColumnDefinition();
            //column1.Width = new GridLength(80, GridUnitType.Pixel);

            grid.RowDefinitions.Add(row1);
            //grid.RowDefinitions.Add(row2);
            grid.ColumnDefinitions.Add(column1);
            //grid.ColumnDefinitions.Add(column2);


            TextBlock txtName = new TextBlock
            {
                FontSize = 14,
                Text = item.TaskName,

                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap
            };



            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);



            grid.Children.Add(txtName);

            return grid;
        }
        private void CreateGridForSelectedProjectTask(Project item)
        {
            //foreach (var task in item.Tasks)
            //{
            //    Canvas myCanvas = CreateCanvasPanel(item);
            //    myCanvas.Width = 291;

            //    myCanvas.MouseLeftButtonDown += ShowProject_Task;
            //    myCanvas.MouseEnter += MyCanvas_MouseEnter;
            //    myCanvas.MouseLeave += MyCanvas_MouseLeave;


            //}




            //Grid grid = new Grid();
            //grid.Margin = new Thickness(0);

            //RowDefinition row1 = new RowDefinition();
            //row1.Height = new GridLength(0, GridUnitType.Auto);
            //RowDefinition row2 = new RowDefinition();
            //row2.Height = new GridLength(0, GridUnitType.Auto);

            //ColumnDefinition column1 = new ColumnDefinition();
            //column1.Width = new GridLength(280, GridUnitType.Pixel);


            //grid.RowDefinitions.Add(row1);
            //grid.RowDefinitions.Add(row2);
            //grid.ColumnDefinitions.Add(column1);

            //TextBlock txtName = new TextBlock
            //{
            //    FontSize = 14,
            //    Text = item.ProjectName,

            //    Foreground = new SolidColorBrush(Colors.White),
            //    TextWrapping = TextWrapping.Wrap
            //};

            //if(item.Tasks.Count > 0)
            //{
            //    StackPanel stackPanel = new StackPanel();
            //    stackPanel.Margin = new Thickness(5, 5, 5, 10);
            //}



            //Grid.SetRow(txtName, 0);
            //Grid.SetColumn(txtName, 0);



            //grid.Children.Add(txtName);

            //return grid;
        }

        private DataTable GetTaskListByProjectDIFromDB(Project project, int userID)
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
    }


}
