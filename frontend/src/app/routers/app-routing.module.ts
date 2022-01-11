import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppGuard } from '../guards/app.guard';
import { UserGuard } from '../guards/user.guard';
import { BrokerListComponent } from '../pages/broker/broker-list/broker-list.component';
import { DeviceEditComponent } from '../pages/device/device-edit/device-edit.component';
import { DeviceListComponent } from '../pages/device/device-list/device-list.component';
import { DeviceNewComponent } from '../pages/device/device-new/device-new.component';
import { DeviceViewComponent } from '../pages/device/device-view/device-view.component';
import { DeviceDataResolve } from '../pages/device/device.data.resolve';
import { DeviceResolve } from '../pages/device/device.resolve';
import { ModbusTcpDataComponent } from '../pages/device/modbus-tcp-data/modbus-tcp-data.component';
import { LoginComponent } from '../pages/login/login.component';
import { UserEditComponent } from '../pages/user/user-edit/user-edit.component';
import { UserListComponent } from '../pages/user/user-list/user-list.component';
import { UserNewComponent } from '../pages/user/user-new/user-new.component';
import { UserViewComponent } from '../pages/user/user-view/user-view.component';
import { UserResolve } from '../pages/user/user.resolve';

const routes: Routes = [
  { path: '', redirectTo: 'user', pathMatch: 'full' },
  {
    path: 'user',
    children: [
      { path: '', component: UserListComponent, canActivate: [AppGuard] },
      { path: 'new', component: UserNewComponent, canActivate: [AppGuard] },
      {
        path: 'view/:id',
        component: UserViewComponent,
        canActivate: [AppGuard],
        resolve: {
          user: UserResolve,
        },
      },
      {
        path: 'edit/:id',
        component: UserEditComponent,
        canActivate: [AppGuard, UserGuard],
        resolve: {
          user: UserResolve,
        },
      },
    ],
  },
  {
    path: 'device',
    children: [
      { path: '', component: DeviceListComponent, canActivate: [AppGuard] },
      { path: 'new', component: DeviceNewComponent, canActivate: [AppGuard] },
      {
        path: 'view/:id',
        component: DeviceViewComponent,
        canActivate: [AppGuard],
        resolve: {
          device: DeviceResolve,
        },
      },
      {
        path: 'modbus-tcp/:id',
        component: ModbusTcpDataComponent,
        canActivate: [AppGuard],
        resolve: {
          device_data: DeviceDataResolve,
        },
      },
      {
        path: 'edit/:id',
        component: DeviceEditComponent,
        canActivate: [AppGuard, UserGuard],
        resolve: {
          device: DeviceResolve,
        },
      },
    ],
  },
  { path: 'broker', component: BrokerListComponent, canActivate: [AppGuard] },
  { path: 'login', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
