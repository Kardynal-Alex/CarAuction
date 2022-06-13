import { Component, OnDestroy, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot-models/lot';
import { LotService } from 'src/app/services/lot.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-sold-lots',
  templateUrl: './sold-lots.component.html',
  styleUrls: [
    './sold-lots.component.less',
    '../show-lots/show-lots.component.less'
  ]
})
export class SoldLotsComponent implements OnInit, OnDestroy {

  constructor(private lotService: LotService) { }

  public lots$ = new BehaviorSubject<Lot[]>([]);
  public ngOnInit() {
    this.getLots();
  }

  public ngOnDestroy(): void {
    this.lots$.complete();
  }

  public getLots() {
    this.lotService.getSoldLots()
      .subscribe((_) => this.lots$.next(_));
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }
}
