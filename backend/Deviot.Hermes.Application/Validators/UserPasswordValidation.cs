using Deviot.Common;
using Deviot.Hermes.Application.Bases;
using Deviot.Hermes.Application.ViewModels;
using FluentValidation;

namespace Deviot.Hermes.Application.Validators
{
    public class UserPasswordValidation : ValidatorEntityViewModelBase<UserPasswordViewModel>
    {
        public UserPasswordValidation()
        {
            RuleFor(x => x.Password).MinimumLength(5).WithMessage("A senha precisa ter no mínimo 5 caracteres")
                                    .MaximumLength(10).WithMessage("A senha precisa ter no máximo 10 caracteres")
                                    .Custom((password, context) => {
                                        if (!Utils.ValidateAlphanumeric(password))
                                            context.AddFailure("A senha precisar ter somente valores alfanuméricos");
                                    });

            RuleFor(x => x.NewPassword).MinimumLength(5).WithMessage("A nova senha precisa ter no mínimo 5 caracteres")
                                       .MaximumLength(10).WithMessage("A nova senha precisa ter no máximo 10 caracteres")
                                       .Custom((password, context) => {
                                           if (!Utils.ValidateAlphanumeric(password))
                                               context.AddFailure("A senha precisar ter somente valores alfanuméricos");
                                       });
        }
    }
}
