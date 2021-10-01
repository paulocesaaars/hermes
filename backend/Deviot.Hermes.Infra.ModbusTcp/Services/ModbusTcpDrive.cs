using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Enumerators;
using Deviot.Hermes.Domain.Interfaces;
using Deviot.Hermes.Infra.ModbusTcp.Configurations;
using Deviot.Hermes.Infra.ModbusTcp.Models;
using FluentModbus;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Deviot.Hermes.Infra.ModbusTcp.Services
{
    public class ModbusTcpDrive : IModbusTcpDrive
    {
        private bool _block;
        private Thread _manageConnection;
        private ModbusTcpClient _modbusClient;
        private CancellationTokenSource _cancellationTokenSource;
        private ModbusTcpConfiguration _modbusTcpConfiguration;
        private ModbusData _modbusData;

        private readonly ILogger<ModbusTcpDrive> _logger;

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public DeviceTypeEnumeration Type { get; private set; }

        public bool Status { get; private set; }

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
            _modbusData = new ModbusData();
            _modbusClient = new ModbusTcpClient();
            _manageConnection = new Thread(ManageComunication);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void ReadCoils()
        {
            var limit = _modbusTcpConfiguration.CoilStatus;
            if (_modbusClient.IsConnected && limit > 0)
            {
                var result = _modbusClient.ReadCoils(0xFF, 0, limit);
                for (var x = 0; x < limit; x++)
                {
                    var data = new DigitalData { Address = x, Value = result[x] };
                    if (_modbusData.CoilStatus.Any(m => m.Address == x))
                        _modbusData.CoilStatus[x] = data;
                    else
                        _modbusData.CoilStatus.Add(data);
                }
            }
        }

        private void ReadDiscreteInputs()
        {
            var limit = _modbusTcpConfiguration.InputStatus;
            if (_modbusClient.IsConnected && limit > 0)
            {
                var result = _modbusClient.ReadDiscreteInputs(0xFF, 0, limit);
                for (var x = 0; x < limit; x++)
                {
                    var data = new DigitalData { Address = x, Value = result[x] };
                    if (_modbusData.InputStatus.Any(m => m.Address == x))
                        _modbusData.InputStatus[x] = data;
                    else
                        _modbusData.InputStatus.Add(data);
                }
            }
        }

        private void ReadInputRegisters()
        {
            var limit = _modbusTcpConfiguration.InputRegister;
            if (_modbusClient.IsConnected && limit > 0)
            {
                var result = _modbusClient.ReadInputRegisters<ushort>(0xFF, 0, limit);
                for (var x = 0; x < limit; x++)
                {
                    var data = new AnalogicData { Address = x, Value = result[x] };
                    if (_modbusData.InputRegisters.Any(m => m.Address == x))
                        _modbusData.InputRegisters[x] = data;
                    else
                        _modbusData.InputRegisters.Add(data);
                }
            }
        }

        private void ReadHoldingRegisters()
        {
            var limit = _modbusTcpConfiguration.HoldingRegister;
            if (_modbusClient.IsConnected && limit > 0)
            {
                var result = _modbusClient.ReadHoldingRegisters<ushort>(0xFF, 0, limit);
                for (var x = 0; x < limit; x++)
                {
                    var data = new AnalogicData { Address = x, Value = result[x] };
                    if (_modbusData.HoldingRegisters.Any(m => m.Address == x))
                        _modbusData.HoldingRegisters[x] = data;
                    else
                        _modbusData.HoldingRegisters.Add(data);
                }
            }
        }

        private void ManageComunication()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (_block)
                {
                    Task.Delay(500, _cancellationTokenSource.Token);
                }
                else
                {
                    Connect();

                    ReadCoils();
                    ReadDiscreteInputs();
                    ReadHoldingRegisters();
                    ReadInputRegisters();

                    Disconnect();

                    Task.Delay(1000, _cancellationTokenSource.Token);
                }
            }

            Disconnect();
        }

        private void Connect()
        {
            try
            {
                if (!_modbusClient.IsConnected)
                    _modbusClient.Connect(new IPEndPoint(IPAddress.Parse(_modbusTcpConfiguration.Ip), _modbusTcpConfiguration.Port), ModbusEndianness.BigEndian);
            }
            catch (Exception exception)
            {
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
                _logger.LogError(exception.Message);
            }
        }

        private void SetConfiguration(string configuration)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _modbusTcpConfiguration = JsonSerializer.Deserialize<ModbusTcpConfiguration>(configuration, options);
        }

        public void SetDevice(Device device)
        {
            Id = device.Id;
            Name = device.Name;
            Type = device.Type;
            SetConfiguration(device.Configuration);
        }

        public void UpdateDrive(Device device)
        {
            _block = true;

            Disconnect();
            SetDevice(device);
            Connect();

            _block = false;
        }

        public void Start()
        {
            _block = false;
            _manageConnection.Start();
        }

        public void Stop() => _cancellationTokenSource.Cancel();

        public Task<object> GetDataAsync(string json)
        {
            throw new NotImplementedException();
        }

        public Task SetDataAsync(string data)
        {
            throw new NotImplementedException();
        }
    }
}
