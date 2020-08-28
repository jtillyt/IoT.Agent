using IoT.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class UpdateAgentStateEvent:IEvent
    {
        public AgentEntity Agent {get;set; }
    }
}
