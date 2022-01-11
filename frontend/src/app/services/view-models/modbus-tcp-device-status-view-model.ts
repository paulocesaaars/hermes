import { EnumerationViewModel } from "./enumeration-view-model";

export interface ModbusTcpDeviceStatusModelView {
  id: string;
  name: string;
  type: EnumerationViewModel;
  enable: boolean;
  statusConnection: boolean;
}
