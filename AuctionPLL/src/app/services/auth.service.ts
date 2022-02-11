import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Login } from '../models/login';
import { Register } from '../models/register';
import { LocalStorageService } from '../services/local-storage.service';
import { User } from '../models/user';
import { CustomEncoder } from '../services/customer-encoder';
import { ForgotPassword } from '../models/forgot-password';
import { ResetPassword } from '../models/reset-password';
import { Facebook } from '../models/facebook';
import { ExternalAuth } from '../models/external-auth';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TwoFactor } from '../models/two-factor';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/urls';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = BaseUrl.ApiURL + 'account/';
  constructor(
    private httpClient: HttpClient,
    private localStorage: LocalStorageService,
    private jwtHelper: JwtHelperService
  ) { }

  public login(login: Login): Observable<Object> {
    return this.httpClient.post(this.apiUrl + "login", login);
  }

  public twoSteplogin(twoFactor: TwoFactor): Observable<Object> {
    return this.httpClient.post(this.apiUrl + "twostepverification", twoFactor);
  }

  public facebookLogin(facebookLogin: Facebook): Observable<Object> {
    return this.httpClient.post(this.apiUrl + "facebook/", facebookLogin);
  }

  public googleLogin(googleLogin: ExternalAuth): Observable<Object> {
    return this.httpClient.post(this.apiUrl + "google/", googleLogin);
  }

  public logout(): Observable<Object> {
    this.localStorage.remove("token");
    return this.httpClient.post(this.apiUrl + "logout", null);
  }

  public register(register: Register): Observable<Object> {
    return this.httpClient.post(this.apiUrl + "register", register);
  }

  public getUserById(id: string): Observable<User> {
    return this.httpClient.get<User>(this.apiUrl + "getuserbyid/?id=" + id);
  }

  public confirmEmail(route: string, token: string, email: string) {
    let params = new HttpParams({ encoder: new CustomEncoder() });
    params = params.append('token', token);
    params = params.append('email', email);

    return this.httpClient.get(this.apiUrl + "emailconfirmation", { params: params });
  }

  public forgotPassword(forgotPassword: ForgotPassword): Observable<Object> {
    return this.httpClient.post(this.apiUrl + "forgotpassword", forgotPassword);
  }

  public resetPassword(resetPassword: ResetPassword): Observable<Object> {
    return this.httpClient.post(this.apiUrl + "resetpassword", resetPassword);
  }

  public isAuthenticated(): boolean {
    const token = this.localStorage.get("token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    } else {
      return false;
    }
  }

  public CheckIfIsAdmin(): boolean {
    var payload = JSON.parse(window.atob(this.localStorage.get('token')?.split('.')[1]));
    if (payload.role.toLowerCase() === "admin") {
      return true;
    }
    return false;
  }

  public getUserId(): string {
    var payload = JSON.parse(window.atob(this.localStorage.get('token')?.split('.')[1]));
    return payload?.id ?? null;
  }

  public getUserEmail(): string {
    var payload = JSON.parse(window.atob(this.localStorage.get('token')?.split('.')[1]));
    return payload?.email ?? null;
  }
}