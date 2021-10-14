using Deviot.Common;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Infra.Modbus.Configurations;
using FluentValidation;
using FluentValidation.Results;

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
            if (DeviceTypeEnumeration.ModbusTcp.Equals(device.Type))
            {
                var configuration = Utils.Deserializer<ModbusTcpConfiguration>(device.Configuration);
                return _modbusTcpValidation.Validate(configuration);
            }
            else if (DeviceTypeEnumeration.ModbusRtu.Equals(device.Type))
            {
                var configuration = Utils.Deserializer<ModbusRtuConfiguration>(device.Configuration);
                return _modbusRtuValidation.Validate(configuration);
            }

            var result = new ValidationResult();
            result.Errors.Add(new ValidationFailure(nameof(Device), UNKNOWN_DRIVER));

            return result;
        }
    }
}
