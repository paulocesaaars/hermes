using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Infra.ModbusRtu.Configurations;
using Deviot.Hermes.Infra.ModbusTcp.Configurations;
using FluentValidation;
using FluentValidation.Results;
using System.Text.Json;

namespace Deviot.Hermes.Application.Services
{
    public class DeviceValidationService : IDeviceValidationService
    {
        private readonly IValidator<ModbusTcpConfiguration> _modbusTcpValidation;
        private readonly IValidator<ModbusRtuConfiguration> _modbusRtuValidation;

        private const string UNKNOWN_DRIVER = "Driver desconhecido";
        public DeviceValidationService(IValidator<ModbusTcpConfiguration> modbusTcpValidation,
                                       IValidator<ModbusRtuConfiguration> modbusRtuValidation)
        {
            _modbusTcpValidation = modbusTcpValidation;
            _modbusRtuValidation = modbusRtuValidation;
        }

        public ValidationResult Validate(Device device)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (DeviceTypeEnumeration.ModbusTcp.Equals(device.Type))
            {
                var configuration = JsonSerializer.Deserialize<ModbusTcpConfiguration>(device.Configuration, options);
                return _modbusTcpValidation.Validate(configuration);
            }
            else if (DeviceTypeEnumeration.ModbusRtu.Equals(device.Type))
            {
                var configuration = JsonSerializer.Deserialize<ModbusRtuConfiguration>(device.Configuration, options);
                return _modbusRtuValidation.Validate(configuration);
            }

            var result = new ValidationResult();
            result.Errors.Add(new ValidationFailure(nameof(Device), UNKNOWN_DRIVER));

            return result;
        }
    }
}
