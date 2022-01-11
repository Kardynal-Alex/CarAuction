import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Lot } from 'src/app/models/lot';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { LotService } from 'src/app/services/lot.service';
import { tap } from "rxjs/operators";
import { Guid } from 'guid-typescript';
import { Comment } from 'src/app/models/comment';
import { CommentService } from 'src/app/services/comment.service';
import { Favorite } from 'src/app/models/favorite';
import { FavoriteService } from 'src/app/services/favorite.service';

@Component({
  selector: 'app-show-lot-to-bid',
  templateUrl: './show-lot-to-bid.component.html',
  styleUrls: ['./show-lot-to-bid.component.css']
})
export class ShowLotToBidComponent implements OnInit,AfterViewInit,OnDestroy {
  id: number;
  constructor(private activateRoute:ActivatedRoute,
              private toastrService:ToastrService,
              private lotService:LotService,
              private localStorage:LocalStorageService,
              private commentService:CommentService,
              private favoriteService:FavoriteService) { }
    lot:Lot;
    comments:Comment[];
    filtredComments:Comment[];
    getLot(){
      this.id=this.activateRoute.snapshot.params['id'];
      this.lotService.getLotById(this.id).subscribe(response=>{
        this.lot=response;
        if(this.lot['isSold']===false)
          this.initTimer(this.id,this.lot['startDateTime']);
          else{
            setTimeout(()=>{
              document.getElementById('timer-'+this.lot['id']).innerHTML = "EXPIRED"
            },1000);
          }
      })
    }

  numbers=[];
  ngOnInit(){
    this.getLot();
    this.getUserId();
    this.getComments(this.id);
    this.numbers=Array.from(Array(9).keys());
  }

  ngAfterViewInit(){
    this.init();
  }

  ngOnDestroy() {
    this.id=null;
  }

  init(){
    if(this.userId.length>0){
      const favor:Favorite={
        id:"",
        userId:this.userId,
        lotId:this.lot['id']
      }
      this.favoriteService.getFavoriteByUserIdAndLotId(favor).subscribe(response=>{
        this.favorite=response;
        if(response!=null){
          document.getElementById("star-"+response['lotId']).className="star";
        }else{
          document.getElementById("star-"+this.id).className="unstar";
        }
      });
    }
  }

   getComments(lotId:number){
    this.commentService.getCommentsByLotId(lotId).pipe(tap(comments=>{
      this.comments=comments;
      this.filtredComments=comments;
    })).
    subscribe();
  } 

  userId:string;
  userName:string;
  userSurname:string;
  getUserId(){
    var token=this.localStorage.get('token');
    if(token!=null){
      var payload=JSON.parse(window.atob(token.split('.')[1]));
      this.userId=payload.id;
      this.userName=payload.name;
      this.userSurname=payload.surname;
    }
  }

  public createImgPath(serverPath: string){
    return this.lotService.createImgPath(serverPath);
  }

  placeBid(){
    var bid=prompt("Place bid more than current");
    if(parseFloat(bid)>this.lot['currentPrice'] && this.userId.length>0){
      this.lot['currentPrice']=parseFloat(bid);
      this.lot['lotState']['futureOwnerId']=this.userId;
      this.lot['lotState']['countBid']+=1;
      this.lotService.updateLot(this.lot).subscribe(response=>{
        this.toastrService.success("Thanks for bid");
        const comment:Comment={
          Id:Guid.create().toString(),
          Author:this.userName+" "+this.userSurname,
          Text:"Bid $"+parseFloat(bid).toString(),
          DateTime:new Date(Date.now()),
          LotId:this.id.toString(),
          UserId:this.userId,
          IsBid:true
        }
        this.commentService.addComment(comment).subscribe(response => {
          this.getComments(this.id);
        },
        error =>{
          this.toastrService.error("Cannot add comment!");
        });
      },
      errorresponse=>{
        this.toastrService.error("Error");
      }) 
    }
    else if(parseFloat(bid)<=this.lot['currentPrice'] || !(typeof bid==="number")){
            this.toastrService.error("Incorrect input data","Try again");
          } 
    else if(this.userId.length>0){
            this.toastrService.error("You must be registered","Try again");
          }
    }  

    changeStar(lotId:number){
      document.getElementById('star-'+lotId).className=='unstar'?
      this.addToFavorite(lotId):this.removeFromFavorite(lotId);
    }
  
    favorite:Favorite;
    addToFavorite(lotId:number){
      if(this.userId.length>0){
        const favorite:Favorite={
          id:Guid.create().toString(),
          userId:this.userId,
          lotId:lotId
        }
        this.favoriteService.addFavorite(favorite).subscribe(()=>{
          document.getElementById('star-'+lotId).className="star";
          this.favorite=favorite;
        },
        error=>{
          this.toastrService.info("You need to be authorized!");
        });
      }else{
        this.toastrService.info("You need to be authorized!");
      }
    }
  
    removeFromFavorite(lotId:number){
      this.favoriteService.deleteFavoriteById(this.favorite['id']).subscribe(()=>{
        document.getElementById('star-'+lotId).className="unstar";
        this.favorite=null;
      }); 
    }

    initTimer(id:number,date:Date){
      var dead=new Date(date);
      dead.setDate(dead.getDate()+15);
      var deadline=new Date(dead).getTime();
      //deadline=new Date("Jul 1, 2021 11:05:00").getTime();
      var x = setInterval(()=> {
      var now = new Date().getTime();
      var t = deadline - now;
      var days = Math.floor(t / (1000 * 60 * 60 * 24));
      var hours = Math.floor((t % (1000 * 60 * 60 * 24))/(1000 * 60 * 60));
      var minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60));
      var seconds = Math.floor((t % (1000 * 60)) / 1000);
      if(this.id==null)
        clearInterval(x);

        document.getElementById('timer-'+id).innerHTML = "#Timer "+ days + "d " + hours + "h " + minutes + "m " + seconds + "s ";
          if (t < 0) {
              clearInterval(x);
              this.checkLotIfTimerIsExpired(id);
              document.getElementById('timer-'+id).innerHTML = "EXPIRED";
          }
      }, 1000);
    }

    checkLotIfTimerIsExpired(id:number){
      if(this.lot['startPrice']==this.lot['currentPrice']){
        this.lot['startDateTime']=new Date(Date.now());
        this.initTimer(this.lot['id'],this.lot['startDateTime']);
      }
      else
      if(this.lot['startPrice']<this.lot['currentPrice']){
        this.lot['isSold']=true;
      }
    }

    showImages(){
      document.getElementById('myNav').style.display="block";
    }
}
