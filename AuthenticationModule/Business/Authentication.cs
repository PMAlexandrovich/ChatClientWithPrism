using CommonServiceLocator;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace AuthenticationModule.Business
{
    public class Authentication
    {
        HttpClient _httpClient;
        public Authentication(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterAsync(string username, string password)
        {
            var values = new Dictionary<string, string>()
            {
                {"username",username },
                {"password", password }
            };

            var request = new FormUrlEncodedContent(values);

            return await _httpClient.PostAsync(@"\reg", request);

        }

        public async Task<HttpResponseMessage>LoginAsync(string username, string password)
        {
            var values = new Dictionary<string, string>()
            {
                {"username",username },
                {"password", password }
            };

            var request = new FormUrlEncodedContent(values);
            var result = await _httpClient.PostAsync(@"token", request);
            if (result.IsSuccessStatusCode)
            {
                var appData = ServiceLocator.Current.GetInstance<AppData>();
                appData.UserName = username;
            }
            return result;
        }

        public void SetAccessToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var appData = ServiceLocator.Current.GetInstance<AppData>();
            appData.Token = token;
        }
    }
}
