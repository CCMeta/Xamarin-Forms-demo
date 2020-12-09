using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Xamarin_Forms_demo.Services
{
    public class HttpRequest
    {
        public readonly string _host;

        public HttpRequest(string host)
        {
            _host = host;
        }

        public async Task<T> GetAsync<T>(string path, Dictionary<string, string> queryParams)
        {
            using var content = new FormUrlEncodedContent(queryParams);
            var query = content.ReadAsStringAsync().Result;
            var uri = _host + path + "?" + query;
            var httpClient = await new HttpClient().GetAsync(uri);
            if (!httpClient.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP = {httpClient}");
            }
            using var stream = await httpClient.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }

    }
}
