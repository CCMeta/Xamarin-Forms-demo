using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Services
{
    public class HttpRequest
    {
        public readonly string _domain;

        public HttpRequest(string Domain)
        {
            _domain = Domain;
        }

        public async Task<T> GetAsync<T>(string path)
        {
            using var httpClient = await new HttpClient().GetAsync(_domain + path);
            using var stream = await httpClient.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }

    }
}
