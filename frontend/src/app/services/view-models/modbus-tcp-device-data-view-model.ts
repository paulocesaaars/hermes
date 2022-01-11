import { ModbusTcpDataModelView } from './modbus-tcp-data-view-model';
import { ModbusTcpDeviceStatusModelView } from './modbus-tcp-device-status-view-model';

export interface ModbusTcpDeviceDataModelView {
  device: ModbusTcpDeviceStatusModelView;
  data: ModbusTcpDataModelView;
}
