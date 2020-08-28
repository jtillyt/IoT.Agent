using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Events
{
    public interface IHasQueueName
    {
        string QueueName {get;set; }
    }
}
