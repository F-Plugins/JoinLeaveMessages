using Cysharp.Threading.Tasks;
using Feli.OpenMod.JoinLeaveMessages.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Users.Events;
using SDG.Unturned;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Feli.OpenMod.JoinLeaveMessages.Events
{
    public class EventListener : IEventListener<UnturnedUserConnectedEvent>, IEventListener<UnturnedUserDisconnectedEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IpGeolocationHelper _geolocator;
        private readonly ILogger<EventListener> _logger;

        public EventListener(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            IpGeolocationHelper geolocator,
            ILogger<EventListener> logger)
        {
            _configuration = configuration;
            _stringLocalizer = stringLocalizer;
            _geolocator = geolocator;
            _logger = logger;
        }

        public Task HandleEventAsync(object sender, UnturnedUserConnectedEvent @event)
        {
            if (!_configuration.GetSection("joinMessage:enabled").Get<bool>())
                return Task.CompletedTask;

            UniTask.RunOnThreadPool(async () =>
            {
                try
                {
                    var country = string.Empty;

                    if (_configuration.GetSection("joinMessage:showCountry").Get<bool>())
                        country = await _geolocator.GetCountryFromIpAsync(@event.User.Player.Address.MapToIPv4().ToString());

                    var message = _stringLocalizer["join", new
                    {
                        User = @event.User,
                        Country = country
                    }];

                    await UniTask.SwitchToMainThread();

                    ChatManager.serverSendMessage(message, Color.green, mode: EChatMode.GLOBAL, iconURL: _configuration["joinMessage:customIconUrl"], useRichTextFormatting: true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "There was an error during the player join");
                }
            });

            return Task.CompletedTask;
        }

        public Task HandleEventAsync(object sender, UnturnedUserDisconnectedEvent @event)
        {
            if (!_configuration.GetSection("leaveMessage:enabled").Get<bool>())
                return Task.CompletedTask;

            var message = _stringLocalizer["leave", new
            {
                User = @event.User
            }];

            ChatManager.serverSendMessage(message, Color.green, mode: EChatMode.GLOBAL, iconURL: _configuration["leaveMessage:customIconUrl"], useRichTextFormatting: true);

            return Task.CompletedTask;
        }
    }
}
