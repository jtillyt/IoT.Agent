using IoT.Shared.Events;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Agent.Hubs
{
    public class PiStateHub : Hub
    {
        public PiStateHub()
        {
        }

        public void SendPinState(PinStateChanged pinState)
        {
            Clients.All.SendAsync("OnPinStateChanged", pinState);
        }
    }
}
