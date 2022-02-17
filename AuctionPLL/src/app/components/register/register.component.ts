import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
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
    private activeModal: NgbActiveModal
  ) { }

  public ngOnInit(): void {
  }

  public register(form: NgForm) {
    const registerUser: Register = {
      Email: form.value.email,
      Password: form.value.password,
      Role: CommonConstants.User,
      Name: form.value.name,
      Surname: form.value.surname,
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
