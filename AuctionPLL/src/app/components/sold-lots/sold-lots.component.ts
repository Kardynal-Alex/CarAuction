import { Component, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot';
import { LotService } from 'src/app/services/lot.service';
import { tap } from "rxjs/operators";

@Component({
  selector: 'app-sold-lots',
  templateUrl: './sold-lots.component.html',
  styleUrls: ['./sold-lots.component.css','../show-lots/show-lots.component.css']
})
export class SoldLotsComponent implements OnInit {

  constructor(private lotService:LotService) { }

  lots:Lot[];
  ngOnInit(){
    this.getLots();
  }

  getLots(){
    this.lotService.getSoldLots().pipe(tap(lots=>this.lots=lots)).subscribe();
  }

  public createImgPath(serverPath: string){
    return this.lotService.createImgPath(serverPath);
  }
}
