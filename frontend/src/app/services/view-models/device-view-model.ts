import { EnumerationViewModel } from "./enumeration-view-model";

export interface DeviceViewModel<T> {
  id: string;
  name: string;
  typeId: number;
  type: EnumerationViewModel;
  enabled: boolean;
  configuration: T
}
