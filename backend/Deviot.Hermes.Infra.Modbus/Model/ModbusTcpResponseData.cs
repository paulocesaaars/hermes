using System.Collections.Generic;
using System.Linq;

namespace Deviot.Hermes.Infra.Modbus.Model
{
    public class ModbusTcpResponseData
    {
        public int MaxNumberAddress { get; private set; }

        public IEnumerable<DigitalData> Coils { get; private set; }

        public IEnumerable<DigitalData> Discrete { get; private set; }

        public IEnumerable<AnalogicData> HoldingRegisters { get; private set; }

        public IEnumerable<AnalogicData> InputRegisters { get; private set; }

        public ModbusTcpResponseData(IEnumerable<DigitalData> coils, IEnumerable<DigitalData> discrete, IEnumerable<AnalogicData> holdingRegisters, IEnumerable<AnalogicData> inputRegisters)
        {
            Coils = coils;
            Discrete = discrete;
            HoldingRegisters = holdingRegisters;
            InputRegisters = inputRegisters;

            MaxNumberAddress = coils.Count();
            if (MaxNumberAddress < discrete.Count())
                MaxNumberAddress = discrete.Count();

            if(MaxNumberAddress < holdingRegisters.Count())
                MaxNumberAddress = holdingRegisters.Count();

            if(MaxNumberAddress < inputRegisters.Count())
                MaxNumberAddress= inputRegisters.Count();
        }
    }
}
