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

        private bool _loggedIn;

        public bool LoggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; }
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
                   
                    var authenticateduser= await CallWebApi(new User {
                        UserName = UserID,
                        Password = UserPassword,
                        IsActive = true
                    });

                    if(authenticateduser != null)
                    {
                      
                        DailyTask dailyTask = new DailyTask();
                        dailyTask.Show();
                        _loginView.Close();

                    }
                    else
                    {
                        MessageBox.Show("User id or password mismatch.");
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
              
        private static async Task<User> CallWebApi(User user)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApibaseAddress"])
            };

            var completeApiAddress = ConfigurationManager.AppSettings["WebApibaseAddress"] + Properties.Resources.LoginWebAPIRoutePath;

            var myContent = JsonConvert.SerializeObject(user);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response =  client.PostAsync(completeApiAddress, byteContent).Result;


            if (response.IsSuccessStatusCode)
            {
                var JsonString = await response.Content.ReadAsStringAsync();

                var deserialized = JsonConvert.DeserializeObject<User>(JsonString);
                return deserialized;
            }
            else
                return null;
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
