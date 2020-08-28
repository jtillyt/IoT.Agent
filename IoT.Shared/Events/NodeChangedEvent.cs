using IoT.Shared.Entities;
using IoT.Shared.Events;
using IoT.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Shared.Events
{
    public class NodeChangedEvent : EventBase
    {
        public NodeModel NodeModel {get;set; }

        public static NodeChangedEvent Create(NodeModel nodeModel)
        {
            var nodeEvent = new NodeChangedEvent();

            nodeEvent.NodeModel = nodeModel;

            return nodeEvent;
        }
    }
}
