import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorMessages } from 'src/app/common/constants/error-messages';
import { ResetPasswordViewModel } from 'src/app/generated-models/auth-models/reset-password-view-model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.less']
})
export class ResetPasswordComponent implements OnInit {

  private token: string;
  private email: string;
  constructor(
    private toastrService: ToastrService,
    private authService: AuthService,
    private router: Router,
    private acticeRoute: ActivatedRoute
  ) { }

  public ngOnInit(): void {
    this.token = this.acticeRoute.snapshot.queryParams['token'];
    this.email = this.acticeRoute.snapshot.queryParams['email'];
  }

  public resetPassword(form: NgForm) {
    const resetPass: ResetPasswordViewModel = {
      password: form.value.newPassword,
      token: this.token,
      email: this.email
    };

    this.authService.resetPassword(resetPass)
      .subscribe((_) => {
        this.toastrService.success('Password is updated!');
        this.router.navigate(['']);
      }, (_) => {
        this.toastrService.error(ErrorMessages.Error);
        this.router.navigate(['']);
      });
  }
}
