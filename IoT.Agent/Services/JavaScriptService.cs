using IoT.EventBus;
using IoT.Shared.Models;
using IoT.Shared.Events;
using Microsoft.Extensions.Logging;
using Jint;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Device.Gpio;
using System.Collections.Generic;
using IoT.ServiceHost.Gpio;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace IoT.ServiceHost
{
    public class JavaScriptService:IHostedService
    {
        private Engine _engine;
        private ILogger<JavaScriptService> _logger;
        private IEventBus _eventBus;

        private GpioWrapper _gpio;

        public JavaScriptService(ILogger<JavaScriptService> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
            _gpio = new GpioWrapper();
        }

        private void OutToLog(object log)
        {
            if (log == null)
                return;

            _logger.LogInformation(log.ToString());
        }

        protected void OnServiceScriptUpdated(JavaScriptSavedEvent serviceEvent)
        {
            _logger.LogDebug("Script updated");

            var script = serviceEvent.Script;

            try
            {
                _engine.Execute(script);
            }
            catch (Exception ex)
            {
                _logger.LogError("error executing script", ex);
            }
        }

        private void ResetEngine()
        {
            _engine = new Engine(cfg => cfg.AllowClr());
            _engine.SetValue("wait", new Action<int>(time => System.Threading.Thread.Sleep(time)));

            _engine.SetValue("print", new Action<object>(a => Debug.WriteLine(a)));
            _engine.SetValue("log", new Action<object>(a => OutToLog(a)));

            _engine.SetValue("setPinAsOut", new Action<int>(pin => _gpio.SetOutputPin(pin)));
            _engine.SetValue("setPinAsIn", new Action<int>(pin => _gpio.SetInputPin(pin)));

            _engine.SetValue("writeBool", new Action<int, bool>((pin, val) => _gpio.WriteBoolOut(pin, val)));
            _engine.SetValue("readBool", new Func<int, bool>((pin) => _gpio.ReadPinState(pin)));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting javacript service");

            ResetEngine();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
