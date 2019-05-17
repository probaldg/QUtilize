using QBA.Qutilize.ClientApp.Helper.WPFControlHelper;
using QBA.Qutilize.Models;
using System.Windows.Controls;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.Helper.ViewHelper
{
    public class TaskViewHelper
    {
        public Canvas CreateTaskHeadingControl(ProjectTask task, string backColor)
        {
            Canvas myCanvas = CanvasControlHelper.CreateCanvas();

            myCanvas.DataContext = task;
            myCanvas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));

            Grid grid = WPFGridHelper.CreateTaskHeadingGrid(task);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;
            myCanvas.Children.Add(grid);
            return myCanvas;

        }
        public Canvas CreateTaskHeadingControlForSelectedControl(ProjectTask task, string backColor)
        {
            Canvas myCanvas = CanvasControlHelper.CreateCanvasForSelectedControl();

            myCanvas.DataContext = task;
            myCanvas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));

            Grid grid = WPFGridHelper.CreateTaskHeadingGrid(task);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;
            myCanvas.Children.Add(grid);
            return myCanvas;

        }

        private Canvas CreateTaskDetailsControl(ProjectTask task)
        {
            Canvas myCanvas = CanvasControlHelper.CreateCanvasForSelectedControl();
            ////myCanvas.Width = 350;
            myCanvas.Background = new SolidColorBrush(Colors.White);

            Grid grid = WPFGridHelper.CreateTaskDetailsGrid(task);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;

            myCanvas.Children.Add(grid);
            return myCanvas;
        }

        private Canvas CreateTaskDetailsControlForSelectedProject(ProjectTask task)
        {
            Canvas myCanvas = CanvasControlHelper.CreateCanvasForSelectedControl();
            //myCanvas.Width = 350;
            myCanvas.Background = new SolidColorBrush(Colors.White);

            Grid grid = WPFGridHelper.CreateTaskDetailsGridForSelectedTask(task);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;

            myCanvas.Children.Add(grid);
            return myCanvas;
        }

        public Canvas CreateProjectHeadingControl(Project project)
        {
            Canvas myCanvas = CanvasControlHelper.CreateCanvasForSelectedControl();
            //myCanvas.Width = 350;
            myCanvas.DataContext = project;
            myCanvas.Background = new SolidColorBrush(Colors.Black);

            Grid grid = WPFGridHelper.CreateProjectHeadingGrid(project);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;
            myCanvas.Children.Add(grid);
            return myCanvas;

        }

        public Canvas CreateTaskViewControl(ProjectTask task, string backColor)
        {
            if (task.SubTaskCount != 0)
            {
                var header = CreateTaskHeadingControlForSelectedControl(task, backColor);
                //header.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));
                return header;
            }

            var headerControl = CreateTaskHeadingControlForSelectedControl(task, backColor);
            headerControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));

            var bodyControl = CreateTaskDetailsControl(task);
            bodyControl.Background = new SolidColorBrush(Colors.White);


            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(headerControl);
            stackPanel.Children.Add(bodyControl);


            Canvas myCanvas = CanvasControlHelper.CreateCanvas();
            myCanvas.Height = headerControl.Height + bodyControl.Height;
            myCanvas.DataContext = task;

            Canvas.SetTop(stackPanel, 0);
            Canvas.SetLeft(stackPanel, 0);

            myCanvas.Children.Add(stackPanel);
            return myCanvas;
        }

        public Canvas CreateTaskViewControlForSelectedTask(ProjectTask task, string backColor)
        {
            if (task.SubTaskCount != 0)
            {

                var header = CreateTaskHeadingControlForSelectedControl(task, backColor);
                //header.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));
                return header;
            }

            var headerControl = CreateTaskHeadingControlForSelectedControl(task, backColor);
            headerControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backColor));

            var bodyControl = CreateTaskDetailsControlForSelectedProject(task);
            bodyControl.Background = new SolidColorBrush(Colors.White);


            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(headerControl);
            stackPanel.Children.Add(bodyControl);


            Canvas myCanvas = CanvasControlHelper.CreateCanvas();
            myCanvas.Height = headerControl.Height + bodyControl.Height;
            myCanvas.DataContext = task;
            Canvas.SetTop(stackPanel, 0);
            Canvas.SetLeft(stackPanel, 0);

            myCanvas.Children.Add(stackPanel);
            return myCanvas;
        }
    }
}
