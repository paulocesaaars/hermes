import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { DeviceService } from 'src/app/services/device.service';

import { ResponseViewModel } from '../../services/view-models/response-view-model';

@Injectable()
export class DeviceDataResolve
  implements Resolve<ResponseViewModel<any>>
{
  constructor(private deviceService: DeviceService) {}

  resolve(route: ActivatedRouteSnapshot) {
    return this.deviceService.getData(route.params['id']);
  }
}
