using QBA.Qutilize.ClientApp.ViewModel;
using System.Windows;

namespace QBA.Qutilize.ClientApp.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        LoginViewModel _vm;
        public Login()
        {
          
            InitializeComponent();

            SetWindowToBottomRightOfScreen();

            _vm = new LoginViewModel(this);
           this.DataContext = _vm;
        }

        private void SetWindowToBottomRightOfScreen()
        {
            Left = SystemParameters.WorkArea.Width - Width - 10;
            Top = SystemParameters.WorkArea.Height - Height;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            this.Hide();
        }
    }
}
