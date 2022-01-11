import { Component, OnInit, Output } from '@angular/core';
import { Input } from '@angular/core';
import { Lot } from 'src/app/models/lot';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-show-lot-images',
  templateUrl: './show-lot-images.component.html',
  styleUrls: ['./show-lot-images.component.css']
})
export class ShowLotImagesComponent implements OnInit {

  constructor(private lotService:LotService) { }
  ngOnInit() {
    this.numbers=Array.from(Array(this.lotService.numbersOfImages).keys());
  }
  numbers=[];
  @Input() lot:Lot;
  closeImages(){
    document.getElementById('myNav').style.display="none";
  }

  public createImgPath(serverPath: string){
    return this.lotService.createImgPath(serverPath);
  }
}
