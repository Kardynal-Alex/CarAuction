import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from '../../models/login';
import { AuthService } from '../../services/auth.service';
import { LocalStorageService } from '../../services/local-storage.service';
import { ToastrService } from 'ngx-toastr';
import { CommonConstants } from 'src/app/common/common-constants';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { RegisterComponent } from '../register/register.component';
import { ForgotPasswordComponent } from '../forgot-password/forgot-password.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private toastrService: ToastrService,
    private router: Router,
    private localStorage: LocalStorageService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal
  ) { }

  modalOptions: NgbModalOptions;
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
            this.activeModal.close();
            this.modalService.open(ForgotPasswordComponent);
          } else {
            this.toastrService.success("Login successfully.");
            this.activeModal.close();
            const token = (<any>response).token;
            this.localStorage.set(CommonConstants.JWTToken, JSON.stringify(token));
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

  public openRegisterForm() {
    this.activeModal.close();
    this.modalService.open(RegisterComponent);
  }

  public openResetForm() {
    this.activeModal.close();
    this.modalService.open(ForgotPasswordComponent);
  }

  public close() {
    this.activeModal.close();
  }
}
