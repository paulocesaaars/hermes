﻿using Deviot.Hermes.Infra.Modbus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviot.Hermes.Infra.Modbus.Services
{
    public class ModbusDeviceDataBase
    {
        private List<DigitalData> _coils;

        private List<DigitalData> _discrete;

        private List<AnalogicData> _holdingRegisters;

        private List<AnalogicData> _inputRegisters;

        private int _numberOfAttemptsToReadCoils;

        private int _numberOfAttemptsToReadDiscretes;

        private int _numberOfAttemptsToReadHoldingRegisters;

        private int _numberOfAttemptsToReadInputRegisters;

        private readonly int _maxNumberOfReadAttempts;

        

        public ModbusDeviceDataBase(int quantityCoilStatus, int quantityInputStatus, int quantityHoldingRegisters, int quantityInputRegisters, int maxNumberOfReadAttempts = 3)
        {
            InitializeCoils(quantityCoilStatus);
            InitializeDiscrete(quantityInputStatus);
            InitializeHoldingRegisters(quantityHoldingRegisters);
            InitializeInputRegisters(quantityInputRegisters);

            _maxNumberOfReadAttempts = maxNumberOfReadAttempts;
        }

        private void InitializeCoils(int quantityCoilStatus)
        {
            _coils = new List<DigitalData>(quantityCoilStatus);
            for (var x = 0; x < quantityCoilStatus; x++)
                _coils.Add(new DigitalData(x, null, false));
        }

        private void InitializeDiscrete(int quantityInputStatus)
        {
            _discrete = new List<DigitalData>(quantityInputStatus);
            for (var x = 0; x < quantityInputStatus; x++)
                _discrete.Add(new DigitalData(x, null, false));
        }

        private void InitializeHoldingRegisters(int quantityHoldingRegisters)
        {
            _holdingRegisters = new List<AnalogicData>(quantityHoldingRegisters);
            for (var x = 0; x < quantityHoldingRegisters; x++)
                _holdingRegisters.Add(new AnalogicData(x, null, false));
        }

        private void InitializeInputRegisters(int quantityInputRegisters)
        {
            _inputRegisters = new List<AnalogicData>(quantityInputRegisters);
            for (var x = 0; x < quantityInputRegisters; x++)
                _inputRegisters.Add(new AnalogicData(x, null, false));
        }

        private void SetCoilValue(int address, byte value)
        {
            try
            {
                var newValue = Boolean.Parse(value.ToString());
                var data = _coils.FirstOrDefault(x => x.Address == address);

                if (data is not null)
                    data.SetValue(newValue);
            }
            catch (Exception)
            {
                var data = _coils.FirstOrDefault(x => x.Address == address);
                if (data is not null)
                    data.SetValue(null, false);
            }
        }

        private void SetDiscreteValue(int address, byte value)
        {
            try
            {
                var newValue = Boolean.Parse(value.ToString());
                var data = _discrete.FirstOrDefault(x => x.Address == address);

                if (data is not null)
                    data.SetValue(newValue);
            }
            catch (Exception)
            {
                var data = _discrete.FirstOrDefault(x => x.Address == address);
                if (data is not null)
                    data.SetValue(null, false);
            }
        }

        private void SetHoldingRegisterValue(int address, ushort value)
        {
            var data = _holdingRegisters.FirstOrDefault(x => x.Address == address);

            if (data is not null)
                data.SetValue(value);
        }

        private void SetInputRegisterValue(int address, ushort value)
        {
            var data = _inputRegisters.FirstOrDefault(x => x.Address == address);

            if (data is not null)
                data.SetValue(value);
        }

        public void UpdateCoilsValues(byte[] values)
        {
            _numberOfAttemptsToReadCoils = 0;
            for (var x = 0; x < values.Length; x++)
                SetCoilValue(x, values[x]);
        }

        public void UpdateDiscreteValues(byte[] values)
        {
            _numberOfAttemptsToReadDiscretes = 0;
            for (var x = 0; x < values.Length; x++)
                SetDiscreteValue(x, values[x]);
        }

        public void UpdateHoldingRegisterValues(ushort[] values)
        {
            _numberOfAttemptsToReadHoldingRegisters = 0;
            for (var x = 0; x < values.Length; x++)
                SetHoldingRegisterValue(x, values[x]);
        }
        public void UpdateInputRegisterValues(ushort[] values)
        {
            _numberOfAttemptsToReadInputRegisters = 0;
            for (var x = 0; x < values.Length; x++)
                SetInputRegisterValue(x, values[x]);
        }

        public void UpdateCoilsToBadRequest(bool updateForNullValues = false)
        {
            _numberOfAttemptsToReadCoils = _numberOfAttemptsToReadCoils + 1;
            if (_numberOfAttemptsToReadCoils >= _maxNumberOfReadAttempts)
                foreach (var coil in _coils)
                    if (updateForNullValues)
                        coil.SetValue(null, false);
                    else
                        coil.SetBadRequest();
        }

        public void UpdateDiscreteToBadRequest(bool updateForNullValues = false)
        {
            _numberOfAttemptsToReadDiscretes = _numberOfAttemptsToReadDiscretes + 1;
            if (_numberOfAttemptsToReadDiscretes >= _maxNumberOfReadAttempts)
                foreach (var discrete in _discrete)
                    if (updateForNullValues)
                        discrete.SetValue(null, false);
                    else
                        discrete.SetBadRequest();
        }

        public void UpdateHoldingRegistersToBadRequest(bool updateForNullValues = false)
        {
            _numberOfAttemptsToReadHoldingRegisters = _numberOfAttemptsToReadHoldingRegisters + 1;
            if (_numberOfAttemptsToReadHoldingRegisters >= _maxNumberOfReadAttempts)
                foreach (var holdingRegister in _holdingRegisters)
                    if (updateForNullValues)
                        holdingRegister.SetValue(null, false);
                    else
                        holdingRegister.SetBadRequest();
        }

        public void UpdateInputRegistersToBadRequest(bool updateForNullValues = false)
        {
            _numberOfAttemptsToReadInputRegisters = _numberOfAttemptsToReadInputRegisters + 1;
            if (_numberOfAttemptsToReadInputRegisters >= _maxNumberOfReadAttempts)
                foreach (var inputRegister in _inputRegisters)
                    if (updateForNullValues)
                        inputRegister.SetValue(null, false);
                    else
                        inputRegister.SetBadRequest();
        }

        public void UpdateAllDataToBadRequest(bool updateForNullValues = false)
        {
            UpdateCoilsToBadRequest(updateForNullValues);
            UpdateDiscreteToBadRequest(updateForNullValues);
            UpdateHoldingRegistersToBadRequest(updateForNullValues);
            UpdateInputRegistersToBadRequest(updateForNullValues);
        }

        public ModbusDeviceData GetData()
        {
            return new ModbusDeviceData(_coils, 
                                        _discrete, 
                                        _holdingRegisters, 
                                        _inputRegisters);
        }
    }
}
