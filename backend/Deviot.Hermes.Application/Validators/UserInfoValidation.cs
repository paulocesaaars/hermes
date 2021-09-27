using Deviot.Common;
using Deviot.Hermes.Application.Bases;
using Deviot.Hermes.Application.ViewModels;
using FluentValidation;

namespace Deviot.Hermes.Application.Validators
{
    public class UserInfoValidation : ValidatorEntityViewModelBase<UserInfoViewModel>
    {
        public UserInfoValidation()
        {
            RuleFor(x => x.FullName).MinimumLength(5).WithMessage("O nome completo precisa ter no mínimo 5 caracteres")
                                .MaximumLength(150).WithMessage("O nome completo precisa ter no máximo 150 caracteres");

            RuleFor(x => x.UserName).MinimumLength(5).WithMessage("O nome de usuário precisa ter no mínimo 5 caracteres")
                                    .MaximumLength(20).WithMessage("O nome de usuário precisa ter no máximo 20 caracteres")
                                    .Custom((userName, context) => {
                                            if(!Utils.ValidateAlphanumericWithUnderline(userName))
                                                context.AddFailure("O nome de usuário precisar ter somente valores alfanuméricos ou underline");
                                    });
        }
    }
}
