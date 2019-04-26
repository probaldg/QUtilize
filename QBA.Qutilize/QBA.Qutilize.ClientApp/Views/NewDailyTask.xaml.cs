﻿using System.Windows;

namespace QBA.Qutilize.ClientApp.Views
{
    /// <summary>
    /// Interaction logic for NewDailyTask.xaml
    /// </summary>
    public partial class NewDailyTask : Window
    {
        public NewDailyTask()
        {
            InitializeComponent();
            SetWindowToBottomRightOfScreen();
        }

        private void SetWindowToBottomRightOfScreen()
        {
            Left = SystemParameters.WorkArea.Width - Width - 10;
            Top = SystemParameters.WorkArea.Height - Height;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Application.Current.MainWindow = this;

            e.Cancel = true;
            this.Hide();
        }

        //private void Window_Deactivated(object sender, EventArgs e)
        //{

        //    this.Hide();
        //}
    }
}
