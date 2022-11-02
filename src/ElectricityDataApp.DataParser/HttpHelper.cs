using ElectricityDataApp.DataParser.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.DataParser
{
    public class HttpHelper : IHttpHelper
    {
        private readonly HttpClient _client;
        private readonly DataParserClientOptions _options;

        public HttpHelper(HttpClient client,
            IOptions<DataParserClientOptions> options)
        {
            _client = client;
            _options = options.Value;

            if (_options.HttpRequestTimeoutInMinutes.HasValue)
            {
                _client.Timeout = TimeSpan.FromMinutes(_options.HttpRequestTimeoutInMinutes.Value);
            }
        }

        public async Task<string> GetContentAsString(string url)
        {
            using var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            using var content = response.Content;

            var result = await content.ReadAsStringAsync();

            return result;
        }
    }
}
