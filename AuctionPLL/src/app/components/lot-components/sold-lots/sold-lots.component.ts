import { Component, OnDestroy, OnInit } from '@angular/core';
import { LotService } from 'src/app/services/lot.service';
import { BehaviorSubject } from 'rxjs';
import { LotViewModel } from 'src/app/generated-models/lot-models/lot-view-model';

@Component({
  selector: 'app-sold-lots',
  templateUrl: './sold-lots.component.html',
  styleUrls: [
    './sold-lots.component.less',
    '../show-lots/show-lots.component.less'
  ]
})
export class SoldLotsComponent implements OnInit, OnDestroy {

  public lots$ = new BehaviorSubject<LotViewModel[]>([]);
  constructor(private lotService: LotService) { }

  public ngOnInit() {
    this.getLots();
  }

  public ngOnDestroy(): void {
    this.lots$.complete();
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }

  private getLots() {
    this.lotService.getSoldLots()
      .subscribe((_) => this.lots$.next(_));
  }
}
