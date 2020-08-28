using IoT.Shared.Events;
using System;
using System.Reactive.Subjects;

namespace IoT.EventBus
{
    public class EventBus : IEventBus
    {
        public EventBus()
        {
            EventStream = new Subject<IEvent>();
        }

        public IObservable<IEvent> EventStream { get; set; }

        public void Publish(IEvent evt)
        {
            if (evt == null)
                return;

            ((Subject<IEvent>)EventStream).OnNext(evt);
        }
    }
}
