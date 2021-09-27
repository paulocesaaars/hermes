using Deviot.Common;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Application.Mappings;
using Deviot.Hermes.Application.Services;
using Deviot.Hermes.Application.Validators;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Infra.ModbusTcp.Configurations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Deviot.Hermes.Application.Configurations
{
    public static class DependencyInjectionApplication
    {
        public static IServiceCollection AddDependencyInjectionApplication(this IServiceCollection services)
        {
            // Commons
            services.AddScoped<INotifier, Notifier>();

            // Automapper
            services.AddAutoMapper(typeof(EntityToViewModelMapping),
                                   typeof(ViewModelToEntityMapping));

            // Validations
            services.AddScoped<IValidator<LoginViewModel>>(v => new LoginValidation());
            services.AddScoped<IValidator<UserViewModel>>(v => new UserValidation());
            services.AddScoped<IValidator<UserInfoViewModel>>(v => new UserInfoValidation());
            services.AddScoped<IValidator<UserPasswordViewModel>>(v => new UserPasswordValidation());
            services.AddScoped<IValidator<DeviceViewModel>>(v => new DeviceValidation());
            services.AddScoped<IValidator<ModbusTcpConfiguration>>(v => new ModbusTcpConfigurationValidation());

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDeviceService, DeviceService>();

            return services;
        }
    }
}
