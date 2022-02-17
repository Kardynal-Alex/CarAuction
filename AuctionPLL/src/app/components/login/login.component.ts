import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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
    private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder
  ) {
    this.initLoginForm();
  }

  public modalOptions: NgbModalOptions;
  public loginForm: FormGroup;
  public ngOnInit() {
    this.isAuth = this.authService.isAuthenticated();
  }

  private initLoginForm() {
    this.loginForm = this.formBuilder.group({
      Email: [null, [Validators.required, Validators.email]],
      Password: [null, [Validators.required, Validators.minLength(6)]]
    });
  }

  public ngDoCheck() {
    this.isAuth = this.authService.isAuthenticated();
  }

  public isAuth: boolean = false;
  public login() {
    const loginUser = this.loginForm.value;
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
            this.modalService.open(ForgotPasswordComponent, { animation: false });
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

  public openRegisterForm() {
    this.activeModal.close();
    this.modalService.open(RegisterComponent, { animation: false });
  }

  public openResetForm() {
    this.activeModal.close();
    this.modalService.open(ForgotPasswordComponent, { animation: false });
  }

  public close() {
    this.activeModal.close();
  }
}
