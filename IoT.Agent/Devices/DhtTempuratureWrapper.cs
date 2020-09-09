using Iot.Device.GrovePiDevice;
using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Agent.Devices
{
    public class DhtTempuratureWrapper
    {
        public event EventHandler<TempHumidityResult> TempRead;

        public DhtTempuratureWrapper()
        {

        }

        private void Initialize(int pin)
        {
#if PI
            //if (_sensor == null)
            //{
            //    Console.WriteLine($"Initializing temp sensor on physical pin: {pin}");
            //}
#endif
        }

        public TempHumidityResult ReadTemp(int pin)
        {
            var result = new TempHumidityResult();

#if PI
            //using (Dht22 sensor = new Dht22(pin, System.Device.Gpio.PinNumberingScheme.Board))
            //{
            //    result.Humidity = _sensor.Humidity;
            //    result.Temp = _sensor.Temperature.Kelvin;
            //}
#else
            var ran = new Random(30);

            result.Humidity = ran.NextDouble();
            result.Temp = ran.NextDouble();
            result.Pin = pin;

#endif
            TempRead?.Invoke(this, result);
            return result;
        }
    }

    public struct TempHumidityResult
    {
        public double Temp { get; set; }
        public double Humidity { get; set; }
        public int Pin { get; set; }
    }
}
