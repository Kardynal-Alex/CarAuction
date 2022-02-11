import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  constructor(private authService: AuthService) { }

  public IsAuthenticated: boolean = false;
  @Input() IsAdmin: boolean;
  public ngOnInit() {
    this.IsAuthenticated = this.authService.isAuthenticated();
  }

  public ngDoCheck() {
    this.IsAuthenticated = this.authService.isAuthenticated();
  }
}
