import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommonConstants } from 'src/app/common/constants/common-constants';
import { ExternalAuth } from 'src/app/models/external-auth';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-google-login',
  templateUrl: './google-login.component.html',
  styleUrls: ['./google-login.component.less']
})
export class GoogleLoginComponent implements OnInit {
  @ViewChild('loginRef', { static: true }) loginElement: ElementRef;
  auth2: any;
  constructor(
    private toastrService: ToastrService,
    private authService: AuthService,
    private localStorage: LocalStorageService) { }

  public ngOnInit() {
    this.googleInitialize();
  }

  public googleInitialize() {
    window['googleSDKLoaded'] = () => {
      window['gapi'].load('auth2', () => {
        this.auth2 = window['gapi'].auth2.init({
          client_id: '531159136829-49p64ohcrno7uq5qn3ardv3nn13804uu.apps.googleusercontent.com',
          cookie_policy: 'single_host_origin',
          scope: 'profile email'
        });
        this.prepareLogin();
      });
    }
    (function (d, s, id) {
      var js, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) { return; }
      js = d.createElement(s); js.id = id;
      js.src = "https://apis.google.com/js/platform.js?onload=googleSDKLoaded";
      fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'google-jssdk'));
  }

  public prepareLogin() {
    this.auth2.attachClickHandler(this.loginElement.nativeElement, {},
      (googleUser) => {
        let profile = googleUser.getBasicProfile();
        const externalAuth: ExternalAuth = {
          provider: "GOOGLE",
          idToken: googleUser.getAuthResponse().id_token
        }
        this.authService.googleLogin(externalAuth)
          .subscribe(response => {
            this.toastrService.success("Login successfully.");
            const token = (<any>response).token;
            this.localStorage.set(CommonConstants.JWTToken, JSON.stringify(token));
            window.location.reload();
          }, _ => {
            this.toastrService.error("error");
          });
        //console.log('Token || ' + googleUser.getAuthResponse().id_token);
        //console.log('Email: ' , profile.getEmail());
        //console.log("profile", googleUser.getAuthResponse());
      }, (error) => {
        alert(JSON.stringify(error, undefined, 2));
      });
  }
}
