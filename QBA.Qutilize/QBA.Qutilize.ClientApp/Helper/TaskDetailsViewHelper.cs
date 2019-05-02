using QBA.Qutilize.ClientApp.ViewModel;
using QBA.Qutilize.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.Helper
{
    public class TaskDetailsViewHelper
    {
        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;

            }
        }
        public TaskDetailsViewHelper(User user)
        {
            this.User = user;
        }

        //public Canvas CreateTemplateForTaskAtLastLevel(ProjectTask task, string backgroundColor)
        //{
        //    var headerControl = CreateControlForTaskHeadingCanvas(task);
        //    headerControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundColor));


        //    var bodyControl = CreateControlForSelectedTaskItem(task);
        //    bodyControl.Background = new SolidColorBrush(Colors.White);

        //    bodyControl.MouseLeftButtonDown += BodyControl_MouseLeftButtonDown; ;
        //    bodyControl.MouseEnter += BodyControl_MouseEnter;
        //    bodyControl.MouseLeave += BodyControl_MouseLeave;


        //    //Stackpanel to contain both heading and body..
        //    StackPanel stackPanel = new StackPanel();
        //    stackPanel.Orientation = Orientation.Vertical;
        //    stackPanel.Children.Add(headerControl);
        //    stackPanel.Children.Add(bodyControl);

        //    stackPanel.Height = headerControl.Height + bodyControl.Height;


        //    Canvas myCanvas = CreateCanvasPanel(task);
        //    myCanvas.Width = 281;
        //    myCanvas.Height = stackPanel.Height;

        //    Canvas.SetTop(stackPanel, 0);
        //    Canvas.SetLeft(stackPanel, 0);

        //    myCanvas.Children.Add(stackPanel);

        //    //Border brdr = CreateBorderForSelectedProjectCanvas();


        //    //brdr.Child = myCanvas;
        //    //return brdr;

        //    return myCanvas;
        //}
        public Border CreateTemplateForTaskAtLastLevel(ProjectTask task, string backgroundColor)
        {
            var headerControl = CreateControlForTaskHeadingCanvas(task);
            headerControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundColor));


            var bodyControl = CreateControlForSelectedTaskItem(task);
            bodyControl.Background = new SolidColorBrush(Colors.White);

            bodyControl.MouseLeftButtonDown += BodyControl_MouseLeftButtonDown; ;
            bodyControl.MouseEnter += BodyControl_MouseEnter;
            bodyControl.MouseLeave += BodyControl_MouseLeave;


            //Stackpanel to contain both heading and body..
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(headerControl);
            stackPanel.Children.Add(bodyControl);

            stackPanel.Height = headerControl.Height + bodyControl.Height;


            Canvas myCanvas = CreateCanvasPanel(task);
            myCanvas.Width = 281;
            myCanvas.Height = stackPanel.Height;

            Canvas.SetTop(stackPanel, 0);
            Canvas.SetLeft(stackPanel, 0);

            myCanvas.Children.Add(stackPanel);

            Border brdr = CreateBorderForSelectedProjectCanvas();
            brdr.Margin = new Thickness(0);
            brdr.BorderThickness = new Thickness(1, 1, 1, 1);
            // brdr.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundColor));
            brdr.BorderBrush = new SolidColorBrush(Colors.Black);

            brdr.Child = myCanvas;
            return brdr;

            //return myCanvas;
        }
        private void BodyControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void BodyControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BodyControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
            //MessageBox.Show("need to implement");

        }

        private Canvas CreateControlForSelectedTaskItem(ProjectTask task)
        {
            Canvas myCanvas = CreateCanvasPanel(task);

            try
            {
                myCanvas.Background = new SolidColorBrush(Colors.White);

                Grid grid = CreateGridForSelectedTaskDetails(task);

                Canvas.SetTop(grid, 0);
                Canvas.SetLeft(grid, 0);

                myCanvas.Height = grid.Height;
                myCanvas.Children.Add(grid);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return myCanvas;
        }

        private Grid CreateGridForSelectedTaskDetails(ProjectTask task)
        {
            // throw new NotImplementedException();


            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            RowDefinition row1 = new RowDefinition
            {
                Height = new GridLength(30)
            };
            RowDefinition row2 = new RowDefinition
            {
                Height = new GridLength(30)
            };

            ColumnDefinition column1 = new ColumnDefinition
            {
                MinWidth = 200,
                Width = new GridLength(200, GridUnitType.Pixel)
            };

            ColumnDefinition column2 = new ColumnDefinition
            {
                MinWidth = 80,
                Width = new GridLength(80, GridUnitType.Pixel)
            };



            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);


            TextBlock txtTaskDesc = new TextBlock
            {
                FontSize = 14,
                Padding = new Thickness(5, 0, 0, 0),
                Text = "Description Not Available",
                Foreground = new SolidColorBrush(Colors.Black),
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

                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 2, 5, 0),
                IsChecked = true
            };

            //if (item.DifferenceInSecondsInCurrentDate == null)
            //{
            //    TimeSpan ts = TimeSpan.FromSeconds(0);
            //    item.TimeElapsedValue = ts.ToString(@"hh\:mm\:ss");
            //    item.PreviousElapsedTime = ts;
            //}
            //else
            //{
            //    TimeSpan ts = TimeSpan.FromSeconds(Convert.ToDouble(item.DifferenceInSecondsInCurrentDate));
            //    item.TimeElapsedValue = ts.ToString(@"hh\:mm\:ss");
            //    item.PreviousElapsedTime = ts;
            //}


            TextBlock txtElapsedTime = new TextBlock
            {
                FontSize = 14,

                Text = "00:00:00",
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center

            };


            Grid.SetRow(txtTaskDesc, 0);
            Grid.SetColumn(txtTaskDesc, 0);

            Grid.SetRow(toggleButton, 0);
            Grid.SetColumn(toggleButton, 1);

            Grid.SetRow(txtElapsedTime, 1);
            Grid.SetColumn(txtElapsedTime, 1);


            grid.Children.Add(txtTaskDesc);
            grid.Children.Add(toggleButton);
            grid.Children.Add(txtElapsedTime);

            double GridHeight = row1.Height.Value + row2.Height.Value;
            grid.Height = GridHeight;
            return grid;
        }

        private Canvas CreateControlForTaskHeadingCanvas(ProjectTask projectTask)
        {

            Canvas myCanvas = CreateCanvasPanel(projectTask);
            myCanvas.Width = 280;

            Grid grid = CreateGridForTaskHeading(projectTask);

            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;

            myCanvas.Children.Add(grid);
            return myCanvas;
        }

        private Canvas CreateCanvasPanel(ProjectTask task)
        {
            return new Canvas
            {
                Margin = new Thickness(0),
                //  Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorArray[0].ToString())),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 280,
                MinHeight = 30,
                DataContext = task

            };
        }

        private static Grid CreateGridForTaskHeading(ProjectTask item)
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
                Text = item.TaskName,
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(5, 1, 1, 1),

            };


            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);

            grid.Height = row1.Height.Value + 1;
            grid.Children.Add(txtName);

            return grid;
        }
        private static Grid CreateGridForProjectHeading(ProjectTask item)
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

            };


            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);

            grid.Height = row1.Height.Value + 1;
            grid.Children.Add(txtName);

            return grid;
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

        public Canvas CreateProjectHeadingControl(Project project, string backColor)
        {
            Canvas myCanvas = CreateCanvasPanelForProjectHeading(project);
            myCanvas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));
            myCanvas.Width = 281;

            Grid grid = CreateGridForProjectHeading(project);

            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;
            myCanvas.Children.Add(grid);

            return myCanvas;
        }

        private Grid CreateGridForProjectHeading(Project project)
        {
            //throw new NotImplementedException();
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
                Text = project.ProjectName,
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

        public Canvas CreateControlForTaskItem(ProjectTask item, string backColor)
        {
            Canvas myCanvas = CreateCanvasPanelForTask(item);
            myCanvas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));
            myCanvas.Width = 276;

            myCanvas.MouseLeftButtonDown += MyCanvas_MouseLeftButtonDown; ;
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
        //public Border CreateControlForTaskItem(ProjectTask item, string backColor)
        //{
        //    Border border = new Border();
        //    try
        //    {
        //        Canvas myCanvas = CreateCanvasPanelForTask(item);
        //        myCanvas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));
        //        myCanvas.Width = 276;

        //        myCanvas.MouseLeftButtonDown += MyCanvas_MouseLeftButtonDown;
        //        //myCanvas.MouseEnter += MyCanvas_MouseEnter;
        //        //myCanvas.MouseLeave += MyCanvas_MouseLeave;

        //        Grid grid = CreateGridForProjectsTasks(item);


        //        var gridSize = grid.RenderSize;
        //        Canvas.SetTop(grid, 0);
        //        Canvas.SetLeft(grid, 0);


        //        myCanvas.Height = grid.Height;
        //        myCanvas.Children.Add(grid);

        //        border = CreateBorderForSelectedProjectCanvas();
        //        border.Margin = new Thickness(0);
        //        border.BorderThickness = new Thickness(1);
        //        border.Child = myCanvas;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //    return border;
        //}
        private void MyCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //  throw new NotImplementedException();
            // MessageBox.Show("test.");

            try
            {
                DailyTaskWithTaskListViewModel viewModel = new DailyTaskWithTaskListViewModel();
                viewModel.User = this.User;
                viewModel.ShowProject_Task(sender, e);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static Canvas CreateCanvasPanelForTask(ProjectTask item)
        {
            return new Canvas
            {
                Margin = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 280,
                MinHeight = 50,
                DataContext = item

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

        private Canvas CreateCanvasPanelForProjectHeading(Project item)
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

    }
}
