import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Login } from '../models/auth-models/login';
import { Register } from '../models/auth-models/register';
import { LocalStorageService } from '../services/local-storage.service';
import { CustomEncoder } from '../services/customer-encoder';
import { ExternalAuth } from '../models/auth-models/external-auth';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';
import { CommonConstants } from '../common/constants/common-constants';
import { UserViewModel } from '../generated-models/auth-models/user-view-model';
import { ForgotPasswordViewModel } from '../generated-models/auth-models/forgot-password-view-model';
import { FacebookAuthViewModel } from '../generated-models/auth-models/facebook-auth-view-model';
import { ResetPasswordViewModel } from '../generated-models/auth-models/reset-password-view-model';
import { TwoFactorViewModel } from '../generated-models/auth-models/two-factor-view-model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = `${BaseUrl.ApiURL}/account`;

  constructor(
    private httpClient: HttpClient,
    private localStorage: LocalStorageService,
    private jwtHelper: JwtHelperService
  ) { }

  public login(login: Login): Observable<Object> {
    return this.httpClient.post(`${this.apiUrl}/Login`, login);
  }

  public twoSteplogin(twoFactor: TwoFactorViewModel): Observable<Object> {
    return this.httpClient.post(`${this.apiUrl}/TwoStepVerification`, twoFactor);

  }

  public facebookLogin(facebookLogin: FacebookAuthViewModel): Observable<Object> {
    return this.httpClient.post(`${this.apiUrl}/Facebook/`, facebookLogin);
  }

  public googleLogin(googleLogin: ExternalAuth): Observable<Object> {
    return this.httpClient.post(`${this.apiUrl}/Google/`, googleLogin);
  }

  public logout(): Observable<Object> {
    this.localStorage.remove(CommonConstants.JWTToken);
    return this.httpClient.post(`${this.apiUrl}/Logout`, null);
  }

  public register(register: Register): Observable<Object> {
    return this.httpClient.post(`${this.apiUrl}/Register`, register);
  }

  public getUserById(id: string): Observable<UserViewModel> {
    return this.httpClient.get<UserViewModel>(`${this.apiUrl}/GetUserById/?id=${id}`);
  }

  public confirmEmail(route: string, token: string, email: string) {
    let params = new HttpParams({ encoder: new CustomEncoder() });
    params = params.append('token', token);
    params = params.append('email', email);

    return this.httpClient.get(`${this.apiUrl}/EmailConfirmation`, { params: params });
  }

  public forgotPassword(forgotPassword: ForgotPasswordViewModel): Observable<Object> {
    return this.httpClient.post(`${this.apiUrl}/ForgotPassword`, forgotPassword);
  }

  public resetPassword(resetPassword: ResetPasswordViewModel): Observable<Object> {
    return this.httpClient.post(`${this.apiUrl}/ResetPassword`, resetPassword);
  }

  public isAuthenticated(): boolean {
    const token = this.localStorage.get(CommonConstants.JWTToken);
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    return false;
  }

  public getToken(): string {
    var token = this.localStorage.get(CommonConstants.JWTToken)?.split('.')[1];
    return !!token ? token : null;
  }

  public getPayload(): any {
    var token = this.getToken();
    return !!token ? JSON.parse(window.atob(token)) : null;
  }

  public checkIfUserIsAdmin(): boolean {
    var payload = this.getPayload();
    if (payload?.role.toLowerCase() === CommonConstants.Admin) {
      return true;
    }
    return false;
  }

  public getUserIdFromToken(): string {
    var payload = this.getPayload();
    return payload?.id ?? null;
  }

  public getUserEmailFromToken(): string {
    var payload = this.getPayload();
    return payload?.email ?? null;
  }
}