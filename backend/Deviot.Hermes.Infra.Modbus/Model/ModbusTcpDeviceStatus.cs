namespace Deviot.Hermes.Infra.Modbus.Model
{
    public class ModbusTcpDeviceStatus
    {
        public ModbusTcpDevice Device { get; private set; }

        public ModbusTcpResponseData Data { get; private set; }

        public ModbusTcpDeviceStatus(ModbusTcpDevice device, ModbusTcpResponseData data)
        {
            Device = device;
            Data = data;
        }
    }
}
