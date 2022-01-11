using Deviot.Common;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Domain.Interfaces;
using Deviot.Hermes.Infra.Modbus.Configurations;
using Deviot.Hermes.Infra.Modbus.Enums;
using Deviot.Hermes.Infra.Modbus.Model;
using FluentModbus;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Deviot.Hermes.Infra.Modbus.Services
{
    public class ModbusTcpDrive : IModbusTcpDrive
    {
        private ModbusTcpManageData _modbusTcpManageData;
        private ModbusTcpConfiguration _modbusTcpConfiguration;


        private readonly ILogger<ModbusTcpDrive> _logger;
        private readonly ModbusTcpClient _modbusTcpClient;
        private readonly Queue<ModbusTcpWriteData> _modbusWriteList;
        private readonly IValidator<ModbusTcpWriteData> _dataValidator;
        private readonly IValidator<ModbusTcpConfiguration> _configurationValidator;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private const string ERROR_READ_COILS = "Houve um problema ao ler as entradas digitais";
        private const string ERROR_READ_DICRETE = "Houve um problema ao ler as saídas digitais";
        private const string ERROR_READ_HOLDING_REGISTERS = "Houve um problema ao ler as entradas analógicas";
        private const string ERROR_READ_INPUT_REGISTERS = "Houve um problema ao ler as saídas analógicas";
        private const string ERROR_CONNECT = "Houve um problema ao conectar no dispositivo";
        private const string ERROR_DISCONNECT = "Houve um problema ao desconectar no dispositivo";
        private const string ERROR_EXECUTE = $"Existe um erro não tratado no dispositivo";
        private const string ERROR_START = "Houve um problema ao iniciar o dispositivo";
        private const string ERROR_STOP = "Houve um problema ao parar o dispositivo";
        private const string ERROR_GET_DATA = "Houve um problema ao ler os dados do dispositivo";
        private const string ERROR_SET_DATA = "Houve um problema ao escrever os dados no dispositivo";

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public DeviceTypeEnumeration Type { get; private set; }

        public bool Enable { get; private set; }

        public bool StatusConnection { get; private set; }

        public ModbusTcpDrive(ILogger<ModbusTcpDrive> logger,
                              IValidator<ModbusTcpWriteData> dataValidator,
                              IValidator<ModbusTcpConfiguration> configurationValidator)
        {
            _logger = logger;
            _dataValidator = dataValidator;
            _configurationValidator = configurationValidator;
            _modbusTcpClient = new ModbusTcpClient();
            _modbusWriteList = new Queue<ModbusTcpWriteData>();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void ReadCoils()
        {
            try
            {
                var quatity = _modbusTcpConfiguration.NumberOfCoils;
                if (_modbusTcpClient.IsConnected && Enable && quatity > 0)
                {
                    var result = _modbusTcpClient.ReadCoils(0xFF, 0, quatity);
                    _modbusTcpManageData.UpdateCoilsValues(result);
                    return;
                }

                _modbusTcpManageData.UpdateCoilsToBadRequest();
            }
            catch (Exception exception)
            {
                _modbusTcpManageData.UpdateCoilsToBadRequest();
                _logger.LogError(ERROR_READ_COILS);
                _logger.LogError(exception.Message);
            }
        }

        private void ReadDiscreteInputs()
        {
            try
            {
                var quatity = _modbusTcpConfiguration.NumberOfDiscrete;
                if (_modbusTcpClient.IsConnected && Enable && quatity > 0)
                {
                    var result = _modbusTcpClient.ReadDiscreteInputs(0xFF, 0, quatity);
                    _modbusTcpManageData.UpdateDiscreteValues(result.ToArray());
                    return;
                }

                _modbusTcpManageData.UpdateDiscreteToBadRequest();
            }
            catch (Exception exception)
            {
                _modbusTcpManageData.UpdateDiscreteToBadRequest();
                _logger.LogError(ERROR_READ_DICRETE);
                _logger.LogError(exception.Message);
            }
        }

        private void ReadHoldingRegisters()
        {
            try
            {
                var quatity = _modbusTcpConfiguration.NumberOfHoldingRegisters;
                if (_modbusTcpClient.IsConnected && Enable && quatity > 0)
                {
                    var result = _modbusTcpClient.ReadHoldingRegisters<ushort>(0xFF, 0, quatity);
                    _modbusTcpManageData.UpdateHoldingRegisterValues(result.ToArray());
                    return;
                }

                _modbusTcpManageData.UpdateHoldingRegistersToBadRequest();
            }
            catch (Exception exception)
            {
                _modbusTcpManageData.UpdateHoldingRegistersToBadRequest();
                _logger.LogError(ERROR_READ_HOLDING_REGISTERS);
                _logger.LogError(exception.Message);
            }
        }

        private void ReadInputRegisters()
        {
            try
            {
                var quatity = _modbusTcpConfiguration.NumberOfInputRegisters;
                if (_modbusTcpClient.IsConnected && Enable && quatity > 0)
                {
                    var result = _modbusTcpClient.ReadInputRegisters<ushort>(0xFF, 0, quatity);
                    _modbusTcpManageData.UpdateInputRegisterValues(result.ToArray());
                    return;
                }

                _modbusTcpManageData.UpdateInputRegistersToBadRequest();
            }
            catch (Exception exception)
            {
                _modbusTcpManageData.UpdateInputRegistersToBadRequest();
                _logger.LogError(ERROR_READ_INPUT_REGISTERS);
                _logger.LogError(exception.Message);
            }
        }

        private void WriteData()
        {
            while(_modbusWriteList.Count > 0)
            {
                try
                {
                    var data = _modbusWriteList.Dequeue();
                    if (_modbusTcpClient.IsConnected && Enable)
                    {
                        if (data.TypeData == ModbusTypeDataEnum.Coil)
                        {
                            var value = Boolean.Parse(data.Value);
                            _modbusTcpClient.WriteSingleCoil(0xFF, data.Address, value);
                        }
                        else if (data.TypeData == ModbusTypeDataEnum.HoldingRegister)
                        {
                            var value = Int16.Parse(data.Value);
                            _modbusTcpClient.WriteSingleRegister(0xFF, data.Address, value);
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(ERROR_SET_DATA);
                    _logger.LogError(exception.Message);
                }
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
                        _modbusTcpManageData.UpdateAllDataToBadRequest(true);
                        await Task.Delay(1000, _cancellationTokenSource.Token);
                        continue;
                    }

                    Connect();

                    WriteData();
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
                if (!_modbusTcpClient.IsConnected && Enable)
                    _modbusTcpClient.Connect(new IPEndPoint(IPAddress.Parse(_modbusTcpConfiguration.Ip), _modbusTcpConfiguration.Port), ModbusEndianness.BigEndian);

                StatusConnection = true;
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_CONNECT);
                _logger.LogError(exception.Message);

                StatusConnection = false;
            }
        }

        private void Disconnect()
        {
            try
            {
                if (_modbusTcpClient.IsConnected)
                    _modbusTcpClient.Disconnect();
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_DISCONNECT);
                _logger.LogError(exception.Message);
            }
        }

        private ValidationResult ValidateConfiguration(ModbusTcpConfiguration modbusTcpConfiguration)
        {
            return _configurationValidator.Validate(modbusTcpConfiguration);
        }

        private ValidationResult ValidateWriteData(ModbusTcpWriteData data) 
        {
            return _dataValidator.Validate(data);
        }

        public ValidationResult ValidateConfiguration(string deviceConfiguration)
        {
            var modbusTcpConfiguration = Utils.Deserializer<ModbusTcpConfiguration>(deviceConfiguration);
            return ValidateConfiguration(modbusTcpConfiguration);
        }

        public async Task SetConfiguration(Device device)
        {
            var modbusTcpConfiguration = Utils.Deserializer<ModbusTcpConfiguration>(device.Configuration);
            var validationResult = ValidateConfiguration(device.Configuration);
            if (!validationResult.IsValid)
                throw new Exception(validationResult.Errors[0].ErrorMessage);

            Enable = false;
            await Task.Delay(500, _cancellationTokenSource.Token);

            Id = device.Id;
            Name = device.Name;
            Type = device.Type;
            Enable = device.Enabled;
            _modbusTcpConfiguration = modbusTcpConfiguration;
            _modbusTcpManageData = new ModbusTcpManageData(modbusTcpConfiguration.NumberOfCoils,
                                                          modbusTcpConfiguration.NumberOfDiscrete,
                                                          modbusTcpConfiguration.NumberOfHoldingRegisters,
                                                          modbusTcpConfiguration.NumberOfInputRegisters,
                                                          modbusTcpConfiguration.MaxNumberOfReadAttempts);
        }

        public async Task StartAsync()
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

        public async Task StopAsync()
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
                var data = _modbusTcpManageData.GetData();
                var device = new ModbusTcpDevice(Id, Name, Type, Enable, StatusConnection);
                return new ModbusTcpDeviceStatus(device, data);
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_GET_DATA);
                _logger.LogError(exception.Message);
                return null;
            }
        }

        public ValidationResult ValidateWriteData(string data)
        {
            var modbusTcpWriteData = Utils.Deserializer<ModbusTcpWriteData>(data);
            return ValidateWriteData(modbusTcpWriteData);
        }

        public async Task SetDataAsync(string data)
        {
            var modbusTcpWriteData = Utils.Deserializer<ModbusTcpWriteData>(data);
            var validationResult = ValidateWriteData(modbusTcpWriteData);

            if (!validationResult.IsValid)
                throw new Exception(validationResult.Errors[0].ErrorMessage);

            _modbusWriteList.Enqueue(modbusTcpWriteData);
        }
    }
}
