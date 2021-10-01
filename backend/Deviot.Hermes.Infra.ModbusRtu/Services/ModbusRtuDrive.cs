using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Deviot.Hermes.Infra.ModbusRtu.Services
{
    public class ModbusRtuDrive : IModbusRtuDrive
    {
        public ModbusRtuDrive()
        {

        }

        public bool Status => throw new NotImplementedException();

        public bool StatusConnection => throw new NotImplementedException();

        public Device Device => throw new NotImplementedException();

        public Task StartAsync()
        {
            throw new NotImplementedException();
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

        public void AddConfiguration(string configuration)
        {
            throw new NotImplementedException();
        }
    }
}
