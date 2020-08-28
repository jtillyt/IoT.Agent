using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class PinStateChanged : EventBase
    {
        public PinStateChanged(int pinNumber, bool pinState)
        {
            PinNumber = pinNumber;
            PinState = pinState;
        }

        public bool PinState { get; set; }
        public int PinNumber { get; set; }

        public static PinStateChanged Create(int pinNumber, bool pinState)
        {
            return new PinStateChanged(pinNumber, pinState);
        }
    }
}
