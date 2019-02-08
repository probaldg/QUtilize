using Newtonsoft.Json;
using QBA.Qutilize.Models;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace QBA.Qutilize.ClientApp.Helper
{
    public class WebAPIHelper
    {

        private const string LoginWebAPIRoutePath = "api/Account/Login/";
        private const string InsertStartTimeAPIRoutePath = "api/DailyTask/UpdateStartTime/";
        private const string UpdateEndTimeAPIRoutePath = "api/DailyTask/UpdateEndTime/";

        public static async Task<User> CallWebApiForUserAuthentication(User user)
        {
            try
            {
                HttpClient client = CreateHTTPClient();
                ByteArrayContent byteContent = ConvertToObjectToByte(user);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var completeApiAddress = client.BaseAddress + LoginWebAPIRoutePath;
                var response = client.PostAsync(completeApiAddress, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var JsonString = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonConvert.DeserializeObject<User>(JsonString);
                    return deserialized;
                }
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<DailyTaskModel> CallInserStartTimeWebApi(DailyTaskModel dailyTaskModel)
        {
            try
            {

                HttpClient client = CreateHTTPClient();
                ByteArrayContent byteContent = ConvertToObjectToByte(dailyTaskModel);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var completeApiAddress = client.BaseAddress + InsertStartTimeAPIRoutePath;

                var response = client.PostAsync(completeApiAddress, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var JsonString = await response.Content.ReadAsStringAsync();

                    var deserialized = JsonConvert.DeserializeObject<DailyTaskModel>(JsonString);
                    return deserialized;
                }
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async static Task<DailyTaskModel> UpdateEndTimeForTheCurrentWorkingProject(DailyTaskModel dailyTaskModel)
        {
            try
            {
                HttpClient client = CreateHTTPClient();
                ByteArrayContent byteContent = ConvertToObjectToByte(dailyTaskModel);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var completeApiAddress = client.BaseAddress + UpdateEndTimeAPIRoutePath;

                var response = client.PostAsync(completeApiAddress, byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    var JsonString = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonConvert.DeserializeObject<DailyTaskModel>(JsonString);

                    return deserialized;
                }
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private static HttpClient CreateHTTPClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri(GetAPIBaseAddress()),
            };
        }

        private static ByteArrayContent ConvertToObjectToByte<T>(T value)
        {
            try
            {
                var myContent = JsonConvert.SerializeObject(value);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                return byteContent;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private static string GetAPIBaseAddress()
        {
            return ConfigurationManager.AppSettings["WebApibaseAddress"];
        }
    }
}
