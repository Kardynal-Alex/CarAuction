import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LotService } from 'src/app/services/lot.service';
import { FavoriteService } from 'src/app/services/favorite.service';
import { AuthService } from 'src/app/services/auth.service';
import { Guid } from 'guid-typescript';
import { ErrorMessages } from 'src/app/common/constants/error-messages';
import { getStarId, getTimerId } from 'src/app/utils/element-id.service';
import { FavoriteConstants } from 'src/app/common/constants/favorite-constants';
import { FilterService } from 'src/app/services/filter.service';
import { MatSelect } from '@angular/material/select';
import { MatOption } from '@angular/material/core';
import { BehaviorSubject } from 'rxjs';
import { delay, tap } from 'rxjs/operators';
import { SortOrderViewModel } from 'src/app/generated-models/filter/sort-order-view-model';
import { CarBrandArray, CarBrands } from 'src/app/utils/car-brand-util';
import { LotViewModel } from 'src/app/generated-models/lot-models/lot-view-model';
import { FavoriteViewModel } from 'src/app/generated-models/favorite-view-model';

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

  public get SortViewMode(): typeof SortOrderViewModel {
    return SortOrderViewModel;
  }
  public get FilterConstants(): typeof FilterConstants {
    return FilterConstants;
  }
  public get CarBrands() {
    return CarBrandArray;
  }
  public get FavoriteConstants(): FavoriteConstants {
    return FavoriteConstants;
  }

  public str = {};
  public userId: string = '';
  public isAuth: boolean = false;
  public lots: LotViewModel[];
  public runSpinner = new BehaviorSubject<boolean>(false);
  public filterConstants: FilterConstants;
  public CarBrandMapping = CarBrands;
  public favorites: FavoriteViewModel[];
  private allSelected = false;
  @ViewChild('multiple') public multSelectBrand: MatSelect;
  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private favoriteService: FavoriteService,
    private authService: AuthService,
    private filterService: FilterService
  ) { }

  public ngOnInit() {
    this.isAuth = this.authService.isAuthenticated();
    if (this.isAuth) {
      this.userId = this.authService.getUserIdFromToken();
    }
    this.getLots();
  }

  public sortingData(sortOrder: any, sortfield: string) {
    this.filterService.sortingData(sortOrder.value, sortfield)
      .subscribe((lots) => {
        this.lots = lots;
        this.initTimerAndMarkStars(lots);
      });
  }

  public selectCarBrands(isOpened: boolean) {
    if (!isOpened) {
      this.filterService.sortByCarBrand(this.multSelectBrand.value)
        .subscribe((lots) => {
          this.lots = lots;
          this.initTimerAndMarkStars(lots);
        });
    }
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }

  public toggleAllSelection() {
    this.allSelected = !this.allSelected;
    if (this.allSelected) {
      this.multSelectBrand.options.forEach((item: MatOption) => item.select());
    } else {
      this.multSelectBrand.options.forEach((item: MatOption) => { item.deselect() });
    }
    this.multSelectBrand.close();
  }

  public changeStar(lotId: number) {
    document.getElementById(getStarId(lotId)).className == FavoriteConstants.UNSTAR ?
      this.addToFavorite(lotId) : this.removeFromFavorite(lotId);
  }

  private addToFavorite(lotId: number) {
    if (this.isAuth) {
      const favorite: FavoriteViewModel = {
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

  private checkLotIfTimerIsExpired(id: number) {
    var index = this.lots.findIndex(x => x.id == id);
    var lot = this.lots[index];

    return;//remove email sending is not active
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

  private initTimer(id: number, date: Date) {
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

  private getLots() {
    this.lotService.getAllLots()
      .pipe(tap((_) => this.runSpinner.next(true)), delay(1000))
      .subscribe((res) => {
        this.lots = res;
        this.runSpinner.next(false);
        this.initTimerAndMarkStars(res);
      }, () => {
        this.runSpinner.next(false);
      });
  }

  private initTimerAndMarkStars(lots: LotViewModel[]) {
    lots.forEach((lot) => {
      this.initTimer(lot.id, lot.startDateTime);
      if (this.isAuth) {
        this.markStars();
      }
    });
  }

}

