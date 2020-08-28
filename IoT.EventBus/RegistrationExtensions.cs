using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.EventBus
{
    public static class RegistrationExtensions
    {
        public static void AddEventBusService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IEventBus, EventBus>();
        }
    }
}
