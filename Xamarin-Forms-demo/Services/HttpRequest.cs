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

        public static string Token { get; set; } = string.Empty;

        public readonly HttpClient _httpClient = new HttpClient();

        public HttpRequest(string host)
        {
            _host = host;
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

        public async Task<T> PostAsync<T>(string path, T queryParams)
        {
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
