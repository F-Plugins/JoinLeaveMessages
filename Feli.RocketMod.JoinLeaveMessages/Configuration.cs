using Rocket.API;

namespace Feli.RocketMod.JoinLeaveMessages
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool JoinMessages { get; set; }
        public bool ShowCountry { get; set; }
        public string CustomImageUrl { get; set; }
        public bool LeaveMessages { get; set; }

        public void LoadDefaults()
        {
            JoinMessages = true;
            ShowCountry = true;
            CustomImageUrl = string.Empty;
            LeaveMessages = false;
        }
    }
}
