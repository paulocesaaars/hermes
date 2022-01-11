import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from 'src/app/services/base.service';

import { DeviceInfoViewModel } from './view-models/device-info-view-model';
import { DeviceViewModel } from './view-models/device-view-model';
import { ResponseViewModel } from './view-models/response-view-model';

@Injectable()
export class DeviceService extends BaseService {
  constructor(private http: HttpClient) {
    super();
  }

  getAll(
    name: string = ''
  ): Observable<ResponseViewModel<DeviceInfoViewModel[]>> {
    return this.http.get<ResponseViewModel<DeviceInfoViewModel[]>>(
      //this.ApiUrlV1 + 'device',
      this.ApiUrlV1 + 'device?name=' + name,
      this.getAuthHeaderJson()
    );
  }

  get(deviceId: string): Observable<ResponseViewModel<DeviceViewModel<any>>> {
    return this.http.get<ResponseViewModel<DeviceViewModel<any>>>(
      this.ApiUrlV1 + 'device/' + deviceId,
      this.getAuthHeaderJson()
    );
  }

  getData(deviceId: string): Observable<ResponseViewModel<any>> {
    return this.http.get<ResponseViewModel<any>>(
      this.ApiUrlV1 + 'device/data/' + deviceId,
      this.getAuthHeaderJson()
    );
  }

  post(
    device: DeviceViewModel<any>
  ): Observable<ResponseViewModel<DeviceViewModel<any>>> {
    return this.http
      .post(this.ApiUrlV1 + 'device', device, super.getAuthHeaderJson())
      .pipe(map(super.extractData), catchError(super.serviceError));
  }

  put(
    device: DeviceViewModel<any>
  ): Observable<ResponseViewModel<DeviceViewModel<any>>> {
    return this.http
      .put(
        this.ApiUrlV1 + 'device/' + device.id,
        device,
        super.getAuthHeaderJson()
      )
      .pipe(map(super.extractData), catchError(super.serviceError));
  }

  delete(deviceId: string): Observable<ResponseViewModel<any>> {
    return this.http
      .delete(this.ApiUrlV1 + 'device/' + deviceId, super.getAuthHeaderJson())
      .pipe(map(super.extractData), catchError(super.serviceError));
  }
}
