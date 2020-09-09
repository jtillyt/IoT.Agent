using InfluxDB.LineProtocol.Payload;
using IoT.Agent.Hubs;
using IoT.Shared.Events;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Timers;

namespace IoT.ServiceHost.Gpio
{
    public class DuplexSerial
    {
        private object _synclock = new object();

        private IConfiguration _configuration;
        private SerialPort _serialPort;
        private InfluxDB.LineProtocol.Client.LineProtocolClient _client;
        private Timer _timer = new Timer();

        public event EventHandler<PinStateChanged> PinStateChanged;

        public DuplexSerial(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new InfluxDB.LineProtocol.Client.LineProtocolClient(new Uri("http://192.168.1.100:8086"), "Home");
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
            var lineProtocolPayload = new LineProtocolPayload();

            string[] lines = null;
            lock (_synclock)
            {
                var currString = _sb.ToString();

                if (string.IsNullOrWhiteSpace(currString))
                    return;

                var lastLineBreakIndex = currString.LastIndexOf(breakDelim);

                if (lastLineBreakIndex <0)
                    return;

                string valsString = currString.Substring(0, lastLineBreakIndex);
                lines = valsString.Split(breakDelim);
                _sb.Remove(0, lastLineBreakIndex);
            }

            if (lines==null)
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

                Debug.WriteLine($"NodeTypeId={nodeTypeIdString}, NodeId={nodeIdString}, ValueTypeId={valueTypeIdString}, Value={valueString}");

                var fields = new Dictionary<string, object>();

                if (valueTypeIdString == "1")
                    fields.Add("Temp_In_F", double.Parse(valueString));

                if (valueTypeIdString == "2")
                    fields.Add("Humidity_Percent", double.Parse(valueString));

                var point = new LineProtocolPoint($"Node_{nodeIdString}_Climate", fields, null, DateTime.UtcNow);
                lineProtocolPayload.Add(point);
            }

            try
            {
                var result = await _client.WriteAsync(lineProtocolPayload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending metrics: {ex}");
            }
        }

        public void StartListening()
        {
            string port = _configuration["Usb1Port"];
            _serialPort = new SerialPort(port, 9600, Parity.None);
            _serialPort.BaudRate = 9600;
            _serialPort.DataReceived += _serialPort_DataReceived;
            _serialPort.Open();
        }

        private StringBuilder _sb = new StringBuilder();
        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (_synclock)
                {
                    var existing = _serialPort.ReadExisting();
                    _sb.Append(existing);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occured while reading data: {ex}");
            }


        }
    }
}
