using InfluxDB.LineProtocol.Payload;
using IoT.EventBus;
using IoT.Shared.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace IoT.Agent.Services
{
    public class MetricsService : IMetricsService
    {
        private readonly IDictionary<int, string> _nameLookup = new Dictionary<int, string>();

        private readonly InfluxDB.LineProtocol.Client.LineProtocolClient _client;
        private readonly IConfiguration _configuration;
        private readonly IEventBus _eventBus;
        private readonly ILogger<MetricsService> _logger;

        public MetricsService(ILogger<MetricsService> logger, IConfiguration configuration, IEventBus eventBus)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var host = _configuration["InfluxAddress"];
            var dbName = _configuration["InfluxDbName"];

            _client = new InfluxDB.LineProtocol.Client.LineProtocolClient(new Uri(host), dbName);

            _eventBus.EventStream.OfType<NumericNodeValueReceivedEvent>().Subscribe(OnNumericNodeValueReceived);
        }

        private async void OnNumericNodeValueReceived(NumericNodeValueReceivedEvent evt)
        {
            var lineProtocolPayload = new LineProtocolPayload();

            var fields = new Dictionary<string, object>();

            _nameLookup.TryGetValue(evt.ValueTypeId, out string valueTypeName);
            fields.Add(valueTypeName, evt.Value);

            var point = new LineProtocolPoint($"Node_{evt.NodeId}_Climate", fields, null, DateTime.UtcNow);
            lineProtocolPayload.Add(point);

            try
            {
                var result = await _client.WriteAsync(lineProtocolPayload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending metrics: {ex}");
            }
        }

        public void AddValueTypeMapping(int valueTypeId, string name)
        {
            _nameLookup.Add(valueTypeId, name);
        }
    }
}
