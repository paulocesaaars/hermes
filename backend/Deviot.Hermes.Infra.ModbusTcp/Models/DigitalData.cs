using System;

namespace Deviot.Hermes.Infra.ModbusTcp.Models
{
    public class DigitalData
    {
        public int Address { get; set; }

        public Byte Value { get; set; }
    }
}
