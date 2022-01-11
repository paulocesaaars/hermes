import { EnumerationViewModel } from "./enumeration-view-model";

export interface DeviceInfoViewModel {
  id: string;
  name: string;
  typeId: number;
  type: EnumerationViewModel;
  enabled: boolean;
  statusConnection: boolean;
}
