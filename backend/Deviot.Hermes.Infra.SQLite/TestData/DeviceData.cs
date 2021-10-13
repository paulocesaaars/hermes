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
            configuration.AppendLine("  \"scan\": 1000,");
            configuration.AppendLine("  \"numberOfCoils\": 0,");
            configuration.AppendLine("  \"numberOfDiscrete\": 0,");
            configuration.AppendLine("  \"numberOfHoldingRegisters\": 10,");
            configuration.AppendLine("  \"numberOfInputRegisters\": 0,");
            configuration.AppendLine("  \"maxNumberOfReadAttempts\": 3");
            configuration.AppendLine("}");

            var devices = new List<Device>();
            devices.Add(new Device(new Guid("7011423f65144a2fb1d798dec19cf466"), "Device1", 2, true, configuration.ToString()));

            return devices;
        }
    }
}
