using System.Collections.Generic;

namespace Deviot.Hermes.Infra.Modbus.Entities
{
    public class ModbusDeviceData
    {
        public IEnumerable<DigitalData> Coils { get; private set; }

        public IEnumerable<DigitalData> Discrete { get; private set; }

        public IEnumerable<AnalogicData> HoldingRegisters { get; private set; }

        public IEnumerable<AnalogicData> InputRegisters { get; private set; }

        public ModbusDeviceData(IEnumerable<DigitalData> coils, IEnumerable<DigitalData> discrete, IEnumerable<AnalogicData> holdingRegisters, IEnumerable<AnalogicData> inputRegisters)
        {
            Coils = coils;
            Discrete = discrete;
            HoldingRegisters = holdingRegisters;
            InputRegisters = inputRegisters;
        }
    }
}
