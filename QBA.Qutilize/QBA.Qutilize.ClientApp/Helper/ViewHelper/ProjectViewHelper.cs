using QBA.Qutilize.ClientApp.Helper.WPFControlHelper;
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
            myCanvas.Width = 350;
            myCanvas.DataContext = project;
            myCanvas.Background = new SolidColorBrush(Colors.Black);

            Grid grid = WPFGridHelper.CreateProjectHeadingGrid(project);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;
            myCanvas.Children.Add(grid);
            return myCanvas;

        }

        private Canvas CreateProjectDetailsControl(Project project)
        {
            Canvas myCanvas = CanvasControlHelper.CreateCanvas();
            myCanvas.Width = 350;
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
            myCanvas.Width = 350;
            myCanvas.Background = new SolidColorBrush(Colors.White);

            Grid grid = WPFGridHelper.CreateProjectDetailsGridForSelectedProject(project);
            Canvas.SetTop(grid, 0);
            Canvas.SetLeft(grid, 0);

            myCanvas.Height = grid.Height;

            myCanvas.Children.Add(grid);
            return myCanvas;
        }

        public Canvas CreateProjectViewControl(Project project)
        {
            if (project.TaskCount != 0)
            {
                return CreateProjectHeadingControl(project);
            }

            var headerControl = CreateProjectHeadingControl(project);

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

        public Canvas CreateProjectViewControlForSelectedProject(Project project)
        {
            if (project.TaskCount != 0)
            {
                return CreateProjectHeadingControl(project);
            }

            var headerControl = CreateProjectHeadingControl(project);

            var bodyControl = CreateProjectDetailsControlForSelectedProject(project);
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
    }
}
