using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IoT.Agent.Models;
using IoT.EventBus;
using IoT.Shared.Events;
using IoT.Agent.Repos;

namespace IoT.Agent.Controllers
{
    public class DemosController : Controller
    {
        private readonly IEventBus _eventBus;
        private readonly IUserRepo _userRepo;

        public DemosController(IEventBus eventBus, IUserRepo userRepo)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Details(string demoId)
        {
            return View();
        }

        public async Task<ActionResult> Demo1()
        {
            return View();
        }

        public async Task<ActionResult> Demo2()
        {
            return View();
        }

        public async Task<ActionResult> Demo3()
        {
            return View(_userRepo.ListAll());
        }

        [HttpPost]
        public async Task SetScript(string demoId)
        {
            var formData = Request.Form;

            var scriptText = formData["scriptText"];
            var scriptType = formData["scriptType"];


            //node.ScriptText = scriptText;
            //node.ScriptType = scriptType;
        }

        [HttpPost]
        public async Task SetLedState()
        {
            var formData = Request.Form;

            var pinNumberStr = formData["pinNumber"].FirstOrDefault();
            var pinStateStr = formData["pinState"].FirstOrDefault();

            int.TryParse(pinNumberStr, out int pinNumber);
            int.TryParse(pinStateStr, out int pinState);

            //node.ScriptText = scriptText;
            //node.ScriptType = scriptType;
            _eventBus.Publish(PinStateChangeRequested.Create(pinNumber, pinState));
        }

        [HttpPost]
        public async Task ReadTemp()
        {
            var formData = Request.Form;

            var pinNumberStr = formData["pinNumber"].FirstOrDefault();

            int.TryParse(pinNumberStr, out int pinNumber);
            _eventBus.Publish(ReadTempRequested.Create(pinNumber));
        }

        [HttpPost]
        public async Task SetListeningStateForPin()
        {
            var formData = Request.Form;

            var pinNumberStr = formData["pinNumber"].FirstOrDefault();
            var pinListenStateStr = formData["pinListenState"].FirstOrDefault();

            int.TryParse(pinNumberStr, out int pinNumber);
            int.TryParse(pinListenStateStr, out int pinListenState);

            //node.ScriptText = scriptText;
            //node.ScriptType = scriptType;
            _eventBus.Publish(PinListenStateChangeRequested.Create(pinNumber, pinListenState == 1));
        }

        //[HttpPost]
        //public void SendMessageIn(string nodeId)
        //{
        //    var formData = Request.Form;

        //    var messageText = formData["messageText"];
        //    var messageType = formData["messageType"];

        //    _eventBus.Publish(NodeMessageSentEvent.Create(nodeId, nodeId, messageText, messageType));
        //}

        //[HttpPost]
        //public async Task SaveMessageTemplate(string nodeId)
        //{
        //    var formData = Request.Form;

        //    var messageTemplateText = formData["messageTemplateText"];
        //    var messageTeplateType = formData["messageTeplateType"];

        //    var jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(node.MessageTemplateText) as Newtonsoft.Json.Linq.JObject;

        //    foreach(var property in jObject.Properties())
        //    {
        //        string name = property.Name;
        //        string val = property.Value.ToString();
        //        string valType = property.Value.Type.ToString();
        //    }

        //    _eventBus.Publish(NodeMessageTemplateUpdatedEvent.Create(nodeId,  messageTemplateText, messageTeplateType));
        //}
    }
}