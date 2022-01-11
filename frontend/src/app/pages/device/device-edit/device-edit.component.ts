import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DeviceService } from 'src/app/services/device.service';
import { DeviceViewModel } from 'src/app/services/view-models/device-view-model';

@Component({
  selector: 'app-device-edit',
  templateUrl: './device-edit.component.html',
  styleUrls: ['./device-edit.component.scss'],
})
export class DeviceEditComponent implements OnInit {
  enabled = false;
  configuration?: any
  device?: DeviceViewModel<any>;

  form: FormGroup = new FormGroup({
    id: new FormControl(),
    name: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(30),
    ]),
    typeId: new FormControl('', [Validators.required]),
  });

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private deviceService: DeviceService
  ) {
    this.device = this.route.snapshot.data['device'].data;
    this.enabled = this.device?.enabled ?? false;
    this.configuration = this.device?.configuration
  }

  ngOnInit(): void {
    this.form.patchValue({
      id: this.device?.id ?? '',
      name: this.device?.name ?? '',
      typeId: this.device?.typeId.toString() ?? '',
    });
  }

  save() {
    if (this.form.valid) {
      let newDevice = Object.assign({}, this.form.value);
      newDevice.enabled = this.enabled;
      newDevice.configuration = this.configuration;

      this.deviceService.put(newDevice).subscribe(
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
