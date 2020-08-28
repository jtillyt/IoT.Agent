using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Entities
{
    public abstract class EntityBase : IHasEntityType, IHasEntityId
    {
        public EntityType EntityType { get; protected set; }
        public Guid EntityId { get; set; }
    }
}
