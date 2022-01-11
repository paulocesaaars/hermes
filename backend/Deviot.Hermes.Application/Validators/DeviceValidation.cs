using Deviot.Common;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Domain.Enumerators;
using FluentValidation;

namespace Deviot.Hermes.Application.Validators
{
    public class DeviceValidation : ValidatorEntityViewModelBase<DeviceViewModel>
    {
        public DeviceValidation()
        {
            RuleFor(x => x.Name).MinimumLength(5).WithMessage("O nome do dispositivo precisa ter no mínimo 5 caracteres")
                                .MaximumLength(20).WithMessage("O nome do dispositivo precisa ter no máximo 20 caracteres")
                                .Custom((userName, context) => {
                                        if(!Utils.ValidateAlphanumericWithUnderline(userName))
                                            context.AddFailure("O nome do dispositivo precisar ter somente valores alfanuméricos ou underline");
                                });

            RuleFor(x => x.TypeId).Custom((typeId, context) => {
                var deviceType = DeviceTypeEnumeration.FromIdOrDefault<DeviceTypeEnumeration>(typeId, DeviceTypeEnumeration.Nenhum);
                if (DeviceTypeEnumeration.Nenhum.Id == deviceType.Id)
                    context.AddFailure("Tipo de dispositivo desconhecido");
            });

            RuleFor(x => x.Configuration).NotNull().WithMessage("A configuração do dispositivo não foi informada");
        }
    }
}
