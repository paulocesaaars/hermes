using Deviot.Hermes.Infra.Modbus.Configurations;
using FluentValidation;
using System.Net;

namespace Deviot.Hermes.Infra.Modbus.Validators
{
    public class ModbusTcpConfigurationValidation : AbstractValidator<ModbusTcpConfiguration>
    {
        public ModbusTcpConfigurationValidation()
        {
            RuleFor(x => x.Ip).MinimumLength(6).WithMessage("Ip inválido")
                              .Custom((ip, context) => {
                                if (!IPAddress.TryParse(ip, out IPAddress address))
                                    context.AddFailure("Ip inválido");
                               });

            RuleFor(x => x.Port).InclusiveBetween(1, 1000).WithMessage("A porta de conexão deve ser de 1 a 10000");

            RuleFor(x => x.Scan).InclusiveBetween(1000, 60000).WithMessage("O tempo de scan deve ser de 1000 a 60000 milisegundos");

            RuleFor(x => x.NumberOfCoils).InclusiveBetween(0, 10000).WithMessage("O número de posições de entradas digitais deve ser de 0 a 10000");

            RuleFor(x => x.NumberOfDiscrete).InclusiveBetween(0, 10000).WithMessage("O número de posições de saídas digitais deve ser de 0 a 10000");

            RuleFor(x => x.NumberOfHoldingRegisters).InclusiveBetween(0, 10000).WithMessage("O número de posições de entradas analógicas deve ser de 0 a 10000");

            RuleFor(x => x.NumberOfInputRegisters).InclusiveBetween(0, 10000).WithMessage("O número de posições de saídas analógicas deve ser de 0 a 10000");
        }
    }
}
