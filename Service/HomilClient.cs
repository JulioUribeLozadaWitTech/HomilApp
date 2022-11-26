using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HomilApp.Service
{
    internal class HomilClient
    {
        Uri urlBase = new Uri("http://104.208.216.173:8083");
        string token;
        public  HomilClient()
        {

        }
        
        public async Task<T> executeRequestGet<T>(string url, string stParams)
        {
            token = await SecureStorage.GetAsync("AccessToken");
            string requestUri = url + "?";
            var options = new RestClientOptions(urlBase);
            var client = new RestClient(options);
            RestRequest requestMessage = new RestRequest($"{requestUri}{stParams}", Method.Get) { Timeout = -1 };
            if (token != "") {
                requestMessage.AddHeader("Authorization", $"Bearer {token}");
            }
            requestMessage.AddHeader("Accept", "application/json");
            requestMessage.AddHeader("Content-Type", "application/json; charset=utf-8");
            RestResponse response = await client.ExecuteAsync(requestMessage); 

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content;

                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                return default(T);
            }
        }
        public async Task<T> executeRequestPost<T>(string url, string stParams, string token = "")
        {
            token = await SecureStorage.GetAsync("AccessToken");
            var client = new HttpClient();
            client.BaseAddress = urlBase;
            if (token != "")
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var content = new StringContent(stParams, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                return default(T);
            }
        }


    }
}
