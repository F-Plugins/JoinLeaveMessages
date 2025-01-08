using Feli.RocketMod.JoinLeaveMessages.Helpers;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Feli.RocketMod.JoinLeaveMessages
{
    public class Plugin : RocketPlugin<Configuration>
    {
        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"Join", "<color=yellow>{0} has connected to the server from {1}</color>" },
            {"Leave", "<color=red>{0} disconnected from the server</color>" }
        };

        protected override void Load()
        {
            if (Configuration.Instance.JoinMessages)
                U.Events.OnPlayerConnected += OnPlayerConnected;

            if (Configuration.Instance.LeaveMessages)
                U.Events.OnPlayerDisconnected += OnPlayerDisconnected;

            Logger.Log($"JoinLeaveMessages plugin v1.0.4 loaded !");
            Logger.Log("Do you want more cool plugins? Join now: https://discord.gg/4FF2548 !");
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            Task.Run(async () =>
            {
                try
                {
                    var country = string.Empty;

                    if (Configuration.Instance.ShowCountry)
                        country = await IpGeolocationHelper.GetCountryFromIp(player.IP);

                    var message = Translate("Join", player.DisplayName, country);

                    TaskDispatcher.QueueOnMainThread(() =>
                    {
                        Logger.Log(message);
                        ChatManager.serverSendMessage(message, Color.green, mode: EChatMode.GLOBAL, iconURL: Configuration.Instance.CustomImageUrl, useRichTextFormatting: true);
                    });
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "There was an error during the player join");
                }
            });
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            var message = Translate("Leave", player.DisplayName);

            Logger.Log(message);
            ChatManager.serverSendMessage(message, Color.green, mode: EChatMode.GLOBAL, iconURL: Configuration.Instance.CustomImageUrl, useRichTextFormatting: true);
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;

            Logger.Log($"JoinLeaveMessages plugin v1.0.4 unloaded !");
            Logger.Log("Do you want more cool plugins? Join now: https://discord.gg/4FF2548 !");
        }
    }
}
