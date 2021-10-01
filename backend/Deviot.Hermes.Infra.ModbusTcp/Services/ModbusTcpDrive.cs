using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Interfaces;
using Deviot.Hermes.Infra.ModbusTcp.Configurations;
using EasyModbus;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Deviot.Hermes.Infra.ModbusTcp.Services
{
    public class ModbusTcpDrive : IModbusTcpDrive
    {
        private ModbusTcpConfiguration _modbusTcpConfiguration = new ModbusTcpConfiguration();
        private ModbusClient _modbusClient;

        public ModbusTcpDrive()
        {

        }

        public bool Status => throw new NotImplementedException();

        public bool StatusConnection => throw new NotImplementedException();

        public Device Device => throw new NotImplementedException();

        public void AddConfiguration(string configuration)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var modbusConfiguration = JsonSerializer.Deserialize<ModbusTcpConfiguration>(configuration, options);
            _modbusTcpConfiguration = modbusConfiguration;
        }

        public async Task StartAsync()
        {
            _modbusClient = new ModbusClient(_modbusTcpConfiguration.Ip, _modbusTcpConfiguration.Port);
            _modbusClient.Connect();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetDataAsync(string json)
        {
            throw new NotImplementedException();
        }

        public Task SetDataAsync(string data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if(_modbusClient.Connected)
                _modbusClient.Disconnect();
        }
    }
}
