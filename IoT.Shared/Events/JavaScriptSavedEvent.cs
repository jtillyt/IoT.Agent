using IoT.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Shared.Events
{
    public class JavaScriptSavedEvent:EventBase
    {
        public string ViewId { get; set; }
        public string Script { get; set; }

        public static JavaScriptSavedEvent Create(string viewId, string script)
        {
            var scriptEvent = new JavaScriptSavedEvent
            {
                ViewId = viewId,
                Script = script
            };

            return scriptEvent;
        }
    }
}
