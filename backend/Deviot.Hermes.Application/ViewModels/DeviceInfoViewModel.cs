namespace Deviot.Hermes.Application.ViewModels
{
    public class DeviceInfoViewModel : EntityViewModel
    {
        public string Name { get; set; }

        public int TypeId { get; set; }

        public EnumerationViewModel Type { get; set; }

        public bool Enabled { get; set; }

        public bool StatusConnection { get; set; }
    }
}
