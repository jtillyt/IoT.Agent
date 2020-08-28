using IoT.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Shared.Events
{
    public class NodeMessageTemplateUpdatedEvent : EventBase
    {
        public string NodeId { get; set; }
        public string MessageTemplate { get; set; }
        public string MessageTemplateType { get; set; }

        public static NodeMessageTemplateUpdatedEvent Create(string nodeId, string messageTemplate, string messageTemplateType)
        {
            var messageEvent = new NodeMessageTemplateUpdatedEvent();

            messageEvent.NodeId = nodeId;
            messageEvent.MessageTemplate = messageTemplate;
            messageEvent.MessageTemplateType = messageTemplateType;

            return messageEvent;
        }
    }
}
