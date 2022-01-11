using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using FluentValidation.Results;
using System;
using System.Threading.Tasks;

namespace Deviot.Hermes.Domain.Interfaces
{
    public interface IDrive
    {
        public Guid Id { get; }

        public string Name { get; }

        public DeviceTypeEnumeration Type { get; }

        public bool Enable { get; }

        public bool StatusConnection { get; }

        public ValidationResult ValidateConfiguration(string deviceConfiguration);

        public Task SetConfiguration(Device device);

        public Task StartAsync();

        public Task StopAsync();

        public Task<object> GetDataAsync();

        public ValidationResult ValidateWriteData(string data);

        public Task SetDataAsync(string data);
    }
}
