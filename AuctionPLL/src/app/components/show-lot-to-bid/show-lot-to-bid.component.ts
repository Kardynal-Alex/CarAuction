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
import { CommonConstants } from 'src/app/common/common-constants';

@Component({
  selector: 'app-show-lot-to-bid',
  templateUrl: './show-lot-to-bid.component.html',
  styleUrls: ['./show-lot-to-bid.component.less']
})
export class ShowLotToBidComponent implements OnInit, AfterViewInit, OnDestroy {
  public id: number;
  constructor(
    private activateRoute: ActivatedRoute,
    private toastrService: ToastrService,
    private lotService: LotService,
    private localStorage: LocalStorageService,
    private commentService: CommentService,
    private favoriteService: FavoriteService) { }
  public lot: Lot;
  public comments: Comment[];
  public filtredComments: Comment[];
  public getLot() {
    this.id = this.activateRoute.snapshot.params['id'];
    this.lotService.getLotById(this.id)
      .subscribe(response => {
        this.lot = response;
        if (!!this.lot['isSold'])
          this.initTimer(this.id, this.lot['startDateTime']);
        else {
          setTimeout(() => {
            document.getElementById('timer-' + this.lot['id']).innerHTML = "EXPIRED"
          }, 1000);
        }
      });
  }

  public numbers = [];
  public ngOnInit() {
    this.getLot();
    this.getUserId();
    this.getComments(this.id);
    this.numbers = Array.from(Array(9).keys());
  }

  public ngAfterViewInit() {
    this.init();
  }

  public ngOnDestroy() {
    this.id = null;
  }

  public init() {
    if (this.userId.length > 0) {
      const favor: Favorite = {
        id: "",
        userId: this.userId,
        lotId: this.lot['id']
      };
      this.favoriteService.getFavoriteByUserIdAndLotId(favor)
        .subscribe(response => {
          this.favorite = response;
          if (response != null) {
            document.getElementById("star-" + response['lotId']).className = "star";
          } else {
            document.getElementById("star-" + this.id).className = "unstar";
          }
        });
    }
  }

  public getComments(lotId: number) {
    this.commentService.getCommentsByLotId(lotId)
      .pipe(
        tap(comments => {
          this.comments = comments;
          this.filtredComments = comments;
        }))
      .subscribe();
  }

  public userId: string;
  public userName: string;
  public userSurname: string;
  public getUserId() {
    var token = this.localStorage.get(CommonConstants.JWTToken);
    if (token != null) {
      var payload = JSON.parse(window.atob(token.split('.')[1]));
      this.userId = payload.id;
      this.userName = payload.name;
      this.userSurname = payload.surname;
    }
  }

  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }

  public placeBid() {
    var bid = prompt("Place bid more than current");
    if (parseFloat(bid) > this.lot['currentPrice'] && this.userId.length > 0) {
      this.lot['currentPrice'] = parseFloat(bid);
      this.lot['lotState']['futureOwnerId'] = this.userId;
      this.lot['lotState']['countBid'] += 1;
      this.lotService.updateLot(this.lot)
        .subscribe(_ => {
          this.toastrService.success("Thanks for bid");
          const comment: Comment = {
            Id: Guid.create().toString(),
            Author: this.userName + " " + this.userSurname,
            Text: "Bid $" + parseFloat(bid).toString(),
            DateTime: new Date(Date.now()),
            LotId: this.id.toString(),
            UserId: this.userId,
            IsBid: true
          };
          this.commentService.addComment(comment)
            .subscribe(_ => {
              this.getComments(this.id);
            }, _ => {
              this.toastrService.error("Cannot add comment!");
            });
        }, _ => {
          this.toastrService.error("Error");
        });
    }
    else if (parseFloat(bid) <= this.lot['currentPrice'] || !(typeof bid === "number")) {
      this.toastrService.error("Incorrect input data", "Try again");
    }
    else if (this.userId.length > 0) {
      this.toastrService.error("You must be registered", "Try again");
    }
  }

  public changeStar(lotId: number) {
    document.getElementById('star-' + lotId).className == 'unstar' ?
      this.addToFavorite(lotId) : this.removeFromFavorite(lotId);
  }

  public favorite: Favorite;
  public addToFavorite(lotId: number) {
    if (this.userId.length > 0) {
      const favorite: Favorite = {
        id: Guid.create().toString(),
        userId: this.userId,
        lotId: lotId
      };
      this.favoriteService.addFavorite(favorite)
        .subscribe(_ => {
          document.getElementById('star-' + lotId).className = "star";
          this.favorite = favorite;
        }, _ => {
          this.toastrService.info("You need to be authorized!");
        });
    } else {
      this.toastrService.info("You need to be authorized!");
    }
  }

  public removeFromFavorite(lotId: number) {
    this.favoriteService.deleteFavoriteById(this.favorite['id'])
      .subscribe(_ => {
        document.getElementById('star-' + lotId).className = "unstar";
        this.favorite = null;
      });
  }

  public initTimer(id: number, date: Date) {
    var dead = new Date(date);
    dead.setDate(dead.getDate() + 15);
    var deadline = new Date(dead).getTime();
    //deadline=new Date("Jul 1, 2021 11:05:00").getTime();
    var x = setInterval(() => {
      var now = new Date().getTime();
      var t = deadline - now;
      var days = Math.floor(t / (1000 * 60 * 60 * 24));
      var hours = Math.floor((t % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      var minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60));
      var seconds = Math.floor((t % (1000 * 60)) / 1000);
      if (this.id == null)
        clearInterval(x);

      document.getElementById('timer-' + id).innerHTML = "#Timer " + days + "d " + hours + "h " + minutes + "m " + seconds + "s ";
      if (t < 0) {
        clearInterval(x);
        this.checkLotIfTimerIsExpired(id);
        document.getElementById('timer-' + id).innerHTML = "EXPIRED";
      }
    }, 1000);
  }

  public checkLotIfTimerIsExpired(id: number) {
    if (this.lot['startPrice'] == this.lot['currentPrice']) {
      this.lot['startDateTime'] = new Date(Date.now());
      this.initTimer(this.lot['id'], this.lot['startDateTime']);
    }
    else if (this.lot['startPrice'] < this.lot['currentPrice']) {
      this.lot['isSold'] = true;
    }
  }

  public showImages() {
    document.getElementById('myNav').style.display = "block";
  }
}
