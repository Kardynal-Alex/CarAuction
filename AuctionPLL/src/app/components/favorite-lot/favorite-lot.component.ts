import { Component, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot';
import { AuthService } from 'src/app/services/auth.service';
import { FavoriteService } from 'src/app/services/favorite.service';
import { LotService } from 'src/app/services/lot.service';
import { tap } from "rxjs/operators";
import { Favorite } from 'src/app/models/favorite';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-favorite-lot',
  templateUrl: './favorite-lot.component.html',
  styleUrls: ['./favorite-lot.component.less']
})
export class FavoriteLotComponent implements OnInit {

  constructor(private lotService: LotService,
    private favoriteService: FavoriteService,
    private authService: AuthService) { }


  public lots: Lot[];
  userId: string;
  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    this.getLots(this.userId);
  }

  getLots(userId: string) {
    this.lotService.getFavoriteUsersLots(userId).pipe(tap(lots => this.lots = lots)).subscribe();
  }

  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }

  public removeFromFavorite(lotId: number) {
    const favorite: Favorite = {
      id: Guid.create().toString(),
      userId: this.userId,
      lotId: lotId
    }
    this.favoriteService.deleteFavoriteByUserIdAndLotId(favorite).subscribe(() => {
      this.lots = this.lots.filter(x => x.id != lotId);
    });
  }

}
