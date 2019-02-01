using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QBA.Qutilize.ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //MainWindow = new MainWindow();
            //MainWindow.Closing += MainWindow_Closing;

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ShowAppWindow();
            _notifyIcon.Icon = ClientApp.Properties.Resources.AppIcon;
            _notifyIcon.Visible = true;
            CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowAppWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            MainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void ShowAppWindow()
        {
          
            //if (MainWindow.IsVisible)
            //{
            //    if (MainWindow.WindowState == WindowState.Minimized)
            //    {
            //        MainWindow.WindowState = WindowState.Normal;
            //    }
            //    MainWindow.Activate();
            //}
            //else
            //{
            //    MainWindow.Show();
            //}
        }
    }
}
