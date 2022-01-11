import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Lot } from 'src/app/models/lot';
import { LotService } from 'src/app/services/lot.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-user-lots',
  templateUrl: './user-lots.component.html',
  styleUrls: ['./user-lots.component.css']
})
export class UserLotsComponent implements OnInit, OnDestroy {
  lots:Lot[];
  constructor(private toastrService:ToastrService,
              private lotService:LotService,
              private localStorage: LocalStorageService) { }
  
  getUserId():string{
    var payload=JSON.parse(window.atob(this.localStorage.get('token').split('.')[1]));
    return payload.id;
  }

  init(){
    const userId = this.getUserId();
    this.lotService.getLotsByUserId(userId).subscribe(response=>{
      this.lots=response;
      for(let lot of response){
        this.initTimer(lot['id'],lot['startDateTime']);
      }
    });
  }

  ngOnInit(){
    this.init(); 
  }

  ngOnDestroy(){
    for(let lot of this.lots){
      clearInterval(this.str[lot['id']]);
    }
  }
  
  deleteLot(id:number){
    if(confirm("Are you sure?")){
      this.lotService.deleteLotById(id).subscribe(response=>{
        this.lots=this.lots.filter(x=>x['id']!=id);
        clearInterval(this.str[id]);
        this.toastrService.success("Lot is deleted!");
      },
      error=>{
        this.toastrService.error("Something went wrong!");
      })
    }
  }

  endBid(lotEnd:Lot){
    if(confirm("Are you sure?")){
      lotEnd.IsSold=true;
      this.lotService.updateLot(lotEnd).subscribe(response=>{
        lotEnd['isSold']=true;
        clearInterval(this.str[lotEnd['id']]);
        document.getElementById('demo-'+lotEnd['id']).innerHTML = "Expired";
        this.toastrService.success("Lot is closed!");
      },
      error=>{
        this.toastrService.error("Something went wrong!");
      })
    }
  }

  public createImgPath(serverPath: string){
    return this.lotService.createImgPath(serverPath);
  }

  
  str={};
  initTimer(id:number,date:Date){
    var dead=new Date(date);
    dead.setDate(dead.getDate()+15);
    var deadline=new Date(dead).getTime();
    //deadline=new Date("Jul 2, 2021 09:44:00").getTime();
    this.str[id] = setInterval(()=> {
    var now = new Date().getTime();
    var t = deadline - now;
    var days = Math.floor(t / (1000 * 60 * 60 * 24));
    var hours = Math.floor((t % (1000 * 60 * 60 * 24))/(1000 * 60 * 60));
    var minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((t % (1000 * 60)) / 1000);
   
      document.getElementById('demo-'+id).innerHTML = days + "d " + hours + "h " + minutes + "m " + seconds + "s ";
        if (t < 0) {
            clearInterval(this.str[id]);
            this.checkLotIfTimerIsExpired(id);
            return;
        }
    }, 1000);
  }

  checkLotIfTimerIsExpired(id:number){
    var index=this.lots.findIndex(x=>x['id']==id);
    var lot=this.lots[index];

    if(parseFloat(lot['startPrice'])<parseFloat(lot['currentPrice'])){
      this.lots=this.lots.filter(x=>x['id']!=id);
      return;
    }else
    if(parseFloat(lot['startPrice'])===parseFloat(lot['currentPrice'])){
      this.initTimer(lot['id'],lot['startDateTime']);
      return;
    }
  }
}
