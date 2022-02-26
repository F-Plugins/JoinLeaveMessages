using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
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
            var request = CreateRequest(address);

            var response = (HttpWebResponse)await request.GetResponseAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("HTTP Error: {StatusCode}, {StatusCodeString}", (int)response.StatusCode, response.StatusCode.ToString());
                return string.Empty;
            }

            var content = await ReadContentAsync(response.GetResponseStream());

            var @object = JObject.Parse(content);

            if (@object["status"].ToString() == "fail")
            {
                _logger.LogError("Failed to get the ip location. Error: {message}", @object["message"].ToString());
                return string.Empty;
            }

            return @object["country"].ToString();
        }

        internal WebRequest CreateRequest(string address)
        {
            return WebRequest.Create($"http://ip-api.com/json/{address}?fields=status,message,country");
        }

        internal async Task<string> ReadContentAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }
    }
}
