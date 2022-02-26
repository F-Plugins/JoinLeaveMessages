using Newtonsoft.Json.Linq;
using Rocket.Core.Logging;
using System.IO;
using System.Net;

namespace Feli.RocketMod.JoinLeaveMessages.Helpers
{
    public class IpGeolocationHelper
    {
        public static string GetCountryFromIp(string address)
        {
            var request = CreateRequest(address);

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Logger.LogError($"HTTP Error: {(int)response.StatusCode}, {response.StatusCode}");
                return string.Empty;
            }

            var content = ReadContent(response.GetResponseStream());

            var @object = JObject.Parse(content);

            if(@object["status"].ToString() == "fail")
            {
                Logger.LogError($"Failed to get the ip location. Error: {@object["message"]}");
                return string.Empty;
            }

            return @object["country"].ToString();
        }

        internal static WebRequest CreateRequest(string address)
        {
            return WebRequest.Create($"http://ip-api.com/json/{address}?fields=status,message,country");
        }

        internal static string ReadContent(Stream stream)
        {
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
