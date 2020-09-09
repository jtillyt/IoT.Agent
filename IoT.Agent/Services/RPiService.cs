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

        private readonly IDuplexSerialService _serial;
        private readonly IMetricsService _metricsService;

        private readonly System.Timers.Timer _blinkTimer = new System.Timers.Timer();

        private IHubContext<PiStateHub> _hubContext;

        public RPiService(ILogger<RPiService> logger, IEventBus eventBus, IConfiguration configuration, IDuplexSerialService duplexSerialService, IHubContext<PiStateHub> hubContext, IMetricsService metricsService)
        {
            _logger = logger;
            _eventBus = eventBus;
            _configuration = configuration;
            _hubContext = hubContext;

            _gpio = new GpioWrapper();
            _gpio.PinStateChanged += _gpio_PinStateChanged;

            _serial = duplexSerialService;
            _serial.StartListening();

            _metricsService = metricsService;
            _metricsService.AddValueTypeMapping(1, "Temp_In_F");
            _metricsService.AddValueTypeMapping(2, "Humidity_Percent");

            _eventBus.EventStream.OfType<PinStateChangeRequested>().Subscribe(OnPinStateChangeRequested);
            _eventBus.EventStream.OfType<PinListenStateChangeRequested>().Subscribe(OnPinStateListeningChangeRequested);
            _eventBus.EventStream.OfType<NumericNodeValueReceivedEvent>().Subscribe(OnNumericNodeValueReceived);
        }

        private void OnNumericNodeValueReceived(NumericNodeValueReceivedEvent evt)
        {
            _hubContext.Clients.All.SendAsync("OnNumericNodeValueReceived", evt.NodeTypeId,evt.NodeId, evt.ValueTypeId, evt.Value);
        }

        private void _gpio_PinStateChanged(object sender, PinStateChanged pinState)
        {
            _hubContext.Clients.All.SendAsync("OnPinStateChanged", pinState);
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
