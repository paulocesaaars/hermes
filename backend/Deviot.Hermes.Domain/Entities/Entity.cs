using System;

namespace Deviot.Hermes.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
