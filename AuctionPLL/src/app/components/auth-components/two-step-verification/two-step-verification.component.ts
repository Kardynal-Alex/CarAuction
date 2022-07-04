import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonConstants } from 'src/app/common/constants/common-constants';
import { TwoFactor } from 'src/app/models/auth-models/two-factor';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-two-step-verification',
  templateUrl: './two-step-verification.component.html',
  styleUrls: ['./two-step-verification.component.less']
})
export class TwoStepVerificationComponent {

  private provider: string;
  private email: string;
  constructor(
    private toastrService: ToastrService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private localStorage: LocalStorageService,
    private router: Router
  ) { }

  public login(form: NgForm) {
    this.provider = this.route.snapshot.queryParams['provider'];
    this.email = this.route.snapshot.queryParams['email'];
    const twoFactor: TwoFactor = {
      email: this.email,
      provider: this.provider,
      token: form.value.token
    };
    this.authService.twoSteplogin(twoFactor)
      .subscribe((response) => {
        this.toastrService.success('Login successfully');
        this.localStorage.set(CommonConstants.JWTToken, response['token']);
        this.router.navigate(['']);
        document.getElementById('2-step-form').style.display = 'none';
        window.location.reload();
      }, (_) => {
        this.toastrService.error('Something went wrong');
      });
  }

}
