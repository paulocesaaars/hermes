import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DeviceService } from 'src/app/services/device.service';
import { DeviceViewModel } from 'src/app/services/view-models/device-view-model';

@Component({
  selector: 'app-device-new',
  templateUrl: './device-new.component.html',
  styleUrls: ['./device-new.component.scss'],
})
export class DeviceNewComponent implements OnInit {
  enabled = false;
  configuration?: any;
  device?: DeviceViewModel<any>;

  form: FormGroup = new FormGroup({
    name: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(30),
    ]),
    typeId: new FormControl('', [Validators.required]),
  });

  constructor(private router: Router, private deviceService: DeviceService) {}

  ngOnInit(): void {
    this.form.controls['typeId'].setValue('2')
  }

  save() {
    if (this.form.valid) {
      let newDevice = Object.assign({}, this.form.value);
      newDevice.enabled = this.enabled;
      newDevice.configuration = this.configuration;

      this.deviceService.post(newDevice).subscribe(
        () => {
          this.router.navigate(['/device']);
        },
        (error) => {
          alert(error.messages[0]);
          console.error(error);
        }
      );
    }
  }
}
