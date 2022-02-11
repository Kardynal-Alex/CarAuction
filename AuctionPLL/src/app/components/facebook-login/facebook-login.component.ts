import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Facebook } from 'src/app/models/facebook';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

declare var FB: any;

@Component({
  selector: 'app-facebook-login',
  templateUrl: './facebook-login.component.html',
  styleUrls: ['./facebook-login.component.css']
})
export class FacebookLoginComponent implements OnInit {

  constructor(
    private toastrService: ToastrService,
    private authService: AuthService,
    private localStorage: LocalStorageService) { }

  public ngOnInit() {
    (window as any).fbAsyncInit = function () {
      FB.init({
        appId: '233689695255604',
        cookie: true,
        xfbml: true,
        version: 'v11.0'
      });
      FB.AppEvents.logPageView();
    };

    (function (d, s, id) {
      var js, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) { return; }
      js = d.createElement(s); js.id = id;
      js.src = "https://connect.facebook.net/en_US/sdk.js";
      fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
  }

  public submitLogin() {
    FB.login((response) => {
      if (response.authResponse) {
        const facebookLogin: Facebook = {
          accessToken: response.authResponse['accessToken']
        }
        this.authService.facebookLogin(facebookLogin)
          .subscribe(response => {
            this.toastrService.success("Login successfully.");
            const token = (<any>response).token;
            this.localStorage.set("token", JSON.stringify(token));
            window.location.reload();
          }, _ => {
            this.toastrService.error("error");
          });
      }
    });
  }
}
