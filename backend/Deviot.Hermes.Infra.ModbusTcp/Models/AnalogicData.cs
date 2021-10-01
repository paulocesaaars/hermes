using System;

namespace Deviot.Hermes.Infra.ModbusTcp.Models
{
    public class AnalogicData
    {
        public int Address { get; set; }

        public ushort Value { get; set; }
    }
}
