using IoT.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class EntityRunStateChangedEvent : IEvent, IHasEntityType, IHasEntityId
    {
        public EntityRunStateChangedEvent()
        {
        }

        public Guid EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public RunState EntityRunState { get; set; }
    }
}
