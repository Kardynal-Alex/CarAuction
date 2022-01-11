import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css']
})
export class EmailConfirmationComponent implements OnInit {

  constructor(private authService: AuthService, 
              private route: ActivatedRoute,
              private toastrService: ToastrService,
              private router:Router) { }

  ngOnInit(): void {
    this.confirmEmail();
  }

  confirmEmail(){
    const token = this.route.snapshot.queryParams['token'];
    const email = this.route.snapshot.queryParams['email'];

    this.authService.confirmEmail('api/account/emailconfirmation', token, email)
    .subscribe(response => {
      this.toastrService.success("Succesfully confirmed");
      this.router.navigate(['']);
    },
    error => {
      this.toastrService.error("Need to confirm email");
      this.router.navigate(['']);
    });
  }
}
