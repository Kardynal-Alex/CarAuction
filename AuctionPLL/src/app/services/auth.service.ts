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
import { BaseUrl } from '../common/constants/urls';
import { CommonConstants } from '../common/constants/common-constants';

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
    this.localStorage.remove(CommonConstants.JWTToken);
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
    const token = this.localStorage.get(CommonConstants.JWTToken);
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    } else {
      return false;
    }
  }

  public getToken(): string {
    var token = this.localStorage.get(CommonConstants.JWTToken)?.split('.')[1];
    return !!token ? token : null;
  }

  public getPayload(): any {
    var token = this.getToken();
    return !!token ? JSON.parse(window.atob(token)) : null;
  }

  public CheckIfIsAdmin(): boolean {
    var payload = this.getPayload();
    if (payload?.role.toLowerCase() === CommonConstants.Admin) {
      return true;
    }
    return false;
  }

  public getUserId(): string {
    var payload = this.getPayload();
    return payload?.id ?? null;
  }

  public getUserEmail(): string {
    var payload = this.getPayload();
    return payload?.email ?? null;
  }
}