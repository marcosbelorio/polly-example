using HttpClientPollyExample.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientPollyExample.Services
{
    
    public class HttpBinService : IHttpBinService
    {
        private readonly HttpClient _httpClient;
        public HttpBinService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Get(int code)
        {
            var response = await _httpClient.GetAsync($"http://httpbin.org/status/{code}");
            Console.WriteLine(response.IsSuccessStatusCode);
        }
    }
}
