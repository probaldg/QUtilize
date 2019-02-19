using QBA.Qutilize.ClientApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QBA.Qutilize.ClientApp.Views
{
    /// <summary>
    /// Interaction logic for DailyTask.xaml
    /// </summary>
    public partial class DailyTask : Window
    {
        //DailyTaskViewModel _vm;
        public DailyTask()
        {
            InitializeComponent();
            SetWindowToBottomRightOfScreen();

            //_vm = new DailyTaskViewModel(this);
            //this.DataContext = _vm;
        }

        private void SetWindowToBottomRightOfScreen()
        {
            Left = SystemParameters.WorkArea.Width - Width - 10;
            Top = SystemParameters.WorkArea.Height - Height;
        }
    }
}
