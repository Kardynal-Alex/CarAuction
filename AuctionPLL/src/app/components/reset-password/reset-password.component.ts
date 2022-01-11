import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ResetPassword } from 'src/app/models/reset-password';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  constructor(private toastrService:ToastrService,
              private authService:AuthService,
              private router:Router,
              private acticeRoute: ActivatedRoute) { }
  private token: string;
  private email: string;
            
  ngOnInit(): void {
    this.token = this.acticeRoute.snapshot.queryParams['token'];
    this.email = this.acticeRoute.snapshot.queryParams['email'];
  }

  resetPassword(form:NgForm){
    const resetPass: ResetPassword = {
      Password: form.value.NewPassword,
      Token: this.token,
      Email: this.email
    }

    this.authService.resetPassword(resetPass).subscribe(response => {
      this.toastrService.success("Password is updated!");
      this.router.navigate(['']);
    },
    error => {
      this.toastrService.error("Error!");
      this.router.navigate(['']);
    })
  }
}
