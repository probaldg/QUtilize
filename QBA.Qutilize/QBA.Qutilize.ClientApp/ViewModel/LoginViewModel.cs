using QBA.Qutilize.ClientApp.Helper;
using QBA.Qutilize.ClientApp.Views;
using QBA.Qutilize.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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


        //private async void AuthenticatUser()
        //{

        //    try
        //    {
        //        if (ValidateInput())
        //        {
        //            StartLoadingAnimation();

        //            User authenticateduser = null;

        //            Logger.Log("AuthenticatUser", "Info", "Calling User authentication API");

        //            await Task.Run(() =>
        //            {
        //                authenticateduser = WebAPIHelper.CallWebApiForUserAuthentication(
        //                    new User { UserName = UserID, Password = UserPassword, IsActive = true }).Result;
        //            });

        //            StopLoadingAnimation();

        //            Logger.Log("AuthenticatUser", "Info", "successfully called authentication API");

        //            if (authenticateduser != null && authenticateduser.ID != 0)
        //            {
        //                Logger.Log("AuthenticatUser", "Info", $"User Authenticated user Name=  {authenticateduser.UserName}");
        //                ConfigureDailyTaskViewModel(authenticateduser);

        //                _loginView.Close();
        //            }
        //            else
        //            {
        //                Logger.Log("AuthenticatUser", "Info", "User authentication failed.");

        //                MessageBox.Show("User id/password is not correct.");
        //            }
        //        }
        //        else
        //            MessageBox.Show("User id or password required.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log("AuthenticatUser", "Error", $"{ex.ToString()}");

        //        //throw ex;
        //    }


        //}


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
                        //authenticateduser = WebAPIHelper.CallWebApiForUserAuthentication(
                        //new User { UserName = UserID, Password = UserPassword, IsActive = true }).Result;

                        string conStr = ConfigurationManager.ConnectionStrings["QBADBConnetion"].ConnectionString;
                        SqlConnection sqlCon = new SqlConnection(conStr);
                        DataTable dt = new DataTable();
                        var sqlCmd = new SqlCommand("USPUsers_Get", sqlCon)
                        {
                            CommandType = System.Data.CommandType.StoredProcedure
                        };
                        sqlCmd.Parameters.AddWithValue("@UserID", UserID);
                        sqlCmd.Parameters.AddWithValue("@Password", UserPassword);

                        SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            //CreateUser(dt)
                            User userModel = new User();
                            userModel.ID = Convert.ToInt32(dt.Rows[0]["Id"]); ;
                            userModel.Password = dt.Rows[0]["Password"].ToString();
                            userModel.Name = dt.Rows[0]["Name"].ToString();
                            userModel.UserName = dt.Rows[0]["UserName"].ToString();
                            userModel.CreateDate = Convert.ToDateTime(dt.Rows[0]["CreateDate"]);
                            userModel.CreatedBy = dt.Rows[0]["CreatedBy"].ToString();


                            //TODO need to check with dbnull value then assign the values to the fields
                            foreach (DataRow item in dt.Rows)
                            {
                                userModel.Projects.Add(new Project
                                {
                                    ProjectName = item["ProjectName"].ToString(),
                                    ProjectID = Convert.ToInt32(item["ProjectID"]),
                                    ParentProjectID = item["ParentProjectId"] != DBNull.Value ? Convert.ToInt32(item["ParentProjectId"]) : (int?)null,
                                    Description = item["ProjectDescription"].ToString(),
                                    DifferenceInSecondsInCurrentDate = item["DifferenceInSecondsInCurrentDate"] != DBNull.Value ? Convert.ToInt32(item["DifferenceInSecondsInCurrentDate"]) : (int?)null,
                                    MaxProjectTimeInHours = item["MaxProjectTimeInHours"] != DBNull.Value ? Convert.ToInt32(item["MaxProjectTimeInHours"]) : 0,
                                });

                                if (item["RoleID"] != null)
                                {
                                    userModel.Roles.Add(new Roles
                                    {
                                        Id = Convert.ToInt32(item["RoleID"]),
                                        Name = item["RoleName"].ToString()
                                    });
                                }
                            }

                            authenticateduser = userModel;
                        }

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
