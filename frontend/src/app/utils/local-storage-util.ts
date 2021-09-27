import { TokenViewModel } from '../services/view-models/token-view-model';
import { UserInfoViewModel } from '../services/view-models/user-info-view-model';

export class LocalStorageUtil {

  public getUser(): UserInfoViewModel | null {
    try {
      return JSON.parse(localStorage.getItem('deviot.user') ?? '');
    } catch {
      return null;
    }
  }

  public getToken(): string | null {
    return localStorage.getItem('deviot.token');
  }

  public getUserLogged(): boolean {
    let token = this.getToken();
    let user = this.getUser();

    if (user == null || token == null) {
      return false;
    }

    return true;
  }

  public saveToken(token: string) {
    localStorage.setItem('deviot.token', token);
  }

  public saveUser(user: UserInfoViewModel) {
    localStorage.setItem('deviot.user', JSON.stringify(user));
  }

  public saveAll(response: TokenViewModel) {
    this.saveToken(response.accessToken);
    this.saveUser(response.user);
  }

  public clear() {
    localStorage.removeItem('deviot.token');
    localStorage.removeItem('deviot.user');
  }
}
