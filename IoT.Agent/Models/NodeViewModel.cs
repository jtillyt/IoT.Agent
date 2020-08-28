using IoT.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Agent.Models
{
    public class NodeViewModel
    {
        public string DisplayName { get; set; }
        public string EntityId {get;set; }
        public string ServiceId { get; set; }

        public static NodeViewModel Create(NodeModel nodeModel)
        {
            var vm = new NodeViewModel();

            vm.DisplayName = nodeModel.DisplayName;
            vm.EntityId = nodeModel.NodeEntity.EntityId.ToString();
            vm.ServiceId = nodeModel.NodeEntity.ServiceId.ToString();

            return vm;
        }
    }
}
