import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { FavoriteService } from 'src/app/services/favorite.service';
import { LotService } from 'src/app/services/lot.service';
import { Guid } from 'guid-typescript';
import { BehaviorSubject } from 'rxjs';
import { LotViewModel } from 'src/app/generated-models/lot-models/lot-view-model';
import { FavoriteViewModel } from 'src/app/generated-models/favorite-view-model';

@Component({
  selector: 'app-favorite-lot',
  templateUrl: './favorite-lot.component.html',
  styleUrls: [
    './favorite-lot.component.less',
    '../../show-lots/show-lots.component.less'
  ]
})
export class FavoriteLotComponent implements OnInit, OnDestroy {

  public lots$ = new BehaviorSubject<LotViewModel[]>([]);
  public userId: string;
  constructor(
    private lotService: LotService,
    private favoriteService: FavoriteService,
    private authService: AuthService
  ) { }

  public ngOnInit(): void {
    this.userId = this.authService.getUserIdFromToken();
    this.getLots(this.userId);
  }

  public ngOnDestroy(): void {
    this.lots$.complete();
  }

  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }

  public removeFromFavorite(lotId: number) {
    const favorite: FavoriteViewModel = {
      id: Guid.create().toString(),
      userId: this.userId,
      lotId: lotId
    };
    this.favoriteService.deleteFavoriteByUserIdAndLotId(favorite)
      .subscribe((_) => {
        this.lots$.next(this.lots$.value.filter((x) => x.id != lotId));
      });
  }

  private getLots(userId: string) {
    this.lotService.getFavoriteUsersLots(userId)
      .subscribe((_) => this.lots$.next(_));
  }

}
