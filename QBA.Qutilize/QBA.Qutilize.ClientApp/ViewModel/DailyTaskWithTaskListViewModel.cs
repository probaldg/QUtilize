using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System.Windows.Controls;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class DailyTaskWithTaskListViewModel
    {
        public void CreateProjectListControls(NewDailyTask view, User user)
        {
            if (user == null)
            {
                throw new System.ArgumentNullException(nameof(user));
            }

            StackPanel stackPanelMain = new StackPanel();
            stackPanelMain.Orientation = Orientation.Vertical;
            stackPanelMain.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            if (user.Projects.Count > 0)
            {

                foreach (var item in user.Projects)
                {
                    Canvas myCanvas = new Canvas();
                    myCanvas.Height = 30;

                    //myCanvas.Background = S Brushes.LightSteelBlue;

                    // Add a "Hello World!" text element to the Canvas
                    TextBlock txt1 = new TextBlock();
                    txt1.FontSize = 14;
                    txt1.Text = item.ProjectName;
                    Canvas.SetTop(txt1, 100);
                    Canvas.SetLeft(txt1, 10);
                    myCanvas.Children.Add(txt1);

                    //// Add a second text element to show how absolute positioning works in a Canvas
                    //TextBlock txt2 = new TextBlock();
                    //txt2.FontSize = 22;
                    //txt2.Text = "Isn't absolute positioning handy?";
                    //Canvas.SetTop(txt2, 200);
                    //Canvas.SetLeft(txt2, 75);
                    //myCanvas.Children.Add(txt2);
                    stackPanelMain.Children.Add(myCanvas);
                    //mainWindow.Title = "Canvas Sample";
                    //mainWindow.Show();


                }
            }

            var grid = (Grid)view.FindName("grdProject");
            grid.Children.Add(stackPanelMain);
        }
    }


}
