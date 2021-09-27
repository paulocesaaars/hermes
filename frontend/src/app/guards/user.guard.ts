import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { LocalStorageUtil } from '../utils/local-storage-util';

@Injectable()
export class UserGuard implements CanActivate {
  localStorageUtils = new LocalStorageUtil();

  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot) {
    let userLogged = this.localStorageUtils.getUser();
    let id = route.params['id'];

    if (!userLogged?.administrator && userLogged?.id != id) {
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }
}
