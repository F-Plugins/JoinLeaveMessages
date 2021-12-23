using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;
    
[assembly: PluginMetadata("Feli.JoinLeaveMessages",
    DisplayName = "Join and Leave Messages",
    Author = "Feli",
    Description = "Broadcast users connections and disconnections to the global chat",
    Website = "https://discord.gg/4FF2548"
)]

namespace Feli.OpenMod.JoinLeaveMessages
{
    public class Plugin : OpenModUnturnedPlugin
    {
        private readonly ILogger<Plugin> _logger;

        public Plugin(
            ILogger<Plugin> logger,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = logger;
        }

        protected override UniTask OnLoadAsync()
        {
            _logger.LogInformation($"JoinLeaveMessages plugin v{Version} loaded !");
            _logger.LogInformation("Do you want more cool plugins? Join now: https://discord.gg/4FF2548 !");
            return UniTask.CompletedTask;
        }
    }
}
