using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class DailyTaskWithTaskListViewModel
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

        public void CreateProjectListControls(NewDailyTask view, User user)
        {
            if (user == null)
            {
                throw new System.ArgumentNullException(nameof(user));
            }

            var grid = (Grid)view.FindName("grdProject");
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(10, 5, 5, 10);

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

                foreach (var item in projects)
                {

                    Canvas myCanvas = CreateCanvas(item);

                    Border brdr = new Border()
                    {
                        BorderThickness = new Thickness()
                        {
                            Bottom = 1,
                            Left = 1,
                            Right = 1,
                            Top = 1
                        },
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        Margin = new Thickness(0, 2, 0, 2)
                    };
                    brdr.Child = myCanvas;

                    stackPanel.Children.Add(brdr);

                }
                grid.Children.Add(stackPanel);
            }


        }

        private Canvas CreateCanvas(Project item)
        {

            Canvas myCanvas = new Canvas
            {
                Margin = new Thickness(5),
                Background = new SolidColorBrush(Colors.AliceBlue),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 290,
                //MaxWidth = 350,
                MinHeight = 50,
                DataContext = item

            };

            myCanvas.MouseLeftButtonDown += ShowProject_Task;
            myCanvas.MouseEnter += MyCanvas_MouseEnter;
            myCanvas.MouseLeave += MyCanvas_MouseLeave;

            if (item.ProjectID == 155)
            {
                Grid grid = new Grid();
                grid.Margin = new Thickness(0);

                RowDefinition row1 = new RowDefinition();
                row1.Height = new GridLength(30);
                RowDefinition row2 = new RowDefinition();
                row2.Height = new GridLength(25);

                ColumnDefinition column1 = new ColumnDefinition();
                column1.Width = new GridLength(200);
                ColumnDefinition column2 = new ColumnDefinition();

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

                    HorizontalAlignment = HorizontalAlignment.Right,
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
                    TextWrapping = TextWrapping.Wrap

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

                Canvas.SetTop(grid, 5);
                Canvas.SetLeft(grid, 10);


                myCanvas.Children.Add(grid);
                myCanvas.Background = new SolidColorBrush(Colors.Tan);
                myCanvas.Height = 30 + 25 + 5;


            }
            else
            {
                Grid grid = new Grid();
                grid.Margin = new Thickness(0);
                RowDefinition row1 = new RowDefinition();
                RowDefinition row2 = new RowDefinition();

                ColumnDefinition column1 = new ColumnDefinition();
                ColumnDefinition column2 = new ColumnDefinition();
                grid.RowDefinitions.Add(row1);
                grid.RowDefinitions.Add(row2);
                grid.ColumnDefinitions.Add(column1);
                grid.ColumnDefinitions.Add(column2);


                TextBlock txtName = new TextBlock
                {
                    FontSize = 14,
                    Text = item.ProjectName,
                    Foreground = new SolidColorBrush(Colors.Black),

                };

                TextBlock txtProjectDesc = new TextBlock
                {
                    FontSize = 12,
                    Text = item.Description,
                    Foreground = new SolidColorBrush(Colors.Black),

                };
                Grid.SetRow(txtName, 0);
                Grid.SetColumn(txtName, 0);

                Grid.SetRow(txtProjectDesc, 1);
                Grid.SetColumn(txtProjectDesc, 0);

                grid.Children.Add(txtName);
                grid.Children.Add(txtProjectDesc);

                Canvas.SetTop(grid, 5);
                Canvas.SetLeft(grid, 10);


                myCanvas.Children.Add(grid);

            }

            return myCanvas;
        }

        private void MyCanvas_MouseLeave(object sender, MouseEventArgs e)
        {

            Canvas canvas = (Canvas)sender;

            var backColor = canvas.Background;
            if (backColor.ToString().ToLower() == "#FF8A2BE2".ToLower())
            {
                canvas.Background = new SolidColorBrush(Colors.AliceBlue);
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
                MessageBox.Show(project.ProjectID.ToString());
            }

        }

    }


}
