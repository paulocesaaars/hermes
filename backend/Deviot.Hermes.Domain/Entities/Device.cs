using Deviot.Hermes.Domain.Enumerators;
using System;

namespace Deviot.Hermes.Domain.Entities
{
    public class Device : Entity
    {
        public string Name { get; private set; }

        public int TypeId { get; private set; }

        public bool Enabled { get; private set; }

        public string Configuration { get; private set; }

        public DeviceTypeEnumeration Type { get => DeviceTypeEnumeration.FromIdOrDefault<DeviceTypeEnumeration>(TypeId, DeviceTypeEnumeration.Nenhum); }

        public Device()
        {

        }

        public Device(Guid id, string name, int typeId, bool enable, string configuration)
        {
            Id = id;
            Name = name;
            TypeId = typeId;
            Enabled = enable;
            Configuration = configuration;
        }

        public void SetName(string value) => Name = value;

        public void SetEnable(bool value) => Enabled = value;

        public void SetConfiguration(string value) => Configuration = value;
    }
}
