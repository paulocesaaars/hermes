import { CommonModule, registerLocaleData } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import localePt from '@angular/common/locales/pt';
import { LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorIntl, MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { ModbusRtuFormComponent } from './components/modbus-rtu-form/modbus-rtu-form.component';
import { ModbusRtuViewComponent } from './components/modbus-rtu-view/modbus-rtu-view.component';
import { ModbusTcpFormComponent } from './components/modbus-tcp-form/modbus-tcp-form.component';
import { ModbusTcpViewComponent } from './components/modbus-tcp-view/modbus-tcp-view.component';
import { TitleComponent } from './components/title/title.component';
import { AppGuard } from './guards/app.guard';
import { UserGuard } from './guards/user.guard';
import { BrokerListComponent } from './pages/broker/broker-list/broker-list.component';
import { DeviceEditComponent } from './pages/device/device-edit/device-edit.component';
import { DeviceListComponent } from './pages/device/device-list/device-list.component';
import { DeviceNewComponent } from './pages/device/device-new/device-new.component';
import { DeviceViewComponent } from './pages/device/device-view/device-view.component';
import { DeviceDataResolve } from './pages/device/device.data.resolve';
import { DeviceResolve } from './pages/device/device.resolve';
import { ModbusTcpDataComponent } from './pages/device/modbus-tcp-data/modbus-tcp-data.component';
import { LoginComponent } from './pages/login/login.component';
import { UserEditComponent } from './pages/user/user-edit/user-edit.component';
import { UserListComponent } from './pages/user/user-list/user-list.component';
import { UserNewComponent } from './pages/user/user-new/user-new.component';
import { UserViewComponent } from './pages/user/user-view/user-view.component';
import { UserResolve } from './pages/user/user.resolve';
import { AppRoutingModule } from './routers/app-routing.module';
import { AuthService } from './services/auth.service';
import { DeviceService } from './services/device.service';
import { UserService } from './services/user.service';
import { getPtPaginatorIntl } from './utils/pt-paginator';


registerLocaleData(localePt);

@NgModule({
  declarations: [
    AppComponent,
    TitleComponent,
    LoginComponent,
    UserListComponent,
    UserEditComponent,
    UserNewComponent,
    UserViewComponent,
    DeviceListComponent,
    DeviceEditComponent,
    DeviceNewComponent,
    DeviceViewComponent,
    BrokerListComponent,
    ModbusTcpFormComponent,
    ModbusRtuFormComponent,
    ModbusRtuViewComponent,
    ModbusTcpViewComponent,
    ModbusTcpDataComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatGridListModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatToolbarModule,
    MatTableModule,
    MatButtonModule,
    MatPaginatorModule,
    MatCardModule,
    MatTabsModule,
    MatIconModule,
    MatChipsModule,
    MatSlideToggleModule
  ],
  providers: [
    AppGuard,
    UserGuard,
    AuthService,
    UserService,
    UserResolve,
    DeviceService,
    DeviceResolve,
    DeviceDataResolve,

    { provide: LOCALE_ID, useValue: "pt-BR" },
		{ provide: MatPaginatorIntl, useValue: getPtPaginatorIntl() }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
