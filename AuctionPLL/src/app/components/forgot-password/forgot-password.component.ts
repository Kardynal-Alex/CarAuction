import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ForgotPassword } from 'src/app/models/forgot-password';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private toastrService: ToastrService) { }

  public ngOnInit(): void {
  }

  public showLoginForm() {
    document.getElementById('forgot-pass-form').style.display = 'none';
    document.getElementById('login-form').style.display = 'block';
  }

  public resetPassForm(form: NgForm) {
    const forgotPass: ForgotPassword = {
      Email: form.value.Email,
      ClientURI: 'https://localhost:4200/auction/resetpassword'
    }
    document.getElementById('forgot-pass-form').style.display = 'none';
    this.authService.forgotPassword(forgotPass)
      .subscribe(_ => {
        this.toastrService.success("Please check your email to reset your password", "The link has been sent");
      }, _ => {
        this.toastrService.error("Incorect email");
      });
  }
}
