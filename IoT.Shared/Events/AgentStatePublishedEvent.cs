using IoT.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class AgentStatePublishedEvent : EventBase, IAgentEvent
    {
        public AgentEntity Agent { get; set; }
        public Guid EntityId { get; set; }

        public static AgentStatePublishedEvent Create(AgentEntity entity)
        {
            return new AgentStatePublishedEvent() { Agent = entity, EntityId = entity.EntityId };
        }
    }
}
