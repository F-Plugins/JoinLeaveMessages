using Newtonsoft.Json.Linq;
using Rocket.Core.Logging;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Feli.RocketMod.JoinLeaveMessages.Helpers
{
    public class IpGeolocationHelper
    {
        public static async Task<string> GetCountryFromIp(string address)
        {
            var request = (HttpWebRequest)WebRequest.Create($"http://ip-api.com/json/{address}?fields=status,message,country");
            var response = (HttpWebResponse) await request.GetResponseAsync();

            if(response.StatusCode != HttpStatusCode.OK)
            {
                Logger.LogError($"HTTP Error: {(int)response.StatusCode}, {response.StatusCode}");
                return string.Empty;
            }

            var @object = JObject.Parse(await new StreamReader(response.GetResponseStream()).ReadToEndAsync());
            
            if (@object["status"].ToString() == "fail")
            {
                Logger.LogError($"Failed to get the ip location. Error: {@object["message"]}");
                return string.Empty;
            }

            return @object["country"].ToString();
        }
    }
}
