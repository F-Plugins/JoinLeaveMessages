using Feli.OpenMod.JoinLeaveMessages.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Users.Events;
using SDG.Unturned;
using System.Threading.Tasks;
using UnityEngine;

namespace Feli.OpenMod.JoinLeaveMessages.Events
{
    public class EventListener : IEventListener<UnturnedUserConnectedEvent>, IEventListener<UnturnedUserDisconnectedEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IpGeolocationHelper _geolocator;

        public EventListener(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            IpGeolocationHelper geolocator)
        {
            _configuration = configuration;
            _stringLocalizer = stringLocalizer;
            _geolocator = geolocator;
        }

        public async Task HandleEventAsync(object sender, UnturnedUserConnectedEvent @event)
        {
            if (!_configuration.GetSection("joinMessage:enabled").Get<bool>()) 
                return;

            var country = string.Empty;

            if (_configuration.GetSection("joinMessage:showCountry").Get<bool>())
                country = await _geolocator.GetCountryFromIpAsync(@event.User.Player.Address.MapToIPv4().ToString());

            var message = _stringLocalizer["join", new
            {
                User = @event.User,
                Country = country
            }];

            ChatManager.serverSendMessage(message, Color.green, mode: EChatMode.GLOBAL, iconURL: _configuration["joinMessage:customIconUrl"], useRichTextFormatting: true);
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
