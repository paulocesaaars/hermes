import { DeviceInfoViewModel } from './../../../services/view-models/device-info-view-model';
import { Component, OnInit } from '@angular/core';
import { LocalStorageUtil } from 'src/app/utils/local-storage-util';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { DeviceService } from 'src/app/services/device.service';

@Component({
  selector: 'app-device',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit {

  localStorageUtil: LocalStorageUtil = new LocalStorageUtil();
  userLogin = this.localStorageUtil.getUser();
  devices: DeviceInfoViewModel[] = [];

  form: FormGroup = new FormGroup({
    name: new FormControl(''),
  });

  constructor(private router: Router, private deviceService: DeviceService) { }

  ngOnInit(): void {
    this.getDevices();
  }

  getDevices() {
    this.deviceService.getAll(this.form.controls['name'].value).subscribe(
      (response) => {
        this.devices = response == null ? [] : response.data
      },
      (error) => {
        console.error(error);
      }
    );
  }

  remove(id: string) {
    this.deviceService.delete(id).subscribe(
      (response) => {
        alert(response.messages[0]);
        this.getDevices();
      },
      (error) => {
        alert(error.messages[0]);
        console.error(error);
      }
    );
  }

}
