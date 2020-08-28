using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Entities
{
    public class NodeEntity : EntityBase
    {
        public Guid AgentId { get; set; }
        public Guid ServiceId {get;set; }
        public Guid ServiceHostId {get;set; }
        public Guid NodeTypeId {get;set; }

        public string ScriptText {get;set; }
        public string ScriptType {get;set; }
        public string MessageTemplateText { get;  set; }
        public string MessageTemplateType { get;  set; }

        public string DisplayName { get; set; }
        public RunState RunState { get; set; }
    }
}
