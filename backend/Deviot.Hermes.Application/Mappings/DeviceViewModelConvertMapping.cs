using AutoMapper;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Infra.ModbusTcp.Configurations;
using System;
using System.Text.Json;

namespace Deviot.Hermes.Application.Mappings
{
    public class DeviceViewModelConvertMapping : ITypeConverter<Device, DeviceViewModel>
    {
        private static object Convert(Device device)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                if (DeviceTypeEnumeration.ModbusTcp.Equals(device.Type))
                    return JsonSerializer.Deserialize<ModbusTcpConfiguration>(device.Configuration, options);

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
                Type = type,
                TypeId = source.TypeId,
                Configuration = configuration
            };
        }
    }
}
