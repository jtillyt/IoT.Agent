using IoT.Agent.Hubs;
using IoT.Shared.Events;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace IoT.ServiceHost.Gpio
{
    public class GpioWrapper
    {
        private bool _simulatedPinState = false;
        private readonly Timer _pinChangeSimulationTimer = new Timer();

        public event EventHandler<PinStateChanged> PinStateChanged;


#if PI
        private readonly GpioController _controller;
#endif
        public GpioWrapper()
        {
            _pinChangeSimulationTimer.Elapsed += _pinChangeSimulationTimer_Elapsed;
#if PI
            Console.WriteLine("Starting controller");
            _controller = new GpioController(PinNumberingScheme.Board);
#else

            Debug.WriteLine("Starting controller in non-GPIO mode");
#endif
        }

        private void _blinkTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
        }

        private void _pinChangeSimulationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _simulatedPinState = !_simulatedPinState;
            OnPinChanged(this, new PinValueChangedEventArgs(_simulatedPinState ? PinEventTypes.Rising : PinEventTypes.Falling, 0));
        }

        public void SetInputPin(int pinNumber)
        {
#if PI
            try
            {
                if (!_controller.IsPinOpen(pinNumber))
                    _controller.OpenPin(pinNumber, PinMode.InputPullDown);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to set input pin {ex}");
            }
#else
            Console.WriteLine($"Setting pin {pinNumber} as an input pin");
#endif
        }

        public bool ReadPinState(int pinNumber)
        {
#if PI
            try
            {
                var val = _controller.Read(pinNumber);
                var bVal = (val == PinValue.High);
                Console.WriteLine($"Read pin: {pinNumber} as {bVal}");
                return bVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to read input pin {ex}");
            }
#else
            Debug.WriteLine($"Setting pin {pinNumber} as an input pin");
#endif
            return false;
        }


        public void SetOutputPin(int pinNumber)
        {
#if PI
            try
            {
                if (!_controller.IsPinOpen(pinNumber))
                    _controller.OpenPin(pinNumber, PinMode.Output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening pin {pinNumber}:{ex}");
            }
#else
            Debug.WriteLine($"Setting pin {pinNumber} as an output pin");
#endif
        }

        public void WriteBoolOut(int pinNumber, bool state)
        {
#if PI
            try
            {
                var pinValue = state ? PinValue.High : PinValue.Low;
                _controller.Write(pinNumber, pinValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to pin {pinNumber}:{ex}");
            }
#else
            Debug.WriteLine($"Writing to pin {pinNumber} to state: {state}");
#endif
        }

        public void StartListeningToPin(int pinNumber)
        {
#if PI
            _controller.RegisterCallbackForPinValueChangedEvent(pinNumber, PinEventTypes.Rising | PinEventTypes.Falling, OnPinChanged);
#else
            _pinChangeSimulationTimer.Interval = 1000;
            _pinChangeSimulationTimer.Start();
            Debug.WriteLine($"Started to listening to pin {pinNumber}");
#endif
        }

        public void StopListeningToPin(int pinNumber)
        {
#if PI
            _controller.UnregisterCallbackForPinValueChangedEvent(pinNumber, OnPinChanged);
#else
            _pinChangeSimulationTimer.Stop();
            Debug.WriteLine($"Stoped to listening to pin {pinNumber}");
#endif
        }

        private void OnPinChanged(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            Console.WriteLine($"{nameof(OnPinChanged)}: {pinValueChangedEventArgs.PinNumber} with value of {pinValueChangedEventArgs.ChangeType}");
            PinStateChanged?.Invoke(this, new PinStateChanged(pinValueChangedEventArgs.PinNumber, (pinValueChangedEventArgs.ChangeType == PinEventTypes.Rising)));
        }
    }
}
