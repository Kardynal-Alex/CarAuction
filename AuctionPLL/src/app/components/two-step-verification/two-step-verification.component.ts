import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TwoFactor } from 'src/app/models/two-factor';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-two-step-verification',
  templateUrl: './two-step-verification.component.html',
  styleUrls: ['./two-step-verification.component.css']
})
export class TwoStepVerificationComponent implements OnInit {

  constructor(private toastrService:ToastrService,
              private route:ActivatedRoute,
              private authService:AuthService,
              private localStorage:LocalStorageService,
              private router:Router) { }

  private provider: string;
  private email: string;
  ngOnInit(){
    document.getElementById("2-step-form").style.display="block";
  }

  login(form:NgForm){
    this.provider = this.route.snapshot.queryParams['provider'];
    this.email = this.route.snapshot.queryParams['email'];
    let twoFactor: TwoFactor = {
      email: this.email,
      provider: this.provider,
      token: form.value.token
    }
    this.authService.twoSteplogin(twoFactor).subscribe(response=>{
      this.toastrService.success("Login successfully.");
      this.localStorage.set("token", response['token']);
      this.router.navigate(['']);
      document.getElementById("2-step-form").style.display="none";
    }, 
    error => {
      this.toastrService.error("Something went wrong");
    });
  }

}
