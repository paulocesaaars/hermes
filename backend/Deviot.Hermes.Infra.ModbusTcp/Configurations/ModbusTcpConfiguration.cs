namespace Deviot.Hermes.Infra.ModbusTcp.Configurations
{
    public class ModbusTcpConfiguration
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public int CoilStatus { get; set; }

        public int InputStatus { get; set; }

        public int HoldingRegister { get; set; }

        public int InputRegister { get; set; }
    }
}
