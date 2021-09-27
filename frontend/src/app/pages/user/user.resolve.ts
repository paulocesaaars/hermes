import { ResponseViewModel } from '../../services/view-models/response-view-model';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { UserInfoViewModel } from 'src/app/services/view-models/user-info-view-model';

@Injectable()
export class UserResolve implements Resolve<ResponseViewModel<UserInfoViewModel>> {

    constructor(private userService: UserService) { }

    resolve(route: ActivatedRouteSnapshot) {
        return this.userService.get(route.params['id']);
    }
}
