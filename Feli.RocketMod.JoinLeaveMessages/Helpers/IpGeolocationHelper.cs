using Newtonsoft.Json.Linq;
using Rocket.Core.Logging;
using System.Net.Http;

namespace Feli.RocketMod.JoinLeaveMessages.Helpers
{
    public class IpGeolocationHelper
    {
        public static string GetCountryFromIp(string address)
        {
            var client = new HttpClient();

            var response = client.GetAsync($"http://ip-api.com/json/{address}?fields=status,message,country").GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError($"HTTP Error: {(int)response.StatusCode}, {response.StatusCode}");
                return string.Empty;
            }

            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var @object = JObject.Parse(content);

            if(@object["status"].ToString() == "fail")
            {
                Logger.LogError($"Failed to get the ip location. Error: {@object["message"]}");
                return string.Empty;
            }

            return @object["country"].ToString();
        }
    }
}
