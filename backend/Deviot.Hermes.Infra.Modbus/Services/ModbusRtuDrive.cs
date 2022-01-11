using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Domain.Interfaces;
using FluentValidation.Results;
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

        public Task SetConfiguration(Device device)
        {
            throw new NotImplementedException();
        }

        public Task SetDataAsync(string data)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }

        public ValidationResult ValidateConfiguration(string deviceConfiguration)
        {
            throw new NotImplementedException();
        }

        public ValidationResult ValidateWriteData(string data)
        {
            throw new NotImplementedException();
        }
    }
}
