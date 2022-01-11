import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LotService } from 'src/app/services/lot.service';
import { Lot } from '../../models/lot';
import { FavoriteService } from 'src/app/services/favorite.service';
import { Favorite } from 'src/app/models/favorite';
import { AuthService } from 'src/app/services/auth.service';
import { Guid } from 'guid-typescript';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-show-lots',
  templateUrl: './show-lots.component.html',
  styleUrls: ['./show-lots.component.css']
})
export class ShowLotsComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(private toastrService:ToastrService,
              private lotService:LotService,
              private favoriteService:FavoriteService,
              private authService:AuthService,
              private route:ActivatedRoute) { }
  str={};
  userId:string='';
  isAuth:boolean=false;
  lots:Lot[];
  ngOnInit() {
    this.sortField='';
    this.sortMode='';
    this.isAuth=this.authService.isAuthenticated();
    if(this.isAuth){
      this.userId=this.authService.getUserId();
    } 
    this.getLots();
  }

  ngAfterViewInit(){
    if(this.isAuth){
      this.init();
    }
  }

  ngOnDestroy(){
  }

  init(){
    this.favoriteService.getUserFavorite(this.userId).subscribe(response=>{
      this.favorites=response;
      setTimeout(() => {
        for(let favorite of response){
          var x=document.getElementById("star-"+favorite['lotId']);
          if(x!=null)
            x.className="star";
        }
      },1000); 
    });
  }

  getLots(){
    this.lotService.getAllLots().subscribe(res=>{
      this.lots=res;
      for(let lot of res){
          this.initTimer(lot['id'],lot['startDateTime']);
      }
    });
  }

  sortMode:string;
  sortField:string;
  sortingData(sortmode:string,sortfield:string){
    this.sortMode=sortmode;
    this.sortField=sortfield;
  }

  changeStar(lotId:number){
    document.getElementById('star-'+lotId).className=='unstar'?
        this.addToFavorite(lotId):this.removeFromFavorite(lotId);
  }

  favorites:Favorite[];
  addToFavorite(lotId:number){
    if(this.isAuth){
      const favorite:Favorite={
        id:Guid.create().toString(),
        userId:this.userId,
        lotId:lotId
      }
      this.favoriteService.addFavorite(favorite).subscribe(()=>{
        document.getElementById('star-'+lotId).className="star";
        this.favorites.push(favorite);
      },
      error=>{
        this.toastrService.info("You need to be authorized!");
      });
    }else{
      this.toastrService.info("You need to be authorized!!");
    }
  }

  removeFromFavorite(lotId:number){
    const index=this.favorites.findIndex(x=>x['lotId']==lotId);
    const favoriteId=this.favorites[index]['id'];
    this.favoriteService.deleteFavoriteById(favoriteId).subscribe(()=>{
      this.favorites=this.favorites.filter(x=>x['id']!=favoriteId);
      document.getElementById('star-'+lotId).className="unstar";
    }); 
  }

  public createImgPath(serverPath: string){
    return this.lotService.createImgPath(serverPath);
  }

  checkLotIfTimerIsExpired(id:number){
    var index=this.lots.findIndex(x=>x['id']==id);
    var lot=this.lots[index];

    if(parseFloat(lot['startPrice'])<parseFloat(lot['currentPrice'])){
      lot['isSold']=true;
      this.lotService.updateLotAfterClosing(lot).subscribe(response=>{
        this.lots=this.lots.filter(x=>x['id']!=id);
      },error=>{});
      return;
    }else
    if(parseFloat(lot['startPrice'])===parseFloat(lot['currentPrice'])){
      this.lotService.updateOnlyDateLot(lot).subscribe(response=>{
        document.getElementById('demo-'+id).innerHTML = "Expired";
        this.lots[index]['startDateTime']=Date.now();
      },error=>{});
      this.initTimer(lot['id'],this.lots[index]['startDateTime']);
      return;
    }
  }
  
  initTimer(id:number,date:Date){
    var dead=new Date(date);
    dead.setDate(dead.getDate()+15);
    var deadline=new Date(dead).getTime();
    //deadline=new Date("Jul 31, 2021 12:30:00").getTime();
    this.str[id] = setInterval(()=> {
    var now = new Date().getTime();
    var t = deadline - now;
    var days = Math.floor(t / (1000 * 60 * 60 * 24));
    var hours = Math.floor((t % (1000 * 60 * 60 * 24))/(1000 * 60 * 60));
    var minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((t % (1000 * 60)) / 1000);

    if(document.getElementById('demo-'+id)!=null && t>=0)
      document.getElementById('demo-'+id).innerHTML = days + "d " + hours + "h " + minutes + "m " + seconds + "s ";
        if (t < 0) {
            clearInterval(this.str[id]);
            this.checkLotIfTimerIsExpired(id);
            return;
        }
    }, 1000);
  }

}

