using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class ReadTempRequested : EventBase
    {
        public ReadTempRequested(int pinNumber)
        {
            PinNumber = pinNumber;
        }

        public int PinNumber {get; }

        public static ReadTempRequested Create(int pinNumber)
        {
            return new ReadTempRequested(pinNumber);
        }
    }
}
