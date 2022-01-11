using Deviot.Hermes.Infra.Modbus.Enums;

namespace Deviot.Hermes.Infra.Modbus.Model
{
    public class ModbusTcpWriteData
    {
        public ModbusTypeDataEnum TypeData { get; private set; }

        public int Address { get; private set; }

        public string Value { get; private set; }

        public ModbusTcpWriteData(ModbusTypeDataEnum typeData, int address, string value)
        {
            TypeData = typeData;
            Address = address;
            Value = value;
        }
    }
}
