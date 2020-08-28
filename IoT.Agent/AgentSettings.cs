using IoT.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Agent
{
    public static class AgentSettings
    {
        static AgentSettings()
        {
        }

        public static AgentEntity Agent {get; set;}

    }
}
