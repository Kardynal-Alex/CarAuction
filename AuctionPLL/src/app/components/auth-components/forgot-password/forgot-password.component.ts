import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { ForgotPassword } from 'src/app/models/auth-models/forgot-password';
import { AuthService } from 'src/app/services/auth.service';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.less']
})
export class ForgotPasswordComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private toastrService: ToastrService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal
  ) { }

  public ngOnInit(): void { }

  public openLoginForm() {
    this.activeModal.close();
    this.modalService.open(LoginComponent, { animation: false });
  }

  public resetPassForm(form: NgForm) {
    const forgotPass: ForgotPassword = {
      email: form.value.email,
      clientURI: `${window.location.href.includes('https') ? 'https' : 'http'}://localhost:4200/auction/auth/resetpassword`
    };
    this.activeModal.close();
    this.authService.forgotPassword(forgotPass)
      .subscribe(_ => {
        this.toastrService.success('Please check your email to reset your password', 'The link has been sent');
      }, _ => {
        this.toastrService.error('Incorect email');
      });
  }

  public close() {
    this.activeModal.close();
  }
}
