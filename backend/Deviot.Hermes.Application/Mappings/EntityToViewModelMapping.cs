using AutoMapper;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Domain.Entities;

namespace Deviot.Hermes.Application.Mappings
{
    public class EntityToViewModelMapping : Profile
    {
        public EntityToViewModelMapping()
        {
            AllowNullCollections = true;

            CreateMap<User, UserInfoViewModel>();
            CreateMap<Device, DeviceViewModel>().ConvertUsing<DeviceViewModelConvertMapping>();
        }
    }
}
