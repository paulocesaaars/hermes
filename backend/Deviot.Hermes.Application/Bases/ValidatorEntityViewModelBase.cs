using Deviot.Hermes.Application.ViewModels;
using FluentValidation;

namespace Deviot.Hermes.Application.Bases
{
    public abstract class ValidatorEntityViewModelBase<T> : AbstractValidator<T> where T : EntityViewModel
    {
        protected ValidatorEntityViewModelBase()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("O id é obrigatório");
        }
    }
}
