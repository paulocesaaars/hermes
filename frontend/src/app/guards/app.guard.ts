import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { LocalStorageUtil } from '../utils/local-storage-util';

@Injectable()
export class AppGuard implements CanActivate {
  localStorageUtils = new LocalStorageUtil();

  constructor(private router: Router) {}

  canActivate() {
    if (!this.localStorageUtils.getUserLogged()) {
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }
}
