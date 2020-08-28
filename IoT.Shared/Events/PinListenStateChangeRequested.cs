using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class PinListenStateChangeRequested : EventBase
    {
        public PinListenStateChangeRequested(int pinNumber, bool pinListenState)
        {
            PinNumber = pinNumber;
            PinListenState = pinListenState;
        }

        public bool PinListenState { get; set; }
        public int PinNumber { get; set; }

        public static PinListenStateChangeRequested Create(int pinNumber, bool pinListenState)
        {
            return new PinListenStateChangeRequested(pinNumber, pinListenState);
        }
    }
}
