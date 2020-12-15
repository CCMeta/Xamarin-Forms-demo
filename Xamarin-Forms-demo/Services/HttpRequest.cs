using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http.Headers;

namespace Xamarin_Forms_demo.Services
{
    public class HttpRequest
    {
        public readonly string _host;
        private static string token = string.Empty;
        public static string Token { get => token; set => token = value; }
        public readonly HttpClient _httpClient = new HttpClient();

        public HttpRequest(string host)
        {
            _host = host;
        }

        public void Login(string username, string password)
        {
            var identity = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };
            var user = Task.Run(async () =>
                 await PostAsync<Dictionary<string, string>>("/api/token", identity)
            ).Result;
            if (!string.IsNullOrEmpty(user["token"]))
            {
                Token = user["token"];
                return;
            }
            throw new Exception($"No token responsed result = {user}");
        }

        public async Task<T> GetAsync<T>(string path, Dictionary<string, string> queryParams)
        {
            using var content = new FormUrlEncodedContent(queryParams);
            var query = content.ReadAsStringAsync().Result;
            var uri = _host + path + "?" + query;
            if (!string.IsNullOrEmpty(Token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);
            return await _httpClient.GetFromJsonAsync<T>(uri);
        }

        public async Task<T> PostAsync<T>(string path, Dictionary<string, string> queryParams)
        {
            using var content = new FormUrlEncodedContent(queryParams);
            var uri = _host + path;
            if (!string.IsNullOrEmpty(Token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);
            var result = await _httpClient.PostAsJsonAsync(uri, queryParams);
            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP = {result}");
            }
            return await result.Content.ReadFromJsonAsync<T>();
        }

    }
}
