using IoT.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Models
{
    public class AgentModel
    {
        public AgentModel(AgentEntity entity)
        {
            Entity = entity;
        }

        public AgentEntity Entity { get; }
        public List<NodeModel> Nodes { get; private set; } = new List<NodeModel>();

        public void UpdateRunState(RunState entityRunState)
        {
            if (Entity == null)
                return;

            Entity.RunState = entityRunState;
        }

        public string GetQueueName()
        {
            return "Agent_" + Entity.MachineName + "_Queue";
        }
    }
}
