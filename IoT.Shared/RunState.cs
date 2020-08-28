using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared
{
    public enum RunState
    {
        Unknown = 0,
        Starting = 1,
        Started = 2,
        Stopping = 3,
        Stopped = 4
    }
}
