import { ModbusAnalogicDataModelView } from './modbus-analogic-data-view-model';
import { ModbusDigitalDataModelView } from './modbus-digital-data-view-model';

export interface ModbusTcpDataModelView {
  maxRegisters: number;
  coils: ModbusDigitalDataModelView[];
  discrete: ModbusDigitalDataModelView[];
  holdingRegisters: ModbusAnalogicDataModelView[];
  inputRegisters: ModbusAnalogicDataModelView[]
}
