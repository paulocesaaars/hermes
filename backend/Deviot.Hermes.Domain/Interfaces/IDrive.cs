using Deviot.Hermes.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Deviot.Hermes.Domain.Interfaces
{
    public interface IDrive : IDisposable
    {
        public bool Status { get; }

        public bool StatusConnection { get; }

        public Device Device { get; }

        public void AddConfiguration(string configuration);

        public Task StartAsync();

        public Task StopAsync();

        public Task<object> GetDataAsync(string json);

        public Task SetDataAsync(string data);
    }
}
