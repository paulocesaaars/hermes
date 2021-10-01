using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using System;
using System.Threading.Tasks;

namespace Deviot.Hermes.Domain.Interfaces
{
    public interface IDrive
    {
        public Guid Id { get; }

        public string Name { get; }

        public DeviceTypeEnumeration Type { get; }

        public bool Status { get; }

        public bool StatusConnection { get; }

        public void SetDevice(Device device);

        public void UpdateDrive(Device device);

        public void Start();

        public void Stop();

        public Task<object> GetDataAsync(string json);

        public Task SetDataAsync(string data);
    }
}
