import { AppGuard } from './guards/app.guard';
import { UserGuard } from './guards/user.guard';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './routers/app-routing.module';
import { getPtPaginatorIntl } from './utils/pt-paginator';

import { NgModule } from '@angular/core';
import { LOCALE_ID } from '@angular/core';
import localePt from '@angular/common/locales/pt';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule, registerLocaleData } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {MatCardModule} from '@angular/material/card';
import {MatIconModule} from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import {MatChipsModule} from '@angular/material/chips';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { MatPaginatorIntl, MatPaginatorModule } from '@angular/material/paginator';

import { TitleComponent } from './components/title/title.component';

import { LoginComponent } from './pages/login/login.component';
import { UserEditComponent } from './pages/user/user-edit/user-edit.component';
import { UserListComponent } from './pages/user/user-list/user-list.component';
import { UserNewComponent } from './pages/user/user-new/user-new.component';
import { DeviceListComponent } from './pages/device/device-list/device-list.component';
import { BrokerListComponent } from './pages/broker/broker-list/broker-list.component';

import { AuthService } from './services/auth.service';
import { UserService } from './services/user.service';
import { UserResolve } from './pages/user/user.resolve';
import { UserViewComponent } from './pages/user/user-view/user-view.component';


registerLocaleData(localePt);

@NgModule({
  declarations: [
    AppComponent,
    TitleComponent,
    LoginComponent,
    UserListComponent,
    UserEditComponent,
    UserNewComponent,
    DeviceListComponent,
    BrokerListComponent,
    UserViewComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatToolbarModule,
    MatButtonModule,
    MatPaginatorModule,
    MatCardModule,
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

    { provide: LOCALE_ID, useValue: "pt-BR" },
		{ provide: MatPaginatorIntl, useValue: getPtPaginatorIntl() }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
