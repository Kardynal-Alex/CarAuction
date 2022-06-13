import { Component, OnDestroy, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot-models/lot';
import { AuthService } from 'src/app/services/auth.service';
import { FavoriteService } from 'src/app/services/favorite.service';
import { LotService } from 'src/app/services/lot.service';
import { Favorite } from 'src/app/models/favorite';
import { Guid } from 'guid-typescript';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-favorite-lot',
  templateUrl: './favorite-lot.component.html',
  styleUrls: [
    './favorite-lot.component.less',
    '../../show-lots/show-lots.component.less'
  ]
})
export class FavoriteLotComponent implements OnInit, OnDestroy {

  constructor(
    private lotService: LotService,
    private favoriteService: FavoriteService,
    private authService: AuthService
  ) { }

  public lots$ = new BehaviorSubject<Lot[]>([]);
  public userId: string;
  public ngOnInit(): void {
    this.userId = this.authService.getUserIdFromToken();
    this.getLots(this.userId);
  }

  public ngOnDestroy(): void {
    this.lots$.complete();
  }

  getLots(userId: string) {
    this.lotService.getFavoriteUsersLots(userId)
      .subscribe((_) => this.lots$.next(_));
  }

  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }

  public removeFromFavorite(lotId: number) {
    const favorite: Favorite = {
      id: Guid.create().toString(),
      userId: this.userId,
      lotId: lotId
    };
    this.favoriteService.deleteFavoriteByUserIdAndLotId(favorite)
      .subscribe((_) => {
        this.lots$.next(this.lots$.value.filter((x) => x.id != lotId));
      });
  }

}
