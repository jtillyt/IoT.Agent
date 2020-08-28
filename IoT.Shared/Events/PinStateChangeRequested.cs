using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class PinStateChangeRequested : EventBase
    {
        public PinStateChangeRequested(int pinNumber, int pinState)
        {
            PinNumber = pinNumber;
            PinState = pinState;
        }

        public int PinState { get; set; }
        public int PinNumber { get; set; }

        public static PinStateChangeRequested Create(int pinNumber, int pinState)
        {
            return new PinStateChangeRequested(pinNumber, pinState);
        }
    }
}
