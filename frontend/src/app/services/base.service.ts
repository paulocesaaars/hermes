import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LocalStorageUtil } from '../utils/local-storage-util';

export abstract class BaseService {
  protected ApiUrl: string = environment.apiUrl;
  protected ApiUrlV1: string = environment.apiUrl + 'v1/';
  protected LocalStorage = new LocalStorageUtil();

  protected getHeaderJson() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
  }

  protected getAuthHeaderJson() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.LocalStorage.getToken()}`,
      }),
    };
  }

  protected getAuthHeaderBlob(): any {
    return {
      responseType: 'blob',
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.LocalStorage.getToken()}`,
      }),
    };
  }

  protected extractData(response: any) {
    return response || {};
  }

  protected serviceError(response: Response | any) {
    let customError: string[] = [];
    let customResponse = { error: { messages: [''] } };

    if (response instanceof HttpErrorResponse) {
      if (response.statusText === 'Unknown Error') {
        customError.push('Ocorreu um erro desconhecido');
        response.error.errors = customError;
      }
    }

    if (response.status === 401) {
      localStorage.clear();
      customError.push(
        'Ocorreu um erro de autorização, autentique novamente.'
      );

      customResponse.error.messages = customError;
      return throwError(customResponse);
    }

    if (response.status === 500) {
      customError.push(
        'Ocorreu um erro no processamento, tente novamente mais tarde ou contate o nosso suporte.'
      );

      // Erros do tipo 500 não possuem uma lista de erros
      // A lista de erros do HttpErrorResponse é readonly
      customResponse.error.messages = customError;
      return throwError(customResponse);
    }

    return throwError(response.error);
  }
}
