using IoT.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Agent.Models
{
    public class AgentDetailViewModel
    {
        public AgentDetailViewModel()
        {
        }

        public AgentDetailViewModel(AgentModel agent)
        {
            EntityId = agent.Entity.EntityId.ToString();
            AgentName = agent.Entity.Name;
            AgentMachineName = agent.Entity.MachineName;
            RunState = agent.Entity.RunState.ToString();
            LocalModifiedDateTime = agent.Entity.LocalModifiedDateTime?.ToLocalTime().ToLongTimeString();

            RunStateLabelId = "RunStateLabel_" + EntityId;
            ModifiedDateTimeLabelId = "ModifyTimeLabel_" + EntityId;
        }

        public string EntityId { get; set; }
        public string AgentName { get; set; }
        public string AgentMachineName { get; set; }
        public string RunState { get; set; }
        public string LocalModifiedDateTime { get; set; }

        public string RunStateLabelId { get; set; }
        public string ModifiedDateTimeLabelId { get; set; }
    }
}
