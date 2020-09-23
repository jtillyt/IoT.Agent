using IoT.Shared.Entities;
using IoT.Shared.Messaging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Agent.Mobile
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Microsoft.AspNetCore.SignalR.Client.HubConnection _hubConnection;
        private string _connectionId;

        public MainViewModel()
        {

        }

        private string _tempValue = "0";

        public string TempValue
        {
            get { return _tempValue; }
            set
            {
                if (_tempValue != value)
                {
                    _tempValue = value;
                    RaisePropertyChanged(nameof(TempValue));
                }
            }
        }

        private string _humidityValue = "0";

        public string HumidityValue
        {
            get { return _humidityValue; }
            set
            {
                if (_humidityValue != value)
                {
                    _humidityValue = value;
                    RaisePropertyChanged(nameof(HumidityValue));
                }
            }
        }

        #region SignalR 


        public async Task ConnectAsync(string address = null)
        {
            Disconnect();

            try
            {
                var connectionBuilder = new HubConnectionBuilder();
                connectionBuilder.WithUrl(address).WithAutomaticReconnect();

                _hubConnection = connectionBuilder.Build();
                _hubConnection.Closed += _hubConnection_Closed;
                _hubConnection.Reconnected += _hubConnection_Reconnected;
                _hubConnection.Reconnecting += _hubConnection_Reconnecting;
                _hubConnection.On<MessageEnvelopeDto>(MessageConstants.WebClientMethodName, OnMessageSentToClient);
                _hubConnection.On<int, int, int, double>(MessageConstants.OnNumericNodeValueReceived, OnNumericNodeValueReceived);

                await _hubConnection.StartAsync();

                _connectionId = _hubConnection.ConnectionId;

                await Login();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while connecting to {address}. {ex}");
            }
        }

        public async void Disconnect()
        {
            try
            {
                if (_hubConnection != null)
                {
                    await _hubConnection.StopAsync();
                    await _hubConnection.DisposeAsync();
                    _hubConnection = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while disconnecting. {ex}");
            }
        }

        private async Task Login()
        {
            var userInfo = new UserInfo()
            {
                ClientId = Guid.NewGuid().ToString(),
                SignalrConnectionId = _connectionId,
                FirstName = "James",
                LastName = "Thomas"
            };

            try
            {
                await _hubConnection.SendAsync(MessageConstants.MobileLogin, userInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while logging in to hub: {ex}");
            }
        }

        private async Task _hubConnection_Reconnecting(Exception arg)
        {
            Debug.WriteLine($"Reconnecting");
        }

        private async Task _hubConnection_Reconnected(string arg)
        {
            Debug.WriteLine($"Reconnected");
        }

        private async Task _hubConnection_Closed(Exception arg)
        {
            Debug.WriteLine($"Connection close");
        }

        #endregion

        #region Push Message Handlers

        private void OnNumericNodeValueReceived(int nodeId, int nodeTypeId, int valueTypeId, double value)
        {
            if (valueTypeId == 1)
                TempValue = value.ToString();
            else if (valueTypeId == 2)
                HumidityValue = value.ToString();
        }

        private void OnMessageSentToClient(MessageEnvelopeDto message)
        {

        }

        #endregion


        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
