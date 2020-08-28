using IoT.Shared.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class SendEntityStateRequestedEvent : IEvent, IHasEntityType, IHasEntityId,IHasQueueName
    {
        public Guid EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public string QueueName {get;set; }
    }
}
