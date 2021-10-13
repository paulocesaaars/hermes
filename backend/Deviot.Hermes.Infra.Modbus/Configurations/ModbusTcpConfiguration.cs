namespace Deviot.Hermes.Infra.Modbus.Configurations
{
    public class ModbusTcpConfiguration
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public int Scan { get; set; }

        public int NumberOfCoils { get; set; }

        public int NumberOfDiscrete { get; set; }

        public int NumberOfHoldingRegisters { get; set; }

        public int NumberOfInputRegisters { get; set; }

        public int MaxNumberOfReadAttempts { get; set; }
    }
}
