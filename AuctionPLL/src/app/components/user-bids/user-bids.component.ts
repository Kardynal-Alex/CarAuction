import { Component, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot';
import { LotService } from 'src/app/services/lot.service';
import { tap } from "rxjs/operators";
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-bids',
  templateUrl: './user-bids.component.html',
  styleUrls: ['./user-bids.component.less']
})
export class UserBidsComponent implements OnInit {
  public lots: Lot[];
  constructor(
    private lotService: LotService,
    private authService: AuthService
  ) { }

  public getUserId(): string {
    return this.authService.getUserId();
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
