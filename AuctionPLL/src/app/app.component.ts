import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  IsAdmin:boolean=false;
  constructor(private authService:AuthService){}

  ngOnInit(){
    this.IsAdmin=this.authService.CheckIfIsAdmin();
  }
}
//run app with https protocol
//ng serve --ssl --ssl-cert D:\\EpamProject\\FinalProject\\AuctionPLL\\localhost.crt --ssl-key D:\\EpamProject\\FinalProject\\AuctionPLL\\localhost.key
