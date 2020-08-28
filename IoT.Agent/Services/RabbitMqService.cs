using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IoT.EventBus;
using IoT.Shared;
using IoT.Shared.EventBus;
using IoT.Shared.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Agent.Services
{
    public class RabbitMqService : IHostedService
    {
        private readonly ILogger<RabbitMqService> _logger;
        private readonly IEventBus _eventBus;
        private IConnection _connection;
        private IModel _model;
        private readonly string _privateQueueName;

        private EventingBasicConsumer _agentMessageInConsumer;

        public RabbitMqService(ILogger<RabbitMqService> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;

            _privateQueueName = "Agent_" + Environment.MachineName + "_Queue";
        }

        private void OnMqSendOut(MqSendRequestedEvent sendOutEvent)
        {
            using (var model = _connection.CreateModel())
            {
                model.BasicPublish(sendOutEvent.Exchange, sendOutEvent.QueueName, null, sendOutEvent.Data);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _eventBus.EventStream.OfType<MqSendRequestedEvent>().Subscribe(OnMqSendOut);

            _logger.LogInformation("Starting Rabbit MQ Service");

            var factory = new ConnectionFactory() { HostName = "192.168.1.100", Port = 5672, UserName = "tester", Password = "tester" };
            _connection = factory.CreateConnection($"IoTAgent_{Environment.MachineName}");
            _model = _connection.CreateModel();

            _model.ExchangeDeclare(MqExchangeNames.AgentRunStateSent, ExchangeType.Topic, false, true);
            _model.ExchangeDeclare(MqExchangeNames.AgentStatusRequest, ExchangeType.Topic, false, true);
            _model.ExchangeDeclare(MqExchangeNames.AgentStatusSent, ExchangeType.Topic, false, true);
            _model.ExchangeDeclare(MqExchangeNames.AgentStateUpdateRequest, ExchangeType.Topic, false, true);

            _model.QueueDeclare(_privateQueueName, false, false, true);
            _model.QueueBind(_privateQueueName, MqExchangeNames.AgentStateUpdateRequest, "*");
            _model.QueueBind(_privateQueueName, MqExchangeNames.AgentStatusRequest, "*");

            _agentMessageInConsumer = new EventingBasicConsumer(_model);
            _agentMessageInConsumer.Received += OnUpdateRequestIn;
            _model.BasicConsume(_privateQueueName, true, _agentMessageInConsumer);

            _logger.LogInformation("Rabbit MQ Service Started");
        }

        private string QueueNameByExchangeAndId(string exchange, string entityId)
        {
            return $"{exchange}_{entityId}";
        }

        private void OnUpdateRequestIn(object sender, BasicDeliverEventArgs e)
        {
            if (e.Exchange == MqExchangeNames.AgentStatusRequest)
            {
                var messageData = e.Body.ToArray();
                var messageString = Encoding.ASCII.GetString(messageData);
                var message = JsonConvert.DeserializeObject<SendEntityStateRequestedEvent>(messageString);

                _eventBus.Publish(message);
            }
            else if (e.Exchange == MqExchangeNames.AgentStateUpdateRequest)
            {
                var messageData = e.Body.ToArray();
                var messageString = Encoding.ASCII.GetString(messageData);
                var message = JsonConvert.DeserializeObject<UpdateAgentStateEvent>(messageString);

                _eventBus.Publish(message);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stoping Rabbit MQ Service");

            _logger.LogInformation("Rabbit MQ Service Stopped");
        }
    }
}
