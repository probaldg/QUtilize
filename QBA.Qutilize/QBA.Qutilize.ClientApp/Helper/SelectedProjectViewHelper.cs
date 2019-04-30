using QBA.Qutilize.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.Helper
{
    public class SelectedProjectViewHelper
    {
        readonly string[] ColorArray = new string[] { "#262626", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7", "#194C66", "#1F5D7C", "#246E93", "#2A7FAA", "#3090C0", "#3E9ECE", "#55AAD4", "#6BB5DA", "#82C0DF" };

        public Border CreateTemplateForProjectAtLastLevel(Project project)
        {
            var headerControl = CreateControlForProjectCanvas(project);
            headerControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorArray[0].ToString()));

            var bodyControl = CreateControlForSelectedProjctItem(project);
            bodyControl.Background = new SolidColorBrush(Colors.White);

            bodyControl.MouseLeftButtonDown += BodyControl_MouseLeftButtonDown;
            bodyControl.MouseEnter += BodyControl_MouseEnter;
            bodyControl.MouseLeave += BodyControl_MouseLeave;


            //Stackpanel to contain both heading and body..
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(headerControl);
            stackPanel.Children.Add(bodyControl);

            stackPanel.Height = headerControl.Height + bodyControl.Height;


            Canvas myCanvas = CreateCanvasPanel(project);
            myCanvas.Width = 281;
            myCanvas.Height = stackPanel.Height;

            Canvas.SetTop(stackPanel, 0);
            Canvas.SetLeft(stackPanel, 0);

            myCanvas.Children.Add(stackPanel);

            Border brdr = CreateBorderForSelectedProjectCanvas();


            brdr.Child = myCanvas;
            return brdr;
        }

        private void BodyControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // throw new NotImplementedException();
            MessageBox.Show("test");
        }

        private void BodyControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            /// throw new NotImplementedException();
        }

        private void BodyControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private Canvas CreateCanvasPanel(Project item)
        {
            return new Canvas
            {
                Margin = new Thickness(0),
                //  Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorArray[0].ToString())),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 281,
                MinHeight = 30,
                DataContext = item

            };
        }
        private static Grid CreateGridForSelectedProject(Project item)
        {
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


            Grid.SetRow(txtProjectDesc, 0);
            Grid.SetColumn(txtProjectDesc, 0);

            Grid.SetRow(toggleButton, 0);
            Grid.SetColumn(toggleButton, 1);

            Grid.SetRow(txtElapsedTime, 1);
            Grid.SetColumn(txtElapsedTime, 1);


            grid.Children.Add(txtProjectDesc);
            grid.Children.Add(toggleButton);
            grid.Children.Add(txtElapsedTime);

            double GridHeight = row1.Height.Value + row2.Height.Value;
            grid.Height = GridHeight;
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

        private Canvas CreateControlForProjectCanvas(Project item)
        {
            Canvas myCanvas = CreateCanvasPanel(item);
            myCanvas.Width = 281;

            Grid grid = CreateGridForHeading(item);

            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;

            myCanvas.Children.Add(grid);
            return myCanvas;
        }

        private Canvas CreateControlForSelectedProjctItem(Project item)
        {
            Canvas myCanvas = CreateCanvasPanel(item);
            myCanvas.Background = new SolidColorBrush(Colors.White);

            Grid grid = CreateGridForSelectedProject(item);

            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;
            myCanvas.Children.Add(grid);


            return myCanvas;

        }

        private static Grid CreateGridForHeading(Project item)
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
                FontSize = 18,
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
    }
}
