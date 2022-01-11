using AutoMapper;
using Deviot.Common;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Infra.Modbus.Configurations;
using System;

namespace Deviot.Hermes.Application.Mappings
{
    public class DeviceViewModelConvertMapping : ITypeConverter<Device, DeviceViewModel>
    {
        private static object Convert(Device device)
        {
            try
            {
                if (DeviceTypeEnumeration.ModbusTcp.Equals(device.Type))
                    return Utils.Deserializer<ModbusTcpConfiguration>(device.Configuration);

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DeviceViewModel Convert(Device source, DeviceViewModel destination, ResolutionContext context)
        {
            var type = new EnumerationViewModel
            {
                Id = source.Type.Id,
                Name = source.Type.Name
            };

            var configuration = Convert(source);

            return new DeviceViewModel
            {
                Id = source.Id,
                Name = source.Name,
                Enabled = source.Enabled,
                Type = type,
                TypeId = source.TypeId,
                Configuration = configuration
            };
        }
    }
}
