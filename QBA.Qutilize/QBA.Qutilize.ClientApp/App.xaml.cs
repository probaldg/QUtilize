﻿using QBA.Qutilize.ClientApp.Helper;
using System;
using System.Threading;
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
        Mutex m;


        public App()
        {
            m = new Mutex(true, "QBA.Qutilize.ClientApp", out bool isnew);
            if (!isnew)
            {
                MessageBox.Show(string.Format("Another instance of QUtilize is already running.{0} " +
                    " Kindly close all running instances and try again.", Environment.NewLine), " Qutilize " +
                    " Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                _notifyIcon = new System.Windows.Forms.NotifyIcon();
                _notifyIcon.DoubleClick += (s, args) => ShowAppWindow();
                _notifyIcon.Icon = ClientApp.Properties.Resources.qba_icon;
                _notifyIcon.Visible = true;
                CreateContextMenu();
                Logger.Log("OnStartup", "Info", $"Application launched successfully.");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Open").Click += (s, e) => ShowAppWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }


        private void ExitApplication()
        {
            Logger.Log("ExitApplication", "Info", $"Exiting application.");
            _isExit = true;
            try
            {
                LogOutAndCloseApplication();

                MainWindow.Close();
                _notifyIcon.Dispose();
                _notifyIcon = null;
                Application.Current.Shutdown(99);
            }
            catch (Exception)
            {
                MessageBox.Show("Some error occured.");
                // throw;
            }

        }

        private void LogOutAndCloseApplication()
        {
            try
            {
                var vm = MainWindow.DataContext;

                if (vm.GetType().Name == "DailyTaskViewModel")
                {
                    ((QBA.Qutilize.ClientApp.ViewModel.DailyTaskViewModel)vm).LogoutUser();

                }
            }
            catch (Exception ex)
            {
                Logger.Log("LogoutUser", "Error", ex.ToString());
                // throw;
            }

        }

        private void ShowAppWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                if (MainWindow.IsActive == false)
                    MainWindow.Activate();

                if (MainWindow.Visibility == Visibility.Hidden)
                    MainWindow.Show();

            }
        }


    }
}
