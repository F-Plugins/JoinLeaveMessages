using Newtonsoft.Json.Linq;
using Rocket.Core.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Feli.RocketMod.JoinLeaveMessages.Helpers
{
    public class IpGeolocationHelper
    {
        public static async Task<string> GetCountryFromIp(string address)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"http://ip-api.com/json/{address}?fields=status,message,country");

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError($"HTTP Error: {(int)response.StatusCode}, {response.StatusCode}");
                return string.Empty;
            }

            var content = await response.Content.ReadAsStringAsync();

            var @object = JObject.Parse(content);
            if (@object["status"].ToString() == "fail")
            {
                Logger.LogError($"Failed to get the ip location. Error: {@object["message"]}");
                return string.Empty;
            }

            return @object["country"].ToString();
        }
    }
}
