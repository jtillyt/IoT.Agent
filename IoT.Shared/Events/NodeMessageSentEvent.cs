using IoT.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Shared.Events
{
    public class NodeMessageSentEvent : EventBase
    {
        public string FromNodeId { get; set; }
        public string MessageText { get; set; }
        public string MessageType {get;set; }
        public string ToNodeId { get; set; }

        public static NodeMessageSentEvent Create(string fromNodeId, string toNodeId, string messageText, string messageType)
        {
            var messageEvent = new NodeMessageSentEvent();

            messageEvent.FromNodeId = fromNodeId;
            messageEvent.ToNodeId = toNodeId;
            messageEvent.MessageText = messageText;
            messageEvent.MessageType = messageType;

            return messageEvent;
        }
    }
}
