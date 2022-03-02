import { Component, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { LotService } from 'src/app/services/lot.service';
import { tap } from "rxjs/operators";
import { CommonConstants } from 'src/app/common/constants/common-constants';

@Component({
  selector: 'app-user-bids',
  templateUrl: './user-bids.component.html',
  styleUrls: ['./user-bids.component.less']
})
export class UserBidsComponent implements OnInit {
  public lots: Lot[];
  constructor(
    private lotService: LotService,
    private localStorage: LocalStorageService) { }

  public getUserId(): string {
    //replace on function
    var payload = JSON.parse(window.atob(this.localStorage.get(CommonConstants.JWTToken).split('.')[1]));
    return payload.id;
  }

  public ngOnInit(): void {
    const userId = this.getUserId();
    this.lotService.getUserBids(userId)
      .pipe(tap(lots => this.lots = lots))
      .subscribe();
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }

}
