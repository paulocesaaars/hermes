import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { DeviceService } from 'src/app/services/device.service';

import { ResponseViewModel } from '../../services/view-models/response-view-model';
import { DeviceViewModel } from './../../services/view-models/device-view-model';

@Injectable()
export class DeviceResolve
  implements Resolve<ResponseViewModel<DeviceViewModel<any>>>
{
  constructor(private deviceService: DeviceService) {}

  resolve(route: ActivatedRouteSnapshot) {
    return this.deviceService.get(route.params['id']);
  }
}
