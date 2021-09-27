using Deviot.Common;

namespace Deviot.Hermes.Domain.Enumerators
{
    public class DeviceTypeEnumeration : Enumeration
    {
        public static DeviceTypeEnumeration Nenhum = new DeviceTypeEnumeration(0, "Nenhum");

        public static DeviceTypeEnumeration ModbusRtu = new DeviceTypeEnumeration(1, "Modbus RTU");

        public static DeviceTypeEnumeration ModbusTcp = new DeviceTypeEnumeration(2, "Modbus TCP");

        public DeviceTypeEnumeration()
        {

        }

        public DeviceTypeEnumeration(int id, string name) : base(id, name)
        {

        }
    }
}
