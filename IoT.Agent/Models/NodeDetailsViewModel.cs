using IoT.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Agent.Models
{
    public class NodeDetailsViewModel
    {
        public string DisplayName { get; set; }
        public string ServiceId {get;set; }
        public string EntityId {get;set; }
        public string ScriptText {get;set; }
        public string ScriptType {get;set; }
        public string MessageTemplateText {get;set; }

        public static NodeDetailsViewModel Create(NodeModel nodeModel)
        {
            var vm = new NodeDetailsViewModel();

            vm.DisplayName = nodeModel.DisplayName;
            vm.ServiceId = nodeModel.NodeEntity.ServiceId.ToString();
            vm.EntityId = nodeModel.NodeEntity.EntityId.ToString();
            vm.ScriptText = nodeModel.ScriptText;
            vm.ScriptType = nodeModel.ScriptType;
            vm.MessageTemplateText = nodeModel.MessageTemplateText;

            return vm;
        }
    }
}
