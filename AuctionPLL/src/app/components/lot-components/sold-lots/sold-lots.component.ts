import { Component, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot';
import { LotService } from 'src/app/services/lot.service';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-sold-lots',
  templateUrl: './sold-lots.component.html',
  styleUrls: [
    './sold-lots.component.less',
    '../show-lots/show-lots.component.less'
  ]
})
export class SoldLotsComponent implements OnInit {

  constructor(private lotService: LotService) { }

  public lots: Lot[];
  public ngOnInit() {
    this.getLots();
  }

  public getLots() {
    this.lotService.getSoldLots()
      .pipe(tap(lots => this.lots = lots))
      .subscribe();
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }
}
