import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LotService } from 'src/app/services/lot.service';
import { Lot } from '../../../models/lot-models/lot';
import { FavoriteService } from 'src/app/services/favorite.service';
import { Favorite } from 'src/app/models/favorite';
import { AuthService } from 'src/app/services/auth.service';
import { Guid } from 'guid-typescript';
import { ErrorMessages } from 'src/app/common/constants/error-messages';
import { getStarId, getTimerId } from 'src/app/utils/element-id.service';
import { FavoriteConstants } from 'src/app/common/constants/favorite-constants';
import { SortOrder } from 'src/app/models/filter/sort-order';
import { FilterService } from 'src/app/services/filter.service';

export class FilterConstants {
  public static readonly Year = 'year';
  public static readonly StartDateTime = 'startDateTime';
  public static readonly CurrentPrice = 'currentPrice';
}

@Component({
  selector: 'app-show-lots',
  templateUrl: './show-lots.component.html',
  styleUrls: ['./show-lots.component.less']
})
export class ShowLotsComponent implements OnInit {

  public get SortViewMode(): typeof SortOrder {
    return SortOrder;
  }
  public get FilterConstants(): typeof FilterConstants {
    return FilterConstants;
  }

  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private favoriteService: FavoriteService,
    private authService: AuthService,
    private filterService: FilterService
  ) { }

  public str = {};
  public userId: string = '';
  public isAuth: boolean = false;
  public lots: Lot[];
  public filterConstants: FilterConstants;
  public ngOnInit() {
    this.isAuth = this.authService.isAuthenticated();
    if (this.isAuth) {
      this.userId = this.authService.getUserIdFromToken();
    }
    this.getLots();
  }

  private markStars() {
    this.favoriteService.getUserFavorite(this.userId)
      .subscribe((response) => {
        this.favorites = response;
        setTimeout(() => {
          for (let favorite of response) {
            var x = document.getElementById(getStarId(favorite.lotId));
            if (x != null)
              x.className = FavoriteConstants.STAR;
          }
        }, 500);
      });
  }

  public getLots() {
    setTimeout(() =>
      this.lotService.getAllLots()
        .subscribe((res) => {
          this.lots = res;
          this.initTimerAndMarkStars(res);
        })
      , 1000);
  }

  private initTimerAndMarkStars(lots: Lot[]) {
    for (let lot of lots) {
      this.initTimer(lot.id, lot.startDateTime);
      if (this.isAuth) {
        this.markStars();
      }
    }
  }

  public sortingData(sortOrder: SortOrder, sortfield: string) {
    this.filterService.sortingData(sortOrder, sortfield)
      .subscribe((lots) => {
        this.lots = lots;
        this.initTimerAndMarkStars(lots);
      });
  }

  public changeStar(lotId: number) {
    document.getElementById(getStarId(lotId)).className == FavoriteConstants.UNSTAR ?
      this.addToFavorite(lotId) : this.removeFromFavorite(lotId);
  }

  public favorites: Favorite[];
  private addToFavorite(lotId: number) {
    if (this.isAuth) {
      const favorite: Favorite = {
        id: Guid.create().toString(),
        userId: this.userId,
        lotId: lotId
      };
      this.favoriteService.addFavorite(favorite)
        .subscribe((_) => {
          document.getElementById(getStarId(lotId)).className = FavoriteConstants.STAR;
          this.favorites.push(favorite);
        }, (_) => {
          this.toastrService.info(ErrorMessages.Unauthorized);
        });
    } else {
      this.toastrService.info(ErrorMessages.Unauthorized);
    }
  }

  private removeFromFavorite(lotId: number) {
    const index = this.favorites.findIndex(x => x.lotId == lotId);
    const favoriteId = this.favorites[index].id;
    this.favoriteService.deleteFavoriteById(favoriteId)
      .subscribe((_) => {
        this.favorites = this.favorites.filter(x => x.id != favoriteId);
        document.getElementById(getStarId(lotId)).className = FavoriteConstants.UNSTAR;
      });
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }

  private checkLotIfTimerIsExpired(id: number) {
    var index = this.lots.findIndex(x => x.id == id);
    var lot = this.lots[index];

    if (lot.startPrice < lot.currentPrice) {
      lot.isSold = true;
      this.lotService.updateLotAfterClosing(lot)
        .subscribe(() => {
          this.lots = this.lots.filter(x => x.id != id);
        });
      return;
    } else if (lot.startPrice === lot.currentPrice) {
      this.lotService.updateOnlyDateLot(lot)
        .subscribe(() => {
          document.getElementById(getTimerId(id)).innerHTML = 'Expired';
          this.lots[index].startDateTime = new Date(Date.now());
        });
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

      if (document.getElementById(getTimerId(id)) != null && t >= 0) {
        document.getElementById(getTimerId(id)).innerHTML = `${days}d ${hours}h ${minutes}m ${seconds}s`;
      }
      if (t < 0) {
        clearInterval(this.str[id]);
        this.checkLotIfTimerIsExpired(id);
        return;
      }
    }, 1000);
  }

}

