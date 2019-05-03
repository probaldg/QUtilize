using QBA.Qutilize.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.Helper
{
    public class SubTaskViewHelper
    {
        public Border CreateTemplateForSubTask(List<ProjectTask> subTasklist, string[] backgroundColorArray)
        {

            var headerControl = CreateControlForTaskHeadingCanvas(subTasklist[0]);
            var backgroundColor = backgroundColorArray[subTasklist[0].TaskDepthLevel].ToString();
            headerControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundColor));

            //Create body which will contain the sub task list
            StackPanel subTaskStack = new StackPanel();
            foreach (var item in subTasklist)
            {
                Canvas canvas = new Canvas();
                var backColor = backgroundColorArray[item.TaskDepthLevel + 1].ToString();
                canvas = CreateControlForTaskItem(item, backColor);
                Border border = CreateBorderForProjectCanvas();
                border.Child = canvas;
                subTaskStack.Children.Add(border);
            }

            //Stackpanel to contain both heading and body..
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(headerControl);
            stackPanel.Children.Add(subTaskStack);
            stackPanel.Height = headerControl.Height + subTaskStack.Height;


            //Canvas myCanvas = CreateCanvasPanel(task);
            //myCanvas.Width = 281;
            //myCanvas.Height = stackPanel.Height;

            //Canvas.SetTop(stackPanel, 0);
            //Canvas.SetLeft(stackPanel, 0);

            //myCanvas.Children.Add(stackPanel);

            Border brdr = CreateBorderForProjectCanvas();
            brdr.Margin = new Thickness(0);
            brdr.BorderThickness = new Thickness(1, 1, 1, 1);

            brdr.BorderBrush = new SolidColorBrush(Colors.Black);

            brdr.Child = stackPanel;
            return brdr;

        }

        public Canvas CreateControlForTaskHeadingCanvas(ProjectTask projectTask)
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
                Text = item.ParentTaskName,
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


        public Canvas CreateControlForTaskItem(ProjectTask item, string backColor)
        {
            Canvas myCanvas = CreateCanvasPanel(item);
            myCanvas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));
            myCanvas.Width = 276;

            //myCanvas.MouseLeftButtonDown += MyCanvas_MouseLeftButtonDown; ;
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

        public Border CreateBorderForProjectCanvas()
        {
            return new Border()
            {
                BorderThickness = new Thickness()
                {
                    Bottom = 1,
                    Left = 0,
                    Right = 0,
                    Top = 0
                },
                BorderBrush = new SolidColorBrush(Colors.Green),

                Margin = new Thickness(0)
            };
        }
    }
}
