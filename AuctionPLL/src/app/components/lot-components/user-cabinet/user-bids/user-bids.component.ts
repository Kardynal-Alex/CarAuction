import { Component, OnDestroy, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot-models/lot';
import { LotService } from 'src/app/services/lot.service';
import { AuthService } from 'src/app/services/auth.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-user-bids',
  templateUrl: './user-bids.component.html',
  styleUrls: ['./user-bids.component.less']
})
export class UserBidsComponent implements OnInit, OnDestroy {

  public lots$ = new BehaviorSubject<Lot[]>([]);
  constructor(
    private lotService: LotService,
    private authService: AuthService
  ) { }

  public ngOnInit(): void {
    const userId = this.getUserId();
    this.lotService.getUserBids(userId)
      .subscribe((_) => this.lots$.next(_));
  }

  public ngOnDestroy(): void {
    this.lots$.complete();
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }

  private getUserId(): string {
    return this.authService.getUserIdFromToken();
  }

}
