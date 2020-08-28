using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared
{
    public interface IHasEntityId
    {
        Guid EntityId {get; }
    }
}
