using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IoT.EventBus;
using IoT.Shared;
using IoT.Shared.Entities;
using IoT.Shared.EventBus;
using IoT.Shared.Events;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Agent.Services
{
    public class AdminService : IHostedService
    {
        private readonly ILogger<AdminService> _logger;
        private readonly IEventBus _eventBus;

        private System.Timers.Timer _timer;

        private AgentEntity _self;

        public AdminService(ILogger<AdminService> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;

            _timer = new System.Timers.Timer();
            _timer.Interval = 3000;
            _timer.Elapsed += _timer_Elapsed;
            //_timer.Start();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //SendCurrentRunState();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await OnStateChanged(RunState.Starting);


            _logger.LogInformation("Admin Service Started");

            await OnStateChanged(RunState.Started);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await OnStateChanged(RunState.Stopping);

            _logger.LogInformation("Admin Service  Stopped");

            await OnStateChanged(RunState.Stopped);
        }

        private async Task OnStateChanged(RunState runState)
        {
            //SendCurrentRunState();
        }

        //private void SendCurrentRunState()
        //{
        //    if (_self != null)
        //    {
        //        var runStateChangedEvent = new EntityRunStateChangedEvent { EntityId = _self.EntityId, EntityType = EntityType.Agent, EntityRunState = _self.RunState };
        //        var outEvent = MqSendRequestedEvent.Create(MqExchangeNames.AgentRunStateSent, "HubQueue", runStateChangedEvent);

        //        _eventBus.Publish(outEvent);
        //    }
        //}

        //private void OnAgentStatusRequested(SendEntityStateRequestedEvent requestEvent)
        //{
        //    var stateEvent = AgentStatePublishedEvent.Create(_self);
        //    var outEvent = MqSendRequestedEvent.Create(MqExchangeNames.AgentStatusSent, "HubQueue", stateEvent);

        //    _eventBus.Publish(outEvent);
        //}
          }
}
