using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class TempReadEvent : EventBase
    {
        public TempReadEvent(int pinNumber, double temp, double humidity)
        {
            PinNumber = pinNumber;
            Temp = temp;
            Humidity = humidity;
        }

        public int PinNumber {get; }
        public double Temp {get;set; }
        public double Humidity {get;set; }

        public static TempReadEvent Create(int pinNumber, double temp, double humidity)
        {
            return new TempReadEvent(pinNumber, temp, humidity);
        }
    }
}
