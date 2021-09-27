import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from "rxjs/operators";
import { BaseService } from 'src/app/services/base.service';
import { LoginViewModel } from './view-models/login-view-model';
import { ResponseViewModel } from './view-models/response-view-model';
import { TokenViewModel } from './view-models/token-view-model';

@Injectable()
export class AuthService extends BaseService {

    constructor(private http: HttpClient) { super();}

    login(userLogin:LoginViewModel): Observable<ResponseViewModel<TokenViewModel>> {
        return this.http.post(this.ApiUrlV1 + 'auth/login', userLogin, super.getHeaderJson())
            .pipe(
                map(super.extractData),
                catchError(super.serviceError)
            );
    }

}
