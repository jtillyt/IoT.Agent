using IoT.Agent.Repos;
using IoT.Shared.Entities;
using IoT.Shared.Events;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Agent.Hubs
{
    //This is transient. Don't save state in here
    public class PiStateHub : Hub
    {
        private readonly IUserRepo _userRepo; //Save the state in a repo

        public PiStateHub(IUserRepo userRepo)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        public void SendPinState(PinStateChanged pinState)
        {
            Clients.All.SendAsync("OnPinStateChanged", pinState);

            //or
            //foreach(var user in _userRepo.ListAll())
            //{
            //    Debug.WriteLine($"Sending message to { user.FirstName } { user.LastName }");
            //    Clients.Client(user.SignalrConnectionId).SendAsync("OnPinStateChanged", pinState);
            //}
        }

        public override Task OnConnectedAsync()
        {
            Debug.WriteLine($"New client transport connection from ConnectionId: {Context.ConnectionId}");

            return base.OnConnectedAsync();
        }

        public void MobileLogin(UserInfo clientUserInfo)
        {
            clientUserInfo.LoginTime = DateTime.Now;
            Debug.WriteLine($"Connection received from: { clientUserInfo.FirstName } { clientUserInfo.LastName }");
            _userRepo.Add(clientUserInfo);
        }
    }
}
