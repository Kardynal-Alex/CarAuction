import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Register } from 'src/app/models/auth-models/register';
import { CommonConstants } from 'src/app/common/constants/common-constants';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from '../login/login.component';
import { ErrorMessages } from 'src/app/common/constants/error-messages';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less']
})
export class RegisterComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private toastrService: ToastrService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    private formBuilder: UntypedFormBuilder
  ) {
    this.initRegisterForm();
  }

  public registerForm: UntypedFormGroup;
  public ngOnInit(): void {
  }

  private initRegisterForm() {
    this.registerForm = this.formBuilder.group({
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required, Validators.minLength(6)]],
      name: [null, [Validators.required, Validators.pattern('[a-zA-Z]*')]],
      surname: [null, [Validators.required, Validators.pattern('[a-zA-Z]*')]]
    });
  }

  public register() {
    const registerUser: Register = {
      email: this.registerForm.controls.email.value,
      password: this.registerForm.controls.password.value,
      role: CommonConstants.User,
      name: this.registerForm.controls.name.value,
      surname: this.registerForm.controls.surname.value,
      clientURI: `${window.location.href.includes('https') ? 'https' : 'http'}://localhost:4200/auction/auth/emailconfirmation`
    }
    this.authService.register(registerUser)
      .subscribe(_ => {
        this.toastrService.info('You redirect to login page.');
        this.openLoginForm();
      }, _ => {
        this.toastrService.error(ErrorMessages.Error);
      });
  }

  public openLoginForm() {
    this.activeModal.close();
    this.modalService.open(LoginComponent, { animation: false });
  }

  public close() {
    this.activeModal.close();
  }
}
