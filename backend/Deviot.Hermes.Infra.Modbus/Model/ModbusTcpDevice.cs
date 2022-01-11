using Deviot.Hermes.Domain.Enumerators;
using System;

namespace Deviot.Hermes.Infra.Modbus.Model
{
    public class ModbusTcpDevice
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public DeviceTypeEnumeration Type { get; }

        public bool Enable { get; }

        public bool StatusConnection { get; }

        public ModbusTcpDevice(Guid id, string name, DeviceTypeEnumeration type, bool enable, bool statusConnection)
        {
            Id = id;
            Name = name;
            Type = type;
            Enable = enable;
            StatusConnection = statusConnection;
        }
    }
}
