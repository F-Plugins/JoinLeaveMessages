using Autofac;
using Feli.OpenMod.JoinLeaveMessages.Helpers;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Plugins;
using OpenMod.Core.Ioc.Extensions;
using System;

namespace Feli.OpenMod.JoinLeaveMessages
{
    public class PluginContainerConfigurator : IPluginContainerConfigurator
    {
        public void ConfigureContainer(IPluginServiceConfigurationContext context)
        {
            context.ContainerBuilder
                .Register(
                    (builder) => ActivatorUtilities.CreateInstance<IpGeolocationHelper>(builder.Resolve<IServiceProvider>()))
                .WithLifetime(ServiceLifetime.Singleton)
                .OwnedByLifetimeScope();
        }
    }
}
