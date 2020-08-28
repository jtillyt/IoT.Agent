using IoT.Shared.Events;
using System;

namespace IoT.EventBus
{
    public interface IEventBus
    {
        IObservable<IEvent> EventStream { get; set; }
        void Publish(IEvent evt);
    }
}
