﻿using QBA.Qutilize.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.Helper.WPFControlHelper
{
    public static class WPFGridHelper
    {
        public static Grid CreateProjectHeadingGrid(Project project)
        {
            TextBlock txtName = new TextBlock
            {
                Name = "txtName_" + project.ProjectID,
                Text = project.ProjectName,
                FontSize = 16,
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.NoWrap,
                Padding = new Thickness(1, 1, 1, 1),
                Visibility = Visibility.Visible,
                FontWeight = FontWeights.Medium,
                ToolTip = project.ProjectName
            };
            TextBlock txtTaskCount = new TextBlock
            {
                Name = "txtTaskCount_" + project.ProjectID,
                Text = "Task Count(" + project.TaskCount.ToString() + ")",
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap,
                Visibility = Visibility.Visible,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = double.NaN,
                MinHeight = 30

            };

            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            ColumnDefinition column1 = new ColumnDefinition
            {
                Width = new GridLength(200, GridUnitType.Star)
            };
            ColumnDefinition column2 = new ColumnDefinition
            {
                Width = new GridLength(140, GridUnitType.Star)
            };

            RowDefinition row1 = new RowDefinition
            {
                MinHeight = 30,
                Height = new GridLength(30, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row1);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);

            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);

            Grid.SetRow(txtTaskCount, 0);
            Grid.SetColumn(txtTaskCount, 1);

            grid.Width = column1.Width.Value + column2.Width.Value;
            double GridHeight = (row1.MinHeight * row1.Height.Value) + 1;
            grid.Height = GridHeight;

            grid.Children.Add(txtName);
            grid.Children.Add(txtTaskCount);
            return grid;
        }

        public static Grid CreateProjectDetailsGrid(Project item)
        {

            Binding nameBinding = new Binding
            {
                Source = item,
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true
            };

            TextBlock txtProjectDesc = new TextBlock
            {
                FontSize = 14,
                Padding = new Thickness(5, 0, 0, 0),
                Text = (item.Description == "" || item.Description == null) ? "Description Not Available" : item.Description,
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
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 2, 5, 0),
                IsChecked = false
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
                FontSize = 14,

                Text = item.TimeElapsedValue,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center

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
                MinWidth = 200,
                Width = new GridLength(2, GridUnitType.Star)
            };

            ColumnDefinition column2 = new ColumnDefinition
            {
                MinWidth = 140,
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

            double GridHeight = (row1.MinHeight * row1.Height.Value) + (row2.MinHeight * row2.Height.Value) + 10;
            grid.Height = GridHeight;
            return grid;
        }

        public static Grid CreateProjectDetailsGridForSelectedProject(Project item)
        {

            Binding nameBinding = new Binding
            {
                Source = item,
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true
            };

            TextBlock txtProjectDesc = new TextBlock
            {
                FontSize = 14,
                Padding = new Thickness(5, 0, 0, 0),
                Text = (item.Description == "" || item.Description == null) ? "Description Not Available" : item.Description,
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

                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 2, 5, 0),
                IsChecked = true
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
                FontSize = 14,

                Text = item.TimeElapsedValue,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center

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
                MinWidth = 200,
                Width = new GridLength(2, GridUnitType.Star)
            };

            ColumnDefinition column2 = new ColumnDefinition
            {
                MinWidth = 140,
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

            double GridHeight = (row1.MinHeight * row1.Height.Value) + (row2.MinHeight * row2.Height.Value) + 10;
            grid.Height = GridHeight;
            return grid;
        }


        public static Grid CreateTaskHeadingGrid(ProjectTask task)
        {
            TextBlock txtName = new TextBlock
            {
                Name = "txtName_" + task.TaskId,
                Text = task.TaskName,
                FontSize = 16,
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.NoWrap,
                Padding = new Thickness(1, 1, 1, 1),
                Visibility = Visibility.Visible,
                FontWeight = FontWeights.Medium,
                ToolTip = task.TaskName
            };
            TextBlock txtTaskCount = new TextBlock
            {
                Name = "txtTaskCount_" + task.TaskId,
                Text = "SubTask Count(" + task.SubTaskCount.ToString() + ")",
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap,
                Visibility = Visibility.Visible,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = double.NaN,
                MinHeight = 30

            };

            Grid grid = new Grid();
            grid.Margin = new Thickness(0);

            ColumnDefinition column1 = new ColumnDefinition
            {
                Width = new GridLength(200, GridUnitType.Star)
            };
            ColumnDefinition column2 = new ColumnDefinition
            {
                Width = new GridLength(140, GridUnitType.Star)
            };

            RowDefinition row1 = new RowDefinition
            {
                MinHeight = 30,
                Height = new GridLength(30, GridUnitType.Auto)
            };
            grid.RowDefinitions.Add(row1);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);

            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);

            Grid.SetRow(txtTaskCount, 0);
            Grid.SetColumn(txtTaskCount, 1);

            grid.Width = column1.Width.Value + column2.Width.Value;
            double GridHeight = (row1.MinHeight * row1.Height.Value) + 1;
            grid.Height = GridHeight;

            grid.Children.Add(txtName);
            grid.Children.Add(txtTaskCount);
            return grid;
        }

        public static Grid CreateTaskDetailsGrid(ProjectTask item)
        {

            Binding nameBinding = new Binding
            {
                Source = item,
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true
            };

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
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 2, 5, 0),
                IsChecked = false
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
                MinWidth = 200,
                Width = new GridLength(2, GridUnitType.Star)
            };

            ColumnDefinition column2 = new ColumnDefinition
            {
                MinWidth = 140,
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

            double GridHeight = (row1.MinHeight * row1.Height.Value) + (row2.MinHeight * row2.Height.Value) + 10;
            grid.Height = GridHeight;
            return grid;
        }
        public static Grid CreateTaskDetailsGridForSelectedTask(ProjectTask item)
        {

            Binding nameBinding = new Binding
            {
                Source = item,
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true
            };

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
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 2, 5, 0),
                IsChecked = true
            };

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
                MinWidth = 200,
                Width = new GridLength(2, GridUnitType.Star)
            };

            ColumnDefinition column2 = new ColumnDefinition
            {
                MinWidth = 140,
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

            double GridHeight = (row1.MinHeight * row1.Height.Value) + (row2.MinHeight * row2.Height.Value) + 10;
            grid.Height = GridHeight;
            return grid;
        }
    }
}
