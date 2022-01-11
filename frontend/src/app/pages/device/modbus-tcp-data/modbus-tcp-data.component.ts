import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { interval, Subscription } from 'rxjs';
import { DeviceService } from 'src/app/services/device.service';
import { ModbusTcpDataModelView } from 'src/app/services/view-models/modbus-tcp-data-view-model';
import { ModbusTcpDeviceDataModelView } from 'src/app/services/view-models/modbus-tcp-device-data-view-model';

export interface ModbusData {
  address: number;
  coil: boolean | null;
  coilQuality: boolean;
  discrete: boolean | null;
  discreteQuality: boolean;
  holdingRegister: number | null;
  holdingRegisterQuality: boolean;
  inputRegister: number | null;
  inputRegisterQuality: boolean;
}

@Component({
  selector: 'app-modbus-tcp-data',
  templateUrl: './modbus-tcp-data.component.html',
  styleUrls: ['./modbus-tcp-data.component.scss'],
})
export class ModbusTcpDataComponent implements OnInit {
  id: string = '';
  name: string = '';
  data_source: ModbusData[] = [];
  updateSubscription?: Subscription;
  displayedColumns: string[] = [
    'address',
    'coil',
    'discrete',
    'holdingRegister',
    'inputRegister',
  ];

  constructor(
    private route: ActivatedRoute,
    private deviceService: DeviceService
  ) {
    let deviceStatus: ModbusTcpDeviceDataModelView =
      this.route.snapshot.data['device_data']?.data;
    if (deviceStatus != null) {
      this.name = deviceStatus.device.name;
      this.convertData(deviceStatus.data);
    }
  }

  ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.updateSubscription = interval(5000).subscribe(() => {
      this.updateData();
    });
  }

  ngOnDestroy() {
    this.updateSubscription?.unsubscribe();
  }

  updateData() {
    let response = this.deviceService.getData(this.id).subscribe(
      (response) => {
        if (response?.data != null) {
          var deviceStatus = response.data as ModbusTcpDeviceDataModelView;

          this.name = deviceStatus.device.name;
          this.convertData(deviceStatus.data);
        }
      },
      (error) => {
        console.error(error);
      }
    );
  }

  convertData(data: ModbusTcpDataModelView) {
    var modbusDataArray: ModbusData[] = [];
    for (let x = 0; x < data.maxRegisters; x++) {
      let modbusData: ModbusData = {
        address: x + 1,
        coil: data.coils[x]?.value ?? null,
        coilQuality: data.coils[x]?.quality ?? false,
        discrete: data.discrete[x]?.value ?? null,
        discreteQuality: data.discrete[x]?.quality ?? false,
        holdingRegister: data.holdingRegisters[x]?.value ?? null,
        holdingRegisterQuality: data.holdingRegisters[x]?.quality ?? false,
        inputRegister: data.inputRegisters[x]?.value ?? null,
        inputRegisterQuality: data.inputRegisters[x]?.quality ?? false,
      };

      modbusDataArray.push(modbusData);
    }

    this.data_source = modbusDataArray;
  }
}
