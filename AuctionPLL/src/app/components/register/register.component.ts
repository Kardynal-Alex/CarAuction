import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Register } from 'src/app/models/register';
import { CommonConstants } from 'src/app/common/common-constants';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from '../login/login.component';

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
    private formBuilder: FormBuilder
  ) {
    this.initRegisterForm();
  }

  public registerForm: FormGroup;
  public ngOnInit(): void {
  }

  private initRegisterForm() {
    this.registerForm = this.formBuilder.group({
      Email: [null, [Validators.required, Validators.email]],
      Password: [null, [Validators.required, Validators.minLength(6)]],
      Name: [null, [Validators.required, Validators.pattern('[a-zA-Z]*')]],
      Surname: [null, [Validators.required, Validators.pattern('[a-zA-Z]*')]]
    });
  }

  public register() {
    console.log(this.registerForm.controls.Email.value)
    const registerUser: Register = {
      Email: this.registerForm.controls.Email.value,
      Password: this.registerForm.controls.Password.value,
      Role: CommonConstants.User,
      Name: this.registerForm.controls.Name.value,
      Surname: this.registerForm.controls.Surname.value,
      ClientURI: "https://localhost:4200/auction/emailconfirmation"
    }
    this.authService.register(registerUser)
      .subscribe(_ => {
        this.toastrService.info("You redirect to login page.");
        this.openLoginForm();
      }, _ => {
        this.toastrService.error("Error");
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
