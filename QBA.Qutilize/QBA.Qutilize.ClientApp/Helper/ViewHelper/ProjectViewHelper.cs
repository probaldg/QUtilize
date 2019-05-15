﻿using QBA.Qutilize.ClientApp.Helper.WPFControlHelper;
using QBA.Qutilize.Models;
using System.Windows.Controls;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.Helper.ViewHelper
{
    public class ProjectViewHelper
    {
        private Canvas CreateProjectHeadingControl(Project project)
        {
            Canvas myCanvas = CanvasControlHelper.CreateCanvas();

            myCanvas.DataContext = project;
            // myCanvas.Background = new SolidColorBrush(Colors.Black);
            Grid grid = WPFGridHelper.CreateProjectHeadingGrid(project);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Children.Add(grid);
            myCanvas.Height = grid.Height;
            return myCanvas;

        }

        private Canvas CreateProjectDetailsControl(Project project)
        {
            Canvas myCanvas = CanvasControlHelper.CreateCanvas();
            myCanvas.Background = new SolidColorBrush(Colors.White);

            Grid grid = WPFGridHelper.CreateProjectDetailsGrid(project);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;

            myCanvas.Children.Add(grid);
            return myCanvas;
        }

        private Canvas CreateProjectDetailsControlForSelectedProject(Project project)
        {
            Canvas myCanvas = CanvasControlHelper.CreateCanvas();
            myCanvas.Background = new SolidColorBrush(Colors.White);

            Grid grid = WPFGridHelper.CreateProjectDetailsGridForSelectedProject(project);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);
            myCanvas.Height = grid.Height;

            myCanvas.Children.Add(grid);
            return myCanvas;
        }

        public Canvas CreateProjectViewControl(Project project, string backcolor)
        {
            if (project.TaskCount != 0)
            {

                var header = CreateProjectHeadingControl(project);
                header.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backcolor));

                header.Height = 50;
                return header;
            }

            var headerControl = CreateProjectHeadingControl(project);
            headerControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backcolor));

            var bodyControl = CreateProjectDetailsControl(project);
            bodyControl.Background = new SolidColorBrush(Colors.White);


            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(headerControl);
            stackPanel.Children.Add(bodyControl);


            Canvas myCanvas = CanvasControlHelper.CreateCanvas();
            myCanvas.Height = headerControl.Height + bodyControl.Height;
            myCanvas.DataContext = project;
            Canvas.SetTop(stackPanel, 0);
            Canvas.SetLeft(stackPanel, 0);

            myCanvas.Children.Add(stackPanel);
            return myCanvas;
        }

        public Canvas CreateProjectViewControlForSelectedProject(Project project, string backcolor)
        {
            if (project.TaskCount != 0)
            {
                var header = CreateProjectHeadingControl(project);
                header.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backcolor));
                header.Height = 50;
                header.Width = 265;
                return header;
            }

            var headerControl = CreateProjectHeadingControl(project);
            headerControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backcolor));

            headerControl.Width = 265;

            var bodyControl = CreateProjectDetailsControlForSelectedProject(project);
            bodyControl.Background = new SolidColorBrush(Colors.White);
            bodyControl.Width = 265;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(headerControl);
            stackPanel.Children.Add(bodyControl);


            Canvas myCanvas = CanvasControlHelper.CreateCanvas();
            myCanvas.Height = headerControl.Height + bodyControl.Height;
            myCanvas.Width = 265;
            myCanvas.DataContext = project;
            Canvas.SetTop(stackPanel, 0);
            Canvas.SetLeft(stackPanel, 0);

            myCanvas.Children.Add(stackPanel);
            return myCanvas;
        }
    }
}
