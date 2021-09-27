import { BrokerListComponent } from '../pages/broker/broker-list/broker-list.component';
import { DeviceListComponent } from '../pages/device/device-list/device-list.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../pages/login/login.component';
import { AppGuard } from '../guards/app.guard';
import { UserEditComponent } from '../pages/user/user-edit/user-edit.component';
import { UserResolve } from '../pages/user/user.resolve';
import { UserGuard } from '../guards/user.guard';
import { UserListComponent } from '../pages/user/user-list/user-list.component';
import { UserNewComponent } from '../pages/user/user-new/user-new.component';
import { UserViewComponent } from '../pages/user/user-view/user-view.component';

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
  { path: 'device', component: DeviceListComponent, canActivate: [AppGuard] },
  { path: 'broker', component: BrokerListComponent, canActivate: [AppGuard] },
  { path: 'login', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
