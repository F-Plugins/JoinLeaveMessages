using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Feli.OpenMod.JoinLeaveMessages.Helpers
{
    public class IpGeolocationHelper
    {
        private readonly ILogger<IpGeolocationHelper> _logger;

        public IpGeolocationHelper(ILogger<IpGeolocationHelper> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetCountryFromIpAsync(string address)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"http://ip-api.com/json/{address}?fields=status,message,country");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("HTTP Error: {StatusCode}, {StatusCodeString}", (int)response.StatusCode, response.StatusCode.ToString());
                return string.Empty;
            }

            var content = await response.Content.ReadAsStringAsync();

            var @object = JObject.Parse(content);
            if (@object["status"].ToString() == "fail")
            {
                _logger.LogError("Failed to get the ip location. Error: {message}", @object["message"].ToString());
                return string.Empty;
            }

            return @object["country"].ToString();
        }
    }
}
