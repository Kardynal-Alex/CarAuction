import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LotService } from 'src/app/services/lot.service';
import { Lot } from '../../../models/lot-models/lot';
import { FavoriteService } from 'src/app/services/favorite.service';
import { Favorite } from 'src/app/models/favorite';
import { AuthService } from 'src/app/services/auth.service';
import { Guid } from 'guid-typescript';
import { SortMode } from 'src/app/common/sort-mode';
import { ErrorMessages } from 'src/app/common/constants/error-messages';

@Component({
  selector: 'app-show-lots',
  templateUrl: './show-lots.component.html',
  styleUrls: ['./show-lots.component.less']
})
export class ShowLotsComponent implements OnInit {

  public get SortViewMode(): typeof SortMode {
    return SortMode;
  }

  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private favoriteService: FavoriteService,
    private authService: AuthService
  ) { }

  public str = {};
  public userId: string = '';
  public isAuth: boolean = false;
  public lots: Lot[];
  public ngOnInit() {
    this.sortField = '';
    this.sortMode = null;
    this.isAuth = this.authService.isAuthenticated();
    if (this.isAuth) {
      this.userId = this.authService.getUserIdFromToken();
    }
    this.getLots();
  }

  public markStars() {
    this.favoriteService.getUserFavorite(this.userId)
      .subscribe(response => {
        this.favorites = response;
        setTimeout(() => {
          for (let favorite of response) {
            var x = document.getElementById("star-" + favorite.lotId);
            if (x != null)
              x.className = "star";
          }
        }, 500);
      });
  }

  public getLots() {
    setTimeout(() =>
      this.lotService.getAllLots()
        .subscribe(res => {
          this.lots = res;
          for (let lot of res) {
            this.initTimer(lot.id, lot.startDateTime);
            if (this.isAuth) {
              this.markStars();
            }
          }
        })
      , 1000);
  }

  public sortMode: SortMode;
  public sortField: string;
  public sortingData(sortmode: SortMode, sortfield: string) {
    this.sortMode = sortmode;
    this.sortField = sortfield;
  }

  public changeStar(lotId: number) {
    document.getElementById('star-' + lotId).className == 'unstar' ?
      this.addToFavorite(lotId) : this.removeFromFavorite(lotId);
  }

  public favorites: Favorite[];
  public addToFavorite(lotId: number) {
    if (this.isAuth) {
      const favorite: Favorite = {
        id: Guid.create().toString(),
        userId: this.userId,
        lotId: lotId
      };
      this.favoriteService.addFavorite(favorite)
        .subscribe(_ => {
          document.getElementById('star-' + lotId).className = 'star';
          this.favorites.push(favorite);
        }, _ => {
          this.toastrService.info(ErrorMessages.Unauthorized);
        });
    } else {
      this.toastrService.info(ErrorMessages.Unauthorized);
    }
  }

  public removeFromFavorite(lotId: number) {
    const index = this.favorites.findIndex(x => x.lotId == lotId);
    const favoriteId = this.favorites[index].id;
    this.favoriteService.deleteFavoriteById(favoriteId)
      .subscribe(_ => {
        this.favorites = this.favorites.filter(x => x.id != favoriteId);
        document.getElementById('star-' + lotId).className = 'unstar';
      });
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }

  public checkLotIfTimerIsExpired(id: number) {
    var index = this.lots.findIndex(x => x.id == id);
    var lot = this.lots[index];

    if (lot.startPrice < lot.currentPrice) {
      lot.isSold = true;
      this.lotService.updateLotAfterClosing(lot)
        .subscribe(_ => {
          this.lots = this.lots.filter(x => x.id != id);
        }, _ => { });
      return;
    } else if (lot.startPrice === lot.currentPrice) {
      this.lotService.updateOnlyDateLot(lot)
        .subscribe(_ => {
          document.getElementById('timer-' + id).innerHTML = 'Expired';
          this.lots[index].startDateTime = new Date(Date.now());
        }, _ => { });
      this.initTimer(lot.id, this.lots[index].startDateTime);
      return;
    }
  }

  public initTimer(id: number, date: Date) {
    var dead = new Date(date);
    dead.setDate(dead.getDate() + 15);
    var deadline = new Date(dead).getTime();
    //deadline=new Date("Jul 31, 2021 12:30:00").getTime();
    this.str[id] = setInterval(() => {
      var now = new Date().getTime();
      var t = deadline - now;
      var days = Math.floor(t / (1000 * 60 * 60 * 24));
      var hours = Math.floor((t % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      var minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60));
      var seconds = Math.floor((t % (1000 * 60)) / 1000);

      if (document.getElementById('timer-' + id) != null && t >= 0)
        document.getElementById('timer-' + id).innerHTML = days + 'd ' + hours + 'h ' + minutes + 'm ' + seconds + 's ';
      if (t < 0) {
        clearInterval(this.str[id]);
        this.checkLotIfTimerIsExpired(id);
        return;
      }
    }, 1000);
  }

}

