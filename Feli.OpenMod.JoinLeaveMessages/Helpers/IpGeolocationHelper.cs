using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Feli.OpenMod.JoinLeaveMessages.Helpers
{
    public class IpGeolocationHelper : IDisposable
    {
        private readonly ILogger<IpGeolocationHelper> _logger;
        private readonly HttpClient _client;

        public IpGeolocationHelper(ILogger<IpGeolocationHelper> logger)
        {
            _logger = logger;
            _client = new HttpClient();
        }

        public async Task<string> GetCountryFromIpAsync(string address)
        {
            var respnse = await _client.GetAsync($"http://ip-api.com/json/{address}?fields=status,message,country");

            if (!respnse.IsSuccessStatusCode)
            {
                _logger.LogError($"HTTP Error: {(int)respnse.StatusCode}, {respnse.StatusCode}");
                return string.Empty;
            }

            var content = await respnse.Content.ReadAsStringAsync();

            var @object = JObject.Parse(content);

            if (@object["status"].ToString() == "fail")
            {
                _logger.LogError($"Failed to get the ip location. Error: {@object["message"]}");
                return string.Empty;
            }

            return @object["country"].ToString();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
