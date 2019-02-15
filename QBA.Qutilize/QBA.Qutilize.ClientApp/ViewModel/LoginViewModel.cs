using Newtonsoft.Json;
using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        private async  void AuthenticatUser()
        {

            try
            {
                if (ValidateInput())
                {
                   
                    var authenticateduser= await WebAPIHelper.CallWebApiForUserAuthentication(new User
                                                                                                   {
                        
                                                                                                       UserName = UserID,
                                                                                                       Password = UserPassword,
                                                                                                       IsActive = true
                                                                                                   });

                    if(authenticateduser != null && authenticateduser.ID !=0)
                    {
                        ConfigureDailyTaskViewModel(authenticateduser);
                        _loginView.Close();
                    }
                    else
                    {
                        MessageBox.Show("User id/password is not correct.");
                    }
                }
                else
                    MessageBox.Show("User id or password required.");
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private static void ConfigureDailyTaskViewModel(User user)
        {
            try
            {
                DailyTask dailyTask = new DailyTask();
                DailyTaskViewModel dailyTaskVM = new DailyTaskViewModel(dailyTask, user);

                dailyTask.DataContext = dailyTaskVM;
                dailyTask.Show();
            }
            catch (Exception)
            {

                throw;
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
