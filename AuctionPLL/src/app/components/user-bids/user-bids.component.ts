import { Component, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { LotService } from 'src/app/services/lot.service';
import { tap } from "rxjs/operators";

@Component({
  selector: 'app-user-bids',
  templateUrl: './user-bids.component.html',
  styleUrls: ['./user-bids.component.css']
})
export class UserBidsComponent implements OnInit {
  public lots: Lot[];
  constructor(
    private lotService: LotService,
    private localStorage: LocalStorageService) { }

  public getUserId(): string {
    var payload = JSON.parse(window.atob(this.localStorage.get('token').split('.')[1]));
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
