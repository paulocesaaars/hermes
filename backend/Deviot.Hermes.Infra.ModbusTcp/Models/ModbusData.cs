using System.Collections.Generic;

namespace Deviot.Hermes.Infra.ModbusTcp.Models
{
    public class ModbusData
    {
        public List<DigitalData> CoilStatus { get; set; }

        public List<DigitalData> InputStatus { get; set; }

        public List<AnalogicData> HoldingRegisters { get; set; }

        public List<AnalogicData> InputRegisters { get; set; }

        public ModbusData()
        {
            CoilStatus = new List<DigitalData>();
            InputStatus = new List<DigitalData>();
            HoldingRegisters = new List<AnalogicData>();
            InputRegisters = new List<AnalogicData>();
        }
    }
}
