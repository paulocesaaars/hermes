import { UserViewModel } from './view-models/user-view-model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from 'src/app/services/base.service';
import { ResponseViewModel } from './view-models/response-view-model';
import { UserInfoViewModel } from './view-models/user-info-view-model';

@Injectable()
export class UserService extends BaseService {
  constructor(private http: HttpClient) {
    super();
  }

  getAll(
    name: string = ''
  ): Observable<ResponseViewModel<UserInfoViewModel[]>> {
    return this.http.get<ResponseViewModel<UserInfoViewModel[]>>(
      this.ApiUrlV1 + 'user?name=' + name,
      this.getAuthHeaderJson()
    );
  }

  get(userId: string): Observable<ResponseViewModel<UserInfoViewModel>> {
    return this.http.get<ResponseViewModel<UserInfoViewModel>>(
      this.ApiUrlV1 + 'user/' + userId,
      this.getAuthHeaderJson()
    );
  }

  post(user: UserViewModel): Observable<ResponseViewModel<UserInfoViewModel>> {
    return this.http
      .post(this.ApiUrlV1 + 'user', user, super.getAuthHeaderJson())
      .pipe(map(super.extractData), catchError(super.serviceError));
  }

  put(
    user: UserInfoViewModel
  ): Observable<ResponseViewModel<UserInfoViewModel>> {
    return this.http
      .put(this.ApiUrlV1 + 'user/' + user.id, user, super.getAuthHeaderJson())
      .pipe(map(super.extractData), catchError(super.serviceError));
  }

  delete(userId: string): Observable<ResponseViewModel<any>> {
    return this.http
      .delete(this.ApiUrlV1 + 'user/' + userId, super.getAuthHeaderJson())
      .pipe(map(super.extractData), catchError(super.serviceError));
  }
}
