import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Register } from 'src/app/models/register';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private toastrService: ToastrService) { }

  public ngOnInit(): void {
  }

  public register(form: NgForm) {
    const registerUser: Register = {
      Email: form.value.email,
      Password: form.value.password,
      Role: "user",
      Name: form.value.name,
      Surname: form.value.surname,
      ClientURI: "https://localhost:4200/auction/emailconfirmation"
    }
    this.authService.register(registerUser)
      .subscribe(_ => {
        this.toastrService.info("You redirect to login page.");
        document.getElementById('register-form').style.display = 'none';
        document.getElementById('login-form').style.display = 'block';
      }, _ => {
        this.toastrService.error("Error");
      });
  }

  public showLoginForm() {
    document.getElementById('login-form').style.display = 'block';
    document.getElementById('register-form').style.display = 'none';
  }
}
