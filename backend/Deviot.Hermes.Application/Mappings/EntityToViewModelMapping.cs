using AutoMapper;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;

namespace Deviot.Hermes.Application.Mappings
{
    public class EntityToViewModelMapping : Profile
    {
        public EntityToViewModelMapping()
        {
            AllowNullCollections = true;

            CreateMap<User, UserInfoViewModel>();
            CreateMap<DeviceTypeEnumeration, EnumerationViewModel>();
            CreateMap<Device, DeviceInfoViewModel>();
            CreateMap<Device, DeviceViewModel>().ConvertUsing<DeviceViewModelConvertMapping>();
        }
    }
}
