using Newtonsoft.Json;
using IoT.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Shared.EventBus
{
    public class MqSendRequestedEvent:IEvent
    {
        public MqSendRequestedEvent()
        {
        }

        public string Exchange { get; set; }
        public string QueueName { get; set; }
        public byte[] Data { get; set; }

        public static MqSendRequestedEvent Create<T>(string exchange, string queueName, T thing) where T : class
        {
            MqSendRequestedEvent mqEvent = new MqSendRequestedEvent();

            var eventString = JsonConvert.SerializeObject(thing);
            var eventData = Encoding.ASCII.GetBytes(eventString);

            mqEvent.Data = eventData;
            mqEvent.QueueName = queueName;
            mqEvent.Exchange = exchange;

            return mqEvent;
        }
    }
}
