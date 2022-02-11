import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from '../../models/login';
import { AuthService } from '../../services/auth.service';
import { LocalStorageService } from '../../services/local-storage.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private toastrService: ToastrService,
    private router: Router,
    private localStorage: LocalStorageService) { }

  public ngOnInit() {
    this.isAuth = this.authService.isAuthenticated();
  }

  public ngDoCheck() {
    this.isAuth = this.authService.isAuthenticated();
  }

  public isAuth: boolean = false;
  public login(form: NgForm) {
    const loginUser: Login = {
      Email: form.value.email,
      Password: form.value.password
    }
    this.authService.login(loginUser)
      .subscribe(response => {
        if (response['is2StepVerificationRequired']) {
          document.getElementById('login-form').style.display = 'none';
          this.router.navigate(['/twostepverification'],
            {
              queryParams: {
                provider: response['provider'],
                email: loginUser.Email
              }
            });
        } else {
          let errorMessage = response['errorMessage'];
          if (!!errorMessage) {
            this.toastrService.error(errorMessage);
            document.getElementById('login-form').style.display = 'none';
            document.getElementById("forgot-pass-form").style.display = "block";
          } else {
            this.toastrService.success("Login successfully.");
            document.getElementById('login-form').style.display = 'none';
            const token = (<any>response).token;
            this.localStorage.set("token", JSON.stringify(token));
            this.isAuth = true;
            window.location.reload();
          }
        }
      }, _ => {
        this.toastrService.error("Incorect login or password!");
      });
  }

  public logout() {
    this.authService.logout()
      .subscribe(_ => {
        this.isAuth = false;
        this.router.navigate(['/']);
        window.location.reload();
        this.toastrService.success("Logout successfully.");
      }, _ => {
        this.toastrService.error("Error!");
      });
  }

  public showRegisterForm() {
    document.getElementById('login-form').style.display = 'none';
    document.getElementById('register-form').style.display = 'block';
  }

  public showResetForm() {
    document.getElementById('login-form').style.display = 'none';
    document.getElementById('forgot-pass-form').style.display = 'block';
  }
}
