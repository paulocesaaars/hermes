using AutoMapper;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Domain.Entities;
using System.Text.Json;

namespace Deviot.Hermes.Application.Mappings
{
    public class ViewModelToEntityMapping : Profile
    {
        private static string Serialize(object value)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            return JsonSerializer.Serialize(value, options);
        }

        public ViewModelToEntityMapping()
        {
            AllowNullCollections = true;

            CreateMap<UserViewModel, User>();
            CreateMap<DeviceViewModel, Device>()
                .ForMember(dest => dest.Configuration, opt => opt.MapFrom(src => Serialize(src.Configuration)));
        }
    }
}
