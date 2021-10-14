using AutoMapper;
using Deviot.Common;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Domain.Entities;

namespace Deviot.Hermes.Application.Mappings
{
    public class ViewModelToEntityMapping : Profile
    {
        private static string Serialize(object value)
        {
            return Utils.Serializer(value);
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
