import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Lot } from 'src/app/models/lot-models/lot';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { LotService } from 'src/app/services/lot.service';
import { tap } from 'rxjs/operators';
import { Guid } from 'guid-typescript';
import { Comment } from 'src/app/models/comment';
import { CommentService } from 'src/app/services/comment.service';
import { Favorite } from 'src/app/models/favorite';
import { FavoriteService } from 'src/app/services/favorite.service';
import { CommonConstants } from 'src/app/common/constants/common-constants';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShowLotImagesComponent } from '../show-lot-images/show-lot-images.component';
import { AskOwnerFormComponent } from '../ask-owner-form/ask-owner-form.component';
import { AuthService } from 'src/app/services/auth.service';
import { AuthorDescriptionService } from 'src/app/services/author-description.service';
import { ErrorMessages } from 'src/app/common/constants/error-messages';
import { getStarId, getTimerId } from 'src/app/utils/element-id.service';
import { FavoriteConstants } from 'src/app/common/constants/favorite-constants';
import { AuthorDescription } from 'src/app/models/lot-models/author-description';

@Component({
  selector: 'app-show-lot-to-bid',
  templateUrl: './show-lot-to-bid.component.html',
  styleUrls: ['./show-lot-to-bid.component.less']
})
export class ShowLotToBidComponent implements OnInit, OnDestroy {
  public get FavoriteConstants(): FavoriteConstants {
    return FavoriteConstants;
  }

  public id: number;
  constructor(
    private activateRoute: ActivatedRoute,
    private toastrService: ToastrService,
    private lotService: LotService,
    private localStorage: LocalStorageService,
    private commentService: CommentService,
    private favoriteService: FavoriteService,
    private modalService: NgbModal,
    private authService: AuthService,
    private authorDescriptionService: AuthorDescriptionService
  ) { }

  public lot: Lot;
  public comments: Comment[];
  public filtredComments: Comment[];
  public authorDescription: AuthorDescription;
  public getLot() {
    this.id = this.activateRoute.snapshot.params['id'];
    this.lotService.getLotById(this.id)
      .subscribe((response) => {
        this.lot = response;
        if (!this.lot.isSold) {
          this.initTimer(this.id, this.lot.startDateTime);
          this.initFavorite(response);
        } else {
          setTimeout(() => {
            document.getElementById(getTimerId(this.lot.id)).innerHTML = 'EXPIRED';
          }, 1000);
        }
      });
  }

  public getAuthorDescription() {
    this.authorDescriptionService.getAuthorDescriptionByLotId(this.id)
      .subscribe((_) => {
        this.authorDescription = _;
      });
  }

  public ngOnInit() {
    this.getLot();
    this.getUserInfo();
    this.getComments(this.id);
    this.getAuthorDescription();
  }

  public ngOnDestroy() {
    this.id = null;
  }

  private initFavorite(lot: Lot) {
    if (this.userId?.length > 0) {
      const favor = new Favorite(null, this.userId, lot.id);
      this.favoriteService.getFavoriteByUserIdAndLotId(favor)
        .subscribe((response) => {
          this.favorite = response;
          if (response != null) {
            document.getElementById(getStarId(response.lotId)).className = FavoriteConstants.STAR;
          } else {
            document.getElementById(getStarId(this.id)).className = FavoriteConstants.UNSTAR;
          }
        });
    }
  }

  public getComments(lotId: number) {
    this.commentService.getCommentsByLotId(lotId)
      .pipe(
        tap((comments) => {
          this.comments = comments;
          this.filtredComments = comments;
        }))
      .subscribe();
  }

  public userId: string;
  public userName: string;
  public userSurname: string;
  public getUserInfo() {
    var token = this.localStorage.get(CommonConstants.JWTToken);
    if (token != null) {
      var payload = this.authService.getPayload();
      this.userId = payload?.id;
      this.userName = payload?.name;
      this.userSurname = payload?.surname;
    }
  }

  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }

  public placeBid() {
    const bid = prompt('Place bid more than current');
    const parsedBid = parseFloat(bid);
    if (this.userId.length === 0) {
      return this.toastrService.error(ErrorMessages.Unauthorized);
    }
    if (parsedBid <= this.lot.currentPrice) {
      return this.toastrService.error(ErrorMessages.IncorrectData);
    }
    if (parsedBid > this.lot.currentPrice) {
      this.lot.currentPrice = parseFloat(bid);
      this.lot.lotState.futureOwnerId = this.userId;
      this.lot.lotState.countBid += 1;
      this.lotService.updateLot(this.lot)
        .subscribe((_) => {
          this.toastrService.success('Thanks for bid');
          const comment: Comment = {
            id: Guid.create().toString(),
            author: `${this.userName} ${this.userSurname}`,
            text: `Bid $ ${parsedBid}`,
            dateTime: new Date(Date.now()),
            lotId: this.id.toString(),
            userId: this.userId,
            isBid: true
          };
          this.commentService.addComment(comment)
            .subscribe((_) => {
              this.getComments(this.id);
            }, (_) => {
              this.toastrService.error('Cannot add comment!');
            });
        }, (_) => {
          this.toastrService.error(ErrorMessages.Error);
        });
    }
  }

  public changeStar(lotId: number) {
    document.getElementById(getStarId(lotId)).className == FavoriteConstants.UNSTAR ?
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
        .subscribe((_) => {
          document.getElementById(getStarId(lotId)).className = FavoriteConstants.STAR;
          this.favorite = favorite;
        }, (_) => {
          this.toastrService.info(ErrorMessages.Unauthorized);
        });
    } else {
      this.toastrService.info(ErrorMessages.Unauthorized);
    }
  }

  public removeFromFavorite(lotId: number) {
    this.favoriteService.deleteFavoriteById(this.favorite.id)
      .subscribe((_) => {
        document.getElementById(getStarId(lotId)).className = FavoriteConstants.UNSTAR;
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
      if (this.id == null) {
        clearInterval(x);
        return;
      }

      if (document.getElementById(getTimerId(id)) != null && t >= 0) {
        document.getElementById(getTimerId(id)).innerHTML = `#Timer ${days}d ${hours}h ${minutes}m ${seconds}s`;
      }
      if (t < 0) {
        clearInterval(x);
        this.checkLotIfTimerIsExpired(id);
        document.getElementById(getTimerId(id)).innerHTML = 'EXPIRED';
      }
    }, 1000);
  }

  public checkLotIfTimerIsExpired(id: number) {
    if (this.lot.startPrice == this.lot.currentPrice) {
      this.lot.startDateTime = new Date(Date.now());
      this.initTimer(this.lot.id, this.lot.startDateTime);
    } else if (this.lot.startPrice < this.lot.currentPrice) {
      this.lot.isSold = true;
    }
  }

  public showImages() {
    const modalRef = this.modalService.open(ShowLotImagesComponent, { animation: false });
    modalRef.componentInstance.lot = this.lot;
  }

  public openAskForm() {
    const modalRef = this.modalService.open(AskOwnerFormComponent, { animation: false });
    modalRef.componentInstance.ownerEmail = this.lot?.user?.email;
  }
}
