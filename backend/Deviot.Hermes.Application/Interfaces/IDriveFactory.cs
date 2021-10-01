using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Interfaces;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IDriveFactory
    {
        public IDrive GenerateDrive(Device device);
    }
}
