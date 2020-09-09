using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IoT.EventBus;
using IoT.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using IoT.Shared.Events;
using IoT.ServiceHost.Gpio;
using Microsoft.AspNetCore.SignalR;
using IoT.Agent.Hubs;
using IoT.Agent.Devices;
using Microsoft.Extensions.Configuration;

namespace IoT.Agent.Services
{
    public class RPiService : IHostedService
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<RPiService> _logger;
        private readonly IConfiguration _configuration;

        private readonly GpioWrapper _gpio;
        private readonly DhtTempuratureWrapper _tempWrapper;
        private readonly DuplexSerial _serial;

        private readonly System.Timers.Timer _blinkTimer = new System.Timers.Timer();

        private int _blinkingPinState = 0;
        private int _blinkingPinNumber = 11;

        private IHubContext<PiStateHub> _hubContext;

        public RPiService(ILogger<RPiService> logger, IEventBus eventBus, IConfiguration configuration)
        {
            _logger = logger;
            _eventBus = eventBus;
            _configuration = configuration;
            //_hubContext = hubContext;

            _gpio = new GpioWrapper();
            _gpio.PinStateChanged += _gpio_PinStateChanged;

            _tempWrapper = new DhtTempuratureWrapper();
            _tempWrapper.TempRead += _tempWrapper_TempRead;

            _serial = new DuplexSerial(_configuration);
            _serial.StartListening();

            _eventBus.EventStream.OfType<PinStateChangeRequested>().Subscribe(OnPinStateChangeRequested);
            _eventBus.EventStream.OfType<PinListenStateChangeRequested>().Subscribe(OnPinStateListeningChangeRequested);
            _eventBus.EventStream.OfType<ReadTempRequested>().Subscribe(OnTempRequested);

        }

        private void OnTempRequested(ReadTempRequested evt)
        {
            _tempWrapper.ReadTemp(evt.PinNumber);
        }

        private void _tempWrapper_TempRead(object sender, TempHumidityResult evt)
        {
            Console.WriteLine($"Temp:{evt.Temp}, Humidity:{evt.Humidity}");
        }

        private void _gpio_PinStateChanged(object sender, PinStateChanged pinState)
        {
            //_hubContext.Clients.All.SendAsync("OnPinStateChanged", pinState);
        }

        private void OnPinStateListeningChangeRequested(PinListenStateChangeRequested evt)
        {
            if (evt.PinListenState)
            {
                _gpio.SetInputPin(evt.PinNumber);
                _gpio.StartListeningToPin(evt.PinNumber);
            }
            else
            {
                _gpio.StopListeningToPin(evt.PinNumber);
            }
        }

        private void OnPinStateChangeRequested(PinStateChangeRequested evt)
        {
            _gpio.SetOutputPin(evt.PinNumber);
            _gpio.WriteBoolOut(evt.PinNumber, evt.PinState == 1);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
