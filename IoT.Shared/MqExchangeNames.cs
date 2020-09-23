using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared
{
    public static class MqExchangeNames
    {
        public const string HubQueueNamePrefix = "IoT.Entities.Hubs.";
        public static string HubStateSent { get; } = HubQueueNamePrefix + "State.Updated";

        public const string AgentQueueNamePrefix = "IoT.Entities.Agents.";

        public static string AgentStateUpdateRequest{ get; } = AgentQueueNamePrefix + "State.Update.Request";
        public static string AgentStatusSent{ get; } = AgentQueueNamePrefix + "Status.Published";
        public static string AgentStatusRequest {get; } = AgentQueueNamePrefix + "Status.Request";
        public static string AgentRunStateSent{get; } = AgentQueueNamePrefix + "RunState.Published";
    }
}
