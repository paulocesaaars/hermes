using System.Threading.Tasks;

namespace Deviot.Hermes.Domain.Interfaces
{
    public interface IDrive
    {
        public bool Status { get; }

        public bool StatusConnection { get; }

        public Task StartAsync();

        public Task StopAsync();

        public Task RestartAsync();

        public Task UpdateAsync(object device);

        public Task<object> GetDataAsync(object query);

        public Task SetDataAsync(object data);
    }
}
