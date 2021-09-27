using Deviot.Hermes.Domain.Enumerators;
using System;

namespace Deviot.Hermes.Domain.Entities
{
    public class Device : Entity
    {
        public string Name { get; private set; }

        public int TypeId { get; private set; }

        public string Configuration { get; private set; }

        public DeviceTypeEnumeration Type { get => DeviceTypeEnumeration.FromIdOrDefault<DeviceTypeEnumeration>(TypeId, DeviceTypeEnumeration.Nenhum); }

        public Device()
        {

        }

        public Device(Guid id, string name, int typeId, string configuration)
        {
            Id = id;
            Name = name;
            TypeId = typeId;
            Configuration = configuration;
        }

        public void SetName(string value) => Name = value;

        public void SetType(int value) => TypeId = value;

        public void SetType(DeviceTypeEnumeration value) => TypeId = value.Id;

        public void SetConfiguration(string value) => Configuration = value;
    }
}
