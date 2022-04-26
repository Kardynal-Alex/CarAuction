import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Lot } from 'src/app/models/lot-models/lot';
import { LotService } from 'src/app/services/lot.service';

@Component({
  selector: 'app-fresh-lots',
  templateUrl: './fresh-lots.component.html',
  styleUrls: ['./fresh-lots.component.less']
})
export class FreshLotsComponent implements OnInit {

  constructor(
    private lotService: LotService,
    private router: Router
  ) { }

  @Input() lot: Lot;
  public lots: Lot[];
  public ngOnInit() {
    this.getFreshLots();
  }

  public getFreshLots() {
    this.lotService.getFreshLots()
      .pipe(tap(lots => this.lots = lots))
      .subscribe();
  }

  public createImgPath(serverPath: string): string {
    return this.lotService.createImgPath(serverPath);
  }

  public redirectToNewLot(id: number) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate(['/bid/lot/' + id]);
  }

}