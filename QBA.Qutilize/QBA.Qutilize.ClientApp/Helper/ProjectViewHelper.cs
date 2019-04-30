using QBA.Qutilize.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.Helper
{
    public class ProjectViewHelper
    {

        public Border CreateControlForProjectCanvas(Project item, string[] ColorArray)
        {
            Canvas myCanvas = CreateCanvasPanel(item);
            myCanvas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorArray[0].ToString()));
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

        private void MyCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
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
            // throw new NotImplementedException();
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
            //throw new NotImplementedException();
            Canvas canvas = (Canvas)sender;
            try
            {
                var typeOfData = canvas.DataContext.GetType().Name;

                if (typeOfData.ToString().ToLower() == "project".ToLower())
                {
                    var project = (Project)canvas.DataContext;

                   // GetTaskByProjectIDAndGenrateView(project, User.ID);
                }
                else if (typeOfData.ToString().ToLower() == "projecttask".ToLower())
                {
                    var task = (ProjectTask)canvas.DataContext;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some error occured.");
                //throw;
            }

        }

        private Canvas CreateCanvasPanel(Project item)
        {
            return new Canvas
            {
                Margin = new Thickness(0),

                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 281,
                MinHeight = 30,
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


        //private void GetTaskByProjectIDAndGenrateView(Project project, int userID)
        //{
        //    DataTable taskData = GetTaskListByProjectIDFromDB(project, userID);

        //    try
        //    {
        //        if (taskData.Rows.Count > 0)
        //        {
        //            List<ProjectTask> taskList = new List<ProjectTask>();

        //            foreach (DataRow item in taskData.Rows)
        //            {
        //                ProjectTask task = new ProjectTask();
        //                task.TaskId = Convert.ToInt32(item["TaskID"]);
        //                task.TaskCode = item["TaskCode"].ToString();
        //                task.TaskName = item["TaskName"].ToString();
        //                task.ParentTaskId = Convert.ToInt32(item["ParentTaskID"]);
        //                task.TaskStatusID = Convert.ToInt32(item["StatusID"]);
        //                task.TaskStartDate = Convert.ToDateTime(item["TaskStartDate"]);
        //                task.TaskEndDate = Convert.ToDateTime(item["TaskEndDate"]);
        //                task.ActualTaskStartDate = item["TaskStartDateActual"] != DBNull.Value ? Convert.ToDateTime(item["TaskStartDateActual"]) : (DateTime?)null;
        //                task.ActualTaskEndDate = item["TaskEndDateActual"] != DBNull.Value ? Convert.ToDateTime(item["TaskEndDateActual"]) : (DateTime?)null;
        //                task.IsActive = Convert.ToBoolean(item["isACTIVE"]);
        //                task.IsMilestone = item["isMilestone"] != DBNull.Value ? Convert.ToBoolean(item["isMilestone"]) : (Boolean?)null;
        //                task.CompletePercent = Convert.ToInt32(item["CompletePercent"]);

        //                taskList.Add(task);

        //            }

        //            //Adding tasklist to the project
        //            var projectList = User.Projects;
        //            var selectedProject = projectList.FirstOrDefault(x => x.ProjectID == project.ProjectID);
        //            selectedProject.Tasks = taskList;

        //            GenrateViewControl(projectList, project.ProjectID);
        //        }
        //        else
        //        {
        //            // GenrateViewForProjectDetails(project);
        //            //Border border = new Border();
        //            //SelectedProjectViewHelper selectedProjectView = new SelectedProjectViewHelper();
        //            // border = selectedProjectView.CreateTemplateForProjectAtLastLevel(project);
        //            //GenrateViewControl(projectList, project.ProjectID);
        //            var projectList = User.Projects;
        //            GenrateViewControl(projectList, project.ProjectID);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }


        //}

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
        //                if (item.Tasks.Count > 0)
        //                {
        //                    if (item.ProjectID == seletedProjectID)
        //                    {
        //                        border = CreateControlForSelectedProjectTask(item);

        //                    }
        //                    else
        //                    {
        //                        border = CreateControlForProjectCanvas(item);
        //                    }

        //                }
        //                else
        //                {
        //                    if (item.ProjectID == seletedProjectID)
        //                    {

        //                        SelectedProjectViewHelper selectedProjectView = new SelectedProjectViewHelper();
        //                        border = selectedProjectView.CreateTemplateForProjectAtLastLevel(item);
        //                    }
        //                    else
        //                    {
        //                        border = CreateControlForProjectCanvas(item);
        //                    }
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
    }


}
