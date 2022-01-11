import { Component, OnInit , Input } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  constructor(private authService:AuthService){} 

  IsAuthenticated:boolean=false;
  @Input() IsAdmin:boolean;
  ngOnInit(){
    this.IsAuthenticated=this.authService.isAuthenticated();
  }
  
  ngDoCheck() {
    this.IsAuthenticated=this.authService.isAuthenticated();
  }
}
