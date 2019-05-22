using QBA.Qutilize.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.Helper.WPFControlHelper
{
    public static class WPFGridHelper
    {
        //public static Grid CreateProjectHeadingGrid(Project project)
        //{
        //    TextBlock txtName = new TextBlock
        //    {
        //        Name = "txtName_" + project.ProjectID,
        //        Padding = new Thickness(5, 0, 0, 0),
        //        Text = project.ProjectName,
        //        FontSize = 16,
        //        Foreground = new SolidColorBrush(Colors.White),
        //        TextWrapping = TextWrapping.NoWrap,

        //        Visibility = Visibility.Visible,
        //        FontWeight = FontWeights.Medium,
        //        ToolTip = project.ProjectName,

        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        VerticalAlignment = VerticalAlignment.Center,
        //    };
        //    TextBlock txtTaskCount = new TextBlock
        //    {
        //        Name = "txtTaskCount_" + project.ProjectID,
        //        Text = "Task Count(" + project.TaskCount.ToString() + ")",
        //        FontSize = 12,
        //        Foreground = new SolidColorBrush(Colors.White),
        //        TextWrapping = TextWrapping.Wrap,
        //        Visibility = Visibility.Visible,
        //        HorizontalAlignment = HorizontalAlignment.Right,
        //        VerticalAlignment = VerticalAlignment.Center,
        //        Padding = new Thickness(1, 1, 5, 1),


        //    };

        //    Grid grid = new Grid();
        //    grid.Margin = new Thickness(0);

        //    ColumnDefinition column1 = new ColumnDefinition
        //    {
        //        Width = new GridLength(150, GridUnitType.Star)
        //    };
        //    ColumnDefinition column2 = new ColumnDefinition
        //    {
        //        Width = new GridLength(130, GridUnitType.Star)
        //    };

        //    RowDefinition row1 = new RowDefinition
        //    {
        //        MinHeight = 30,
        //        Height = new GridLength(30, GridUnitType.Auto)
        //    };
        //    grid.RowDefinitions.Add(row1);
        //    grid.ColumnDefinitions.Add(column1);
        //    grid.ColumnDefinitions.Add(column2);

        //    Grid.SetRow(txtName, 0);
        //    Grid.SetColumn(txtName, 0);

        //    Grid.SetRow(txtTaskCount, 0);
        //    Grid.SetColumn(txtTaskCount, 1);

        //    grid.Width = column1.Width.Value + column2.Width.Value;
        //    double GridHeight = (row1.MinHeight * row1.Height.Value) + 1;
        //    grid.Height = GridHeight;

        //    grid.Children.Add(txtName);
        //    grid.Children.Add(txtTaskCount);
        //    return grid;
        //}


        public static Grid CreateProjectHeadingGrid(Project project)
        {
            TextBlock txtName = new TextBlock
            {
                Name = "txtName_" + project.ProjectID,
                Padding = new Thickness(5, 0, 0, 0),
                Text = project.ProjectName,
                FontSize = 16,
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.WrapWithOverflow,

                Visibility = Visibility.Visible,
                FontWeight = FontWeights.Medium,
                ToolTip = project.ProjectName,

                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };


            TextBlock txtTaskCount = new TextBlock
            {
                Name = "txtTaskCount_" + project.ProjectID,
                Text = "Task Count(" + project.TaskCount.ToString() + ")",
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap,
                Visibility = Visibility.Visible,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(1, 1, 5, 1),

            };

            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            ColumnDefinition column1 = new ColumnDefinition
            {
                Width = new GridLength(150, GridUnitType.Star)
            };
            ColumnDefinition column2 = new ColumnDefinition
            {
                Width = new GridLength(130, GridUnitType.Star)
            };

            RowDefinition row1 = new RowDefinition
            {
                MinHeight = 50,
                //Height = new GridLength(1, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row1);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);

            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);

            Grid.SetRow(txtTaskCount, 0);
            Grid.SetColumn(txtTaskCount, 1);

            grid.Width = column1.Width.Value + column2.Width.Value;



            grid.Children.Add(txtName);
            grid.Children.Add(txtTaskCount);

            SetGridHeight(grid);
            return grid;
        }


        public static Grid CreateProjectDetailsGrid(Project item)
        {


            TextBlock txtProjectDesc = new TextBlock
            {
                FontSize = 14,
                Padding = new Thickness(5, 0, 0, 0),
                Text = (item.Description == "" || item.Description == null) ? "Description Not Available" : item.Description,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 0, 5),
                VerticalAlignment = VerticalAlignment.Bottom,
                TextWrapping = TextWrapping.Wrap,
                ToolTip = (item.Description == "" || item.Description == null) ? "Description Not Available" : item.Description,


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
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 2, 5, 0),
                IsChecked = false,
                IsEnabled = false
            };



            TextBlock txtElapsedTime = new TextBlock
            {
                FontSize = 14,

                Text = (item.TimeElapsedValue == "" || item.TimeElapsedValue == null) ? "00:00:00" : item.TimeElapsedValue,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right

            };

            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            RowDefinition row1 = new RowDefinition
            {
                //MinHeight = 30,
                //Height = new GridLength(30, GridUnitType.Auto)
            };
            RowDefinition row2 = new RowDefinition
            {

                //MinHeight = 30,
                //Height = new GridLength(30, GridUnitType.Auto)
            };

            ColumnDefinition column1 = new ColumnDefinition
            {
                MinWidth = 150,
                //Width = new GridLength(2, GridUnitType.Star)
                Width = new GridLength(150, GridUnitType.Pixel)
            };

            ColumnDefinition column2 = new ColumnDefinition
            {
                MinWidth = 130,
                Width = new GridLength(130, GridUnitType.Pixel)
            };

            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);

            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);


            Grid.SetRow(txtProjectDesc, 0);
            Grid.SetColumn(txtProjectDesc, 0);

            Grid.SetRow(toggleButton, 0);
            Grid.SetColumn(toggleButton, 1);

            Grid.SetRow(txtElapsedTime, 1);
            Grid.SetColumn(txtElapsedTime, 1);


            grid.Children.Add(txtProjectDesc);
            grid.Children.Add(toggleButton);
            grid.Children.Add(txtElapsedTime);

            //grid.Measure(new Size(Double.MaxValue, Double.MaxValue));
            //Size visualSize = grid.DesiredSize;
            //grid.Arrange(new Rect(new Point(0, 0), visualSize));
            //grid.UpdateLayout();

            //double GridHeight = visualSize.Height + 1;
            //grid.Height = GridHeight;
            //double GridHeight = (row1.MinHeight * row1.Height.Value) + (row2.MinHeight * row2.Height.Value) + 5;
            //grid.Height = GridHeight;


            //grid.Measure(new Size(Double.MaxValue, Double.MaxValue));
            //Size visualSize = grid.DesiredSize;
            //grid.Arrange(new Rect(new Point(0, 0), visualSize));
            //grid.UpdateLayout();
            //row1.Height = new GridLength(visualSize.Height, GridUnitType.Auto);
            //double GridHeight = visualSize.Height + 1; ;
            //grid.Height = GridHeight;
            SetGridHeight(grid);
            return grid;
        }


        public static Grid CreateProjectDetailsGridForSelectedProject(Project item)
        {



            TextBlock txtProjectDesc = new TextBlock
            {
                FontSize = 14,
                Padding = new Thickness(5, 0, 0, 0),
                Text = (item.Description == "" || item.Description == null) ? "Description Not Available" : item.Description,
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
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 2, 5, 0),
                IsChecked = true,
                IsEnabled = false
            };



            TextBlock txtElapsedTime = new TextBlock
            {
                FontSize = 14,


                Text = (item.TimeElapsedValue == "" || item.TimeElapsedValue == null) ? "00:00:00" : item.TimeElapsedValue,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right

            };

            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            RowDefinition row1 = new RowDefinition
            {
                MinHeight = 30,
                Height = new GridLength(30, GridUnitType.Auto)
            };
            RowDefinition row2 = new RowDefinition
            {

                MinHeight = 30,
                Height = new GridLength(30, GridUnitType.Auto)
            };

            ColumnDefinition column1 = new ColumnDefinition
            {
                MinWidth = 150,
                // Width = new GridLength(2, GridUnitType.Star)
                Width = new GridLength(150, GridUnitType.Pixel)
            };

            ColumnDefinition column2 = new ColumnDefinition
            {
                MinWidth = 110,
                Width = new GridLength(110, GridUnitType.Pixel)
            };

            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);

            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);


            Grid.SetRow(txtProjectDesc, 0);
            Grid.SetColumn(txtProjectDesc, 0);

            Grid.SetRow(toggleButton, 0);
            Grid.SetColumn(toggleButton, 1);

            Grid.SetRow(txtElapsedTime, 1);
            Grid.SetColumn(txtElapsedTime, 1);


            grid.Children.Add(txtProjectDesc);
            grid.Children.Add(toggleButton);
            grid.Children.Add(txtElapsedTime);

            //double GridHeight = (row1.MinHeight * row1.Height.Value) + (row2.MinHeight * row2.Height.Value) + 5;
            //grid.Height = GridHeight;
            SetGridHeight(grid);
            return grid;
        }

        public static Grid CreateTaskHeadingGrid(ProjectTask task)
        {
            TextBlock txtName = new TextBlock
            {
                Name = "txtName_" + task.TaskId,
                Text = task.TaskName,
                Padding = new Thickness(5, 0, 0, 0),
                FontSize = 16,
                Foreground = new SolidColorBrush(Colors.Black),
                TextWrapping = TextWrapping.NoWrap,

                Visibility = Visibility.Visible,
                FontWeight = FontWeights.Medium,
                ToolTip = task.TaskName
            };
            TextBlock txtTaskCount = new TextBlock
            {
                Name = "txtTaskCount_" + task.TaskId,
                Text = "SubTask Count(" + task.SubTaskCount.ToString() + ")",
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.Black),
                TextWrapping = TextWrapping.NoWrap,
                Visibility = Visibility.Visible,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Stretch

            };

            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            ColumnDefinition column1 = new ColumnDefinition
            {
                Width = new GridLength(150, GridUnitType.Star)
            };
            ColumnDefinition column2 = new ColumnDefinition
            {
                Width = new GridLength(130, GridUnitType.Star)
            };

            RowDefinition row1 = new RowDefinition
            {
                MinHeight = 30,
                //Height = new GridLength(30, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row1);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);

            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);

            Grid.SetRow(txtTaskCount, 0);
            Grid.SetColumn(txtTaskCount, 1);

            //grid.Width = column1.Width.Value + column2.Width.Value;
            //double GridHeight = (row1.MinHeight * row1.Height.Value);
            //grid.Height = GridHeight;

            grid.Children.Add(txtName);
            grid.Children.Add(txtTaskCount);

            SetGridHeight(grid);
            return grid;
        }

        public static Grid CreateTaskDetailsGrid(ProjectTask item)
        {



            TextBlock txtProjectDesc = new TextBlock
            {
                FontSize = 14,
                Padding = new Thickness(5, 0, 0, 0),
                // Text = (item.Description == "" || item.Description == null) ? "Description Not Available" : item.Description,
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
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 2, 5, 0),
                IsChecked = false
            };




            TextBlock txtElapsedTime = new TextBlock
            {
                FontSize = 14,
                Text = (item.TimeElapsedValue == "" || item.TimeElapsedValue == null) ? "00:00:00" : item.TimeElapsedValue,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right

            };

            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            RowDefinition row1 = new RowDefinition
            {
                MinHeight = 30,
                //Height = new GridLength(30, GridUnitType.Auto)
            };
            RowDefinition row2 = new RowDefinition
            {

                MinHeight = 30,
                //Height = new GridLength(30, GridUnitType.Auto)
            };

            ColumnDefinition column1 = new ColumnDefinition
            {
                MinWidth = 150,
                Width = new GridLength(2, GridUnitType.Star)
            };

            ColumnDefinition column2 = new ColumnDefinition
            {
                MinWidth = 130,
                Width = new GridLength(1, GridUnitType.Star)
            };

            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);

            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);


            Grid.SetRow(txtProjectDesc, 0);
            Grid.SetColumn(txtProjectDesc, 0);

            Grid.SetRow(toggleButton, 0);
            Grid.SetColumn(toggleButton, 1);

            Grid.SetRow(txtElapsedTime, 1);
            Grid.SetColumn(txtElapsedTime, 1);


            grid.Children.Add(txtProjectDesc);
            grid.Children.Add(toggleButton);
            grid.Children.Add(txtElapsedTime);

            //double GridHeight = (row1.MinHeight * row1.Height.Value) + (row2.MinHeight * row2.Height.Value) + 5;
            //grid.Height = GridHeight;
            SetGridHeight(grid);
            return grid;
        }
        public static Grid CreateTaskDetailsGridForSelectedTask(ProjectTask item)
        {

            //Binding nameBinding = new Binding
            //{
            //    Source = item,
            //    Mode = BindingMode.TwoWay,
            //    NotifyOnSourceUpdated = true,
            //    NotifyOnTargetUpdated = true
            //};

            TextBlock txtProjectDesc = new TextBlock
            {
                FontSize = 14,
                Padding = new Thickness(5, 0, 0, 0),
                // Text = (item.Description == "" || item.Description == null) ? "Description Not Available" : item.Description,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 0, 5),
                VerticalAlignment = VerticalAlignment.Bottom,
                TextWrapping = TextWrapping.WrapWithOverflow

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
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 2, 5, 0),
                IsChecked = true
            };

            TextBlock txtElapsedTime = new TextBlock
            {
                FontSize = 14,
                Text = (item.TimeElapsedValue == "" || item.TimeElapsedValue == null) ? "00:00:00" : item.TimeElapsedValue,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right

            };

            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            RowDefinition row1 = new RowDefinition
            {
                MinHeight = 30,
                //Height = new GridLength(30, GridUnitType.Auto)
            };
            RowDefinition row2 = new RowDefinition
            {

                MinHeight = 30,
                //Height = new GridLength(30, GridUnitType.Auto)
            };

            ColumnDefinition column1 = new ColumnDefinition
            {
                MinWidth = 150,
                Width = new GridLength(2, GridUnitType.Star)
            };

            ColumnDefinition column2 = new ColumnDefinition
            {
                MinWidth = 130,
                Width = new GridLength(1, GridUnitType.Star)
            };

            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);

            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);


            Grid.SetRow(txtProjectDesc, 0);
            Grid.SetColumn(txtProjectDesc, 0);

            Grid.SetRow(toggleButton, 0);
            Grid.SetColumn(toggleButton, 1);

            Grid.SetRow(txtElapsedTime, 1);
            Grid.SetColumn(txtElapsedTime, 1);


            grid.Children.Add(txtProjectDesc);
            grid.Children.Add(toggleButton);
            grid.Children.Add(txtElapsedTime);

            //double GridHeight = (row1.MinHeight * row1.Height.Value) + (row2.MinHeight * row2.Height.Value) + 5;
            //grid.Height = GridHeight;
            SetGridHeight(grid);
            return grid;
        }

        private static void SetGridHeight(Grid grid)
        {
            grid.Measure(new Size(Double.MaxValue, Double.MaxValue));
            Size visualSize = grid.DesiredSize;
            grid.Arrange(new Rect(new Point(0, 0), visualSize));
            grid.UpdateLayout();

            double GridHeight = visualSize.Height + 1;
            grid.Height = GridHeight;
        }

    }
}
