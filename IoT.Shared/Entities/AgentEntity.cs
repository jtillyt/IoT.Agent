using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Entities
{
    public class AgentEntity:EntityBase
    {
        public AgentEntity()
        {
            EntityType = EntityType.Agent;
        }

        public string Name {get;set; }
        public string MachineName {get;set; }
        public RunState RunState {get;set; }
        public DateTime? LocalModifiedDateTime {get;set; }
        public string AgentAdminUri {get;set; }

        public string OsType {get;set; }
        public string OsVersion {get;set; }
        public int ProcessorCount { get; set; }
        public bool Is64BitOs { get; set; }
    }
}
