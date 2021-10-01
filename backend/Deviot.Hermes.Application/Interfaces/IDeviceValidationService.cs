using Deviot.Hermes.Domain.Entities;
using FluentValidation.Results;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IDeviceValidationService
    {
        ValidationResult Validate(Device device);
    }
}
