using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Domain.Interfaces;
using System;

namespace Deviot.Hermes.Application.Services
{
    public class DriveFactory : IDriveFactory
    {
        private readonly IModbusTcpDrive _modbusTcpDrive;
        private readonly IModbusRtuDrive _modbusRtuDrive;

        public DriveFactory(IModbusTcpDrive modbusTcpDrive,
                            IModbusRtuDrive modbusRtuDrive)
        {
            _modbusTcpDrive = modbusTcpDrive;
            _modbusRtuDrive = modbusRtuDrive;
        }

        public IDrive GenerateDrive(Device device)
        {
            try
            {
                if(device.Type.Equals(DeviceTypeEnumeration.ModbusTcp))
                {
                    _modbusTcpDrive.SetDevice(device);
                    return _modbusTcpDrive;
                }
                else if (device.Type.Equals(DeviceTypeEnumeration.ModbusRtu))
                {
                    _modbusRtuDrive.SetDevice(device);
                    return _modbusRtuDrive;
                }

                return null;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
