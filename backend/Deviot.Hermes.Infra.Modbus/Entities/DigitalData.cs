namespace Deviot.Hermes.Infra.Modbus.Entities
{
    public class DigitalData
    {
        public int Address { get; private set; }

        public bool? Value { get; private set; }

        public bool Quality { get; private set; }

        public DigitalData(int address, bool? value, bool quality)
        {
            Address = address;
            Value = value;
            Quality = quality;
        }

        public void SetValue(bool? value, bool quality = true)
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
