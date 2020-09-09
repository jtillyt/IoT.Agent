using InfluxDB.LineProtocol.Payload;
using IoT.EventBus;
using IoT.Shared.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Timers;

namespace IoT.ServiceHost.Gpio
{
    public class DuplexSerialService : IDuplexSerialService
    {
        private object _synclock = new object();

        private readonly IConfiguration _configuration;
        private readonly ILogger<DuplexSerialService> _logger;
        private readonly IEventBus _eventBus;

        private readonly Timer _timer = new Timer();

        private readonly StringBuilder _buffer = new StringBuilder();

        private SerialPort _serialPort;

        public DuplexSerialService(ILogger<DuplexSerialService> logger, IConfiguration configuration, IEventBus eventBus)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            _timer.Interval = 5000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ProcessQueue();
        }

        private async void ProcessQueue()
        {
            string breakDelim = "\r\n";

            string[] lines = null;
            lock (_synclock)
            {
                var currString = _buffer.ToString();

                if (string.IsNullOrWhiteSpace(currString))
                    return;

                var lastLineBreakIndex = currString.LastIndexOf(breakDelim);

                if (lastLineBreakIndex < 0)
                    return;

                string valsString = currString.Substring(0, lastLineBreakIndex);
                lines = valsString.Split(breakDelim);
                _buffer.Remove(0, lastLineBreakIndex);
            }

            if (lines == null)
                return;

            foreach (var line in lines)
            {
                var cleanedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(cleanedLine))
                    continue;

                string[] split = cleanedLine.Split(',');

                if (split.Length != 4)
                {
                    Debug.WriteLine($"Received unexpected split of length: {split.Length}. Orginal string: {cleanedLine}");
                    continue;
                }

                string nodeTypeIdString = split[0];
                string nodeIdString = split[1];
                string valueTypeIdString = split[2];
                string valueString = split[3];

                int nodeTypeId = int.Parse(nodeTypeIdString);
                int nodeId = int.Parse(nodeIdString);
                int valueTypeId = int.Parse(valueTypeIdString);
                double value = double.Parse(valueString);

                Debug.WriteLine($"NodeTypeId={nodeTypeIdString}, NodeId={nodeIdString}, ValueTypeId={valueTypeIdString}, Value={valueString}");

                _eventBus.Publish(NumericNodeValueReceivedEvent.Create(nodeTypeId, nodeId, valueTypeId, value));
            }
        }

        public void StartListening()
        {
            string port = _configuration["Usb1Port"];

            _logger.LogInformation($"Starting {nameof(DuplexSerialService)} on port {port}");

            _serialPort = new SerialPort(port, 9600, Parity.None)
            {
                BaudRate = 9600
            };

            _serialPort.DataReceived += _serialPort_DataReceived;
            _serialPort.Open();

            _timer.Start();
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (_synclock)
                {
                    var existing = _serialPort.ReadExisting();
                    _buffer.Append(existing);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occured while reading data: {ex}");
            }
        }

        public void StopListening()
        {
            _logger.LogInformation($"Stopping {nameof(DuplexSerialService)}");

            try
            {
                _timer.Stop();
                _buffer.Clear();

                _serialPort.DataReceived -= _serialPort_DataReceived;
                _serialPort.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
