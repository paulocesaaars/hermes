using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Deviot.Hermes.Infra.Modbus.Services
{
    public class ModbusRtuDrive : IModbusRtuDrive
    {
        public Guid Id => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public DeviceTypeEnumeration Type => throw new NotImplementedException();

        public bool Enable => throw new NotImplementedException();

        public bool StatusConnection => throw new NotImplementedException();

        

        public Task<object> GetDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task SetDataAsync(string data)
        {
            throw new NotImplementedException();
        }

        public void SetDevice(Device device)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public Task UpdateDriveAsync(Device device)
        {
            throw new NotImplementedException();
        }
    }
}
