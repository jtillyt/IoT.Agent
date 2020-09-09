using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public class NumericNodeValueReceivedEvent : EventBase
    {
        public NumericNodeValueReceivedEvent(int nodeTypeId, int nodeId, int valueTypeId, double value)
        {
            NodeTypeId = nodeTypeId;
            NodeId = nodeId;
            ValueTypeId = valueTypeId;
            Value = value;
        }

        public int NodeTypeId { get; }
        public int NodeId { get; }
        public int ValueTypeId { get; }
        public double Value { get; }

        public static NumericNodeValueReceivedEvent Create(int nodeTypeId, int nodeId, int valueTypeId, double value)
        {
            return new NumericNodeValueReceivedEvent(nodeTypeId, nodeId, valueTypeId, value);
        }
    }
}
