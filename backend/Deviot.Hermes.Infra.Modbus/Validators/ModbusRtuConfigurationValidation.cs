using Deviot.Hermes.Infra.Modbus.Configurations;
using FluentValidation;

namespace Deviot.Hermes.Infra.Modbus.Validators
{
    public class ModbusRtuConfigurationValidation : AbstractValidator<ModbusRtuConfiguration>
    {
        public ModbusRtuConfigurationValidation()
        {
            
        }
    }
}
