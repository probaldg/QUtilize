﻿using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace QBA.Qutilize.ClientApp.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private Login _loginView;
        public LoginViewModel(Login login)
        {
            _loginView = login;

        }
        private string _userID;
        private string _password;
        public string UserID
        {
            get { return _userID; }
            set
            {
                _userID = value;
                OnPropertyChanged("UserID");
            }
        }
        public string UserPassword
        {
            get
            {
                return ConvertToMD5(_password);
            }
            set
            {
                _password = value;
                OnPropertyChanged("UserPassword");
            }
        }

        public ICommand Login
        {
            get
            {
                return new CommandHandler(_ => AuthenticatUser());
            }
        }


        private async void AuthenticatUser()
        {

            try
            {
                if (ValidateInput())
                {
                    StartLoadingAnimation();

                    User authenticateduser = null;

                    Logger.Log("AuthenticatUser", "Info", "Calling User authentication API");

                    await Task.Run(() =>
                    {
                        authenticateduser = WebAPIHelper.CallWebApiForUserAuthentication(
                            new User { UserName = UserID, Password = UserPassword, IsActive = true }).Result;
                    });

                    StopLoadingAnimation();

                    Logger.Log("AuthenticatUser", "Info", "successfully called authentication API");

                    if (authenticateduser != null && authenticateduser.ID != 0)
                    {
                        Logger.Log("AuthenticatUser", "Info", $"User Authenticated user Name=  {authenticateduser.UserName}");
                        ConfigureDailyTaskViewModel(authenticateduser);

                        _loginView.Close();
                    }
                    else
                    {
                        Logger.Log("AuthenticatUser", "Info", "User authentication failed.");

                        MessageBox.Show("User id/password is not correct.");
                    }
                }
                else
                    MessageBox.Show("User id or password required.");
            }
            catch (Exception ex)
            {
                Logger.Log("AuthenticatUser", "Error", $"{ex.ToString()}");

                //throw ex;
            }


        }

        private void StopLoadingAnimation()
        {
            ((TextBlock)_loginView.FindName("Wait")).Visibility = Visibility.Collapsed;
            ((Storyboard)_loginView.FindResource("WaitStoryboard")).Stop();
        }

        private void StartLoadingAnimation()
        {
            ((TextBlock)_loginView.FindName("Wait")).Visibility = Visibility.Visible;
            ((Storyboard)_loginView.FindResource("WaitStoryboard")).Begin();
        }

        private static void ConfigureDailyTaskViewModel(User user)
        {
            try
            {
                DailyTask dailyTask = new DailyTask();
                DailyTaskViewModel dailyTaskVM = new DailyTaskViewModel(dailyTask, user);

                dailyTask.DataContext = dailyTaskVM;

                Application.Current.MainWindow = dailyTask;
                dailyTask.Show();
                dailyTask.Activate();
            }
            catch (Exception ex)
            {
                Logger.Log("ConfigureDailyTaskViewModel", "Error", $"{ex.ToString()}");
                throw ex;
            }

        }

        private bool ValidateInput()
        {
            if (UserID == null)
                return false;
            if (UserPassword == null)
                return false;

            return true;
        }

        private string ConvertToMD5(string password)
        {
            return EncryptionHelper.ConvertStringToMD5(password);
        }

    }
}
