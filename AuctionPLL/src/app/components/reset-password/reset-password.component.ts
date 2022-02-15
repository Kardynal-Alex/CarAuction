import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ResetPassword } from 'src/app/models/reset-password';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.less']
})
export class ResetPasswordComponent implements OnInit {

  constructor(
    private toastrService: ToastrService,
    private authService: AuthService,
    private router: Router,
    private acticeRoute: ActivatedRoute) { }
  private token: string;
  private email: string;

  public ngOnInit(): void {
    this.token = this.acticeRoute.snapshot.queryParams['token'];
    this.email = this.acticeRoute.snapshot.queryParams['email'];
  }

  public resetPassword(form: NgForm) {
    const resetPass: ResetPassword = {
      Password: form.value.NewPassword,
      Token: this.token,
      Email: this.email
    };

    this.authService.resetPassword(resetPass)
      .subscribe(_ => {
        this.toastrService.success("Password is updated!");
        this.router.navigate(['']);
      }, _ => {
        this.toastrService.error("Error!");
        this.router.navigate(['']);
      });
  }
}
