namespace Deviot.Hermes.Infra.Modbus.Model
{
    public class AnalogicData
    {
        public int Address { get; private set; }

        public ushort? Value { get; private set; }

        public bool Quality { get; private set; }

        public AnalogicData(int address, ushort? value, bool quality)
        {
            Address = address;
            Value = value;
            Quality = quality;
        }

        public void SetValue(ushort? value, bool quality = true)
        {
            Value = value;
            Quality = quality;
        }

        public void SetBadRequest()
        {
            Quality = false;
        }
    }
}
