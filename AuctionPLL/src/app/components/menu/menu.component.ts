import { Component, OnInit, Input } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../services/auth.service';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.less']
})
export class MenuComponent implements OnInit {

  private modalOptions: NgbModalOptions;
  constructor(
    private authService: AuthService,
    private modalService: NgbModal
  ) {
    this.modalOptions = {
      // backdrop: 'static',
      // backdropClass: 'menu-bar'
    }
  }

  public IsAuthenticated: boolean = false;
  @Input() IsAdmin: boolean;
  public ngOnInit() {
    this.IsAuthenticated = this.authService.isAuthenticated();
  }

  public ngDoCheck() {
    this.IsAuthenticated = this.authService.isAuthenticated();
  }

  public openLoginForm() {
    this.modalService.open(LoginComponent, this.modalOptions);
  }
}
