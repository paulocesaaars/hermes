using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Domain.Interfaces;
using Deviot.Hermes.Infra.Modbus.Configurations;
using FluentModbus;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Deviot.Hermes.Infra.Modbus.Services
{
    public class ModbusTcpDrive : IModbusTcpDrive
    {
        private ModbusDeviceDataBase _modbusDeviceDataBase;
        private ModbusTcpConfiguration _modbusTcpConfiguration;

        private readonly ModbusTcpClient _modbusClient;
        private readonly ILogger<ModbusTcpDrive> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private const string ERROR_READ_COILS = "Houve um problema ao ler as entradas digitais";
        private const string ERROR_READ_DICRETE = "Houve um problema ao ler as saídas digitais";
        private const string ERROR_READ_HOLDING_REGISTERS = "Houve um problema ao ler as entradas analógicas";
        private const string ERROR_READ_INPUT_REGISTERS = "Houve um problema ao ler as saídas analógicas";
        private const string ERROR_CONNECT = "Houve um problema ao se conectar no dispositivo";
        private const string ERROR_DISCONNECT = "Houve um problema ao se desconectar no dispositivo";
        private const string ERROR_EXECUTE = "Existe um erro não tratado no driver";
        private const string ERROR_SET_CONFIGURATION = "Houve um problema ao gravar as configurações do dispositivo";
        private const string ERROR_SET_DEVICE = "Houve um problema ao gravar as informações do dispositivo";
        private const string ERROR_UPDATE_DEVICE = "Houve um problema ao atualizar as informações do dispositivo";
        private const string ERROR_START = "Houve ao iniciar o driver";
        private const string ERROR_STOP = "Houve ao parar o driver";
        private const string ERROR_GET_DATA = "Houve um problema ao ler os dados do dispositivo";
        private const string ERROR_SET_DATA = "Houve um problema ao escrever os dados no dispositivo";

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public DeviceTypeEnumeration Type { get; private set; }

        public bool Enable { get; private set; }

        public bool StatusConnection
        {
            get
            {
                try
                {
                    return _modbusClient.IsConnected;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public ModbusTcpDrive(ILogger<ModbusTcpDrive> logger)
        {
            _logger = logger;
            _modbusClient = new ModbusTcpClient();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void ReadCoils()
        {
            try
            {
                var quatity = _modbusTcpConfiguration.NumberOfCoils;
                if (_modbusClient.IsConnected && Enable && quatity > 0)
                {
                    var result = _modbusClient.ReadCoils(0xFF, 0, quatity);
                    _modbusDeviceDataBase.UpdateCoilsValues(result.ToArray());
                    return;
                }

                _modbusDeviceDataBase.UpdateCoilsToBadRequest();
            }
            catch (Exception exception)
            {
                _modbusDeviceDataBase.UpdateCoilsToBadRequest();
                _logger.LogError(ERROR_READ_COILS);
                _logger.LogError(exception.Message);
            }
        }

        private void ReadDiscreteInputs()
        {
            try
            {
                var quatity = _modbusTcpConfiguration.NumberOfDiscrete;
                if (_modbusClient.IsConnected && Enable && quatity > 0)
                {
                    var result = _modbusClient.ReadDiscreteInputs(0xFF, 0, quatity);
                    _modbusDeviceDataBase.UpdateDiscreteValues(result.ToArray());
                    return;
                }

                _modbusDeviceDataBase.UpdateDiscreteToBadRequest();
            }
            catch (Exception exception)
            {
                _modbusDeviceDataBase.UpdateDiscreteToBadRequest();
                _logger.LogError(ERROR_READ_DICRETE);
                _logger.LogError(exception.Message);
            }
        }

        private void ReadHoldingRegisters()
        {
            try
            {
                var quatity = _modbusTcpConfiguration.NumberOfHoldingRegisters;
                if (_modbusClient.IsConnected && Enable && quatity > 0)
                {
                    var result = _modbusClient.ReadHoldingRegisters<ushort>(0xFF, 0, quatity);
                    _modbusDeviceDataBase.UpdateHoldingRegisterValues(result.ToArray());
                    return;
                }

                _modbusDeviceDataBase.UpdateHoldingRegistersToBadRequest();
            }
            catch (Exception exception)
            {
                _modbusDeviceDataBase.UpdateHoldingRegistersToBadRequest();
                _logger.LogError(ERROR_READ_HOLDING_REGISTERS);
                _logger.LogError(exception.Message);
            }
        }

        private void ReadInputRegisters()
        {
            try
            {
                var quatity = _modbusTcpConfiguration.NumberOfInputRegisters;
                if (_modbusClient.IsConnected && Enable && quatity > 0)
                {
                    var result = _modbusClient.ReadInputRegisters<ushort>(0xFF, 0, quatity);
                    _modbusDeviceDataBase.UpdateInputRegisterValues(result.ToArray());
                    return;
                }

                _modbusDeviceDataBase.UpdateInputRegistersToBadRequest();
            }
            catch (Exception exception)
            {
                _modbusDeviceDataBase.UpdateInputRegistersToBadRequest();
                _logger.LogError(ERROR_READ_INPUT_REGISTERS);
                _logger.LogError(exception.Message);
            }
        }

        private async Task ExecuteAsync()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (!Enable)
                    {
                        Disconnect();
                        _modbusDeviceDataBase.UpdateAllDataToBadRequest(true);
                        await Task.Delay(1000, _cancellationTokenSource.Token);
                        continue;
                    }

                    Connect();

                    ReadCoils();
                    ReadDiscreteInputs();
                    ReadHoldingRegisters();
                    ReadInputRegisters();

                    Disconnect();
                }
                catch (Exception exception)
                {
                    _logger.LogError(ERROR_EXECUTE);
                    _logger.LogError(exception.Message);
                }

                await Task.Delay(_modbusTcpConfiguration.Scan, _cancellationTokenSource.Token);
            }

            Disconnect();
        }

        private void Connect()
        {
            try
            {
                if (!_modbusClient.IsConnected && Enable)
                    _modbusClient.Connect(new IPEndPoint(IPAddress.Parse(_modbusTcpConfiguration.Ip), _modbusTcpConfiguration.Port), ModbusEndianness.BigEndian);
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_CONNECT);
                _logger.LogError(exception.Message);
            }
        }

        private void Disconnect()
        {
            try
            {
                if (_modbusClient.IsConnected)
                    _modbusClient.Disconnect();
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_DISCONNECT);
                _logger.LogError(exception.Message);
            }
        }

        private void SetConfiguration(string configuration)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                _modbusTcpConfiguration = JsonSerializer.Deserialize<ModbusTcpConfiguration>(configuration, options);
                _modbusDeviceDataBase = new ModbusDeviceDataBase(_modbusTcpConfiguration.NumberOfCoils, 
                                             _modbusTcpConfiguration.NumberOfDiscrete, 
                                             _modbusTcpConfiguration.NumberOfHoldingRegisters, 
                                             _modbusTcpConfiguration.NumberOfInputRegisters,
                                             _modbusTcpConfiguration.MaxNumberOfReadAttempts);
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_SET_CONFIGURATION);
                _logger.LogError(exception.Message);
            }
        }

        public void SetDevice(Device device)
        {
            try
            {
                SetConfiguration(device.Configuration);

                Id = device.Id;
                Name = device.Name;
                Type = device.Type;
                Enable = device.Enable;
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_SET_DEVICE);
                _logger.LogError(exception.Message);
            }
        }

        public async Task UpdateDriveAsync(Device device)
        {
            try
            {
                Enable = false;
                await Task.Delay(500, _cancellationTokenSource.Token);

                SetDevice(device);
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_UPDATE_DEVICE);
                _logger.LogError(exception.Message);
            }
        }

        public void Start()
        {
            try
            {
                Task.Run(() => ExecuteAsync());
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_START);
                _logger.LogError(exception.Message);
            }
        }

        public void Stop()
        {
            try
            {
                _cancellationTokenSource.Cancel();
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_STOP);
                _logger.LogError(exception.Message);
            }
        }

        public async Task<object> GetDataAsync()
        {
            try
            {
                await Task.Delay(100, _cancellationTokenSource.Token);
                return _modbusDeviceDataBase.GetData();
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_GET_DATA);
                _logger.LogError(exception.Message);
                return null;
            }
        }

        public async Task SetDataAsync(string data)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_SET_DATA);
                _logger.LogError(exception.Message);
            }
        }
    }
}
