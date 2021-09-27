using Deviot.Hermes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviot.Hermes.Infra.SQLite.TestData
{
    public class DeviceData
    {
        public static IEnumerable<Device> GetDevices()
        {
            var configuration = new StringBuilder();
            configuration.AppendLine("{");
            configuration.AppendLine("  \"ip\": \"127.0.0.1\",");
            configuration.AppendLine("  \"port\": 502,");
            configuration.AppendLine("  \"coilStatus\": 0,");
            configuration.AppendLine("  \"inputStatus\": 0,");
            configuration.AppendLine("  \"holdingRegister\": 10,");
            configuration.AppendLine("  \"inputRegister\": 0");
            configuration.AppendLine("}");

            var devices = new List<Device>();
            devices.Add(new Device(new Guid("7011423f65144a2fb1d798dec19cf466"), "Device1", 2, configuration.ToString()));

            return devices;
        }
    }
}
