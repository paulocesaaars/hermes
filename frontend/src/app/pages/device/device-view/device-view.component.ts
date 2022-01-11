import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DeviceService } from 'src/app/services/device.service';
import { DeviceInfoViewModel } from 'src/app/services/view-models/device-info-view-model';

@Component({
  selector: 'app-device-view',
  templateUrl: './device-view.component.html',
  styleUrls: ['./device-view.component.scss'],
})
export class DeviceViewComponent implements OnInit {

   device?: DeviceInfoViewModel;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private deviceService: DeviceService
  ) {
    this.device = this.route.snapshot.data['device'].data;
  }

  ngOnInit(): void {}
}
