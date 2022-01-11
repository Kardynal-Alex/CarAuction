import { Injectable } from '@angular/core';
import { HttpClient , HttpParams } from '@angular/common/http';
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

@Injectable({providedIn: 'root'})
export class AuthService {
    apiUrl = 'https://localhost:44325/api/account/';
    constructor(private httpClient:HttpClient,
                private localStorage:LocalStorageService,
                private jwtHelper: JwtHelperService) { }

    login(login:Login){
        return this.httpClient.post(this.apiUrl+"login", login);
    }

    twoSteplogin(twoFactor:TwoFactor){
      return this.httpClient.post(this.apiUrl+"twostepverification",twoFactor);
    }

    facebookLogin(facebookLogin:Facebook){
      return this.httpClient.post(this.apiUrl+"facebook/",facebookLogin);
    }

    googleLogin(googleLogin:ExternalAuth){
      return this.httpClient.post(this.apiUrl+"google/",googleLogin);
    }

    logout() {
        this.localStorage.remove("token");
        return this.httpClient.post(this.apiUrl+"logout",null);
    }

    register(register:Register){
        return this.httpClient.post(this.apiUrl+"register", register);
    }
    
    getUserById(id:string){
        return this.httpClient.get<User>(this.apiUrl+"getuserbyid/?id="+id);
    }

    confirmEmail(route: string, token: string, email: string){
      let params = new HttpParams({ encoder: new CustomEncoder() })
      params = params.append('token', token);
      params = params.append('email', email);
  
      return this.httpClient.get(this.apiUrl+"emailconfirmation", { params: params });
    }
    
    forgotPassword(forgotPassword: ForgotPassword){
      return this.httpClient.post(this.apiUrl+"forgotpassword", forgotPassword);
    }

    resetPassword(resetPassword: ResetPassword){
      return this.httpClient.post(this.apiUrl+"resetpassword", resetPassword);
    }

    isAuthenticated() {
      const token=this.localStorage.get("token");
        if(token && !this.jwtHelper.isTokenExpired(token)) {
          return true;
        }
        else {
          return false;
        }
    }
    
    CheckIfIsAdmin(){
      var payload=JSON.parse(window.atob(this.localStorage.get('token').split('.')[1]));
      if(payload.role.toLowerCase()==="admin"){
        return true;
      }
      return false;
    }

    getUserId():string{
      var payload=JSON.parse(window.atob(this.localStorage.get('token').split('.')[1]));
      return payload.id;
    }

    getUserEmail():string{
      var payload=JSON.parse(window.atob(this.localStorage.get('token').split('.')[1]));
      return payload.email;
    }
}