import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.less']
})
export class MenuComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private modalService: NgbModal,
    private toastrService: ToastrService,
    private router: Router
  ) { }

  public IsAuthenticated: boolean = false;
  @Input() IsAdmin: boolean;
  public ngOnInit() {
    this.IsAuthenticated = this.authService.isAuthenticated();
  }

  public openLoginForm() {
    this.modalService.open(LoginComponent, { animation: false });
  }

  public logout() {
    this.authService.logout()
      .subscribe(_ => {
        this.IsAuthenticated = false;
        this.router.navigate(['/']);
        window.location.reload();
        this.toastrService.success("Logout successfully.");
      }, _ => {
        this.toastrService.error("Error!");
      });
  }
}
