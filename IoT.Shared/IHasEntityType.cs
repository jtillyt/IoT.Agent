using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared
{
    public interface IHasEntityType
    {
        EntityType EntityType {get; }
    }
}
