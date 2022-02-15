import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Lot } from 'src/app/models/lot';
import { LotService } from 'src/app/services/lot.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { CommonConstants } from 'src/app/common/common-constants';

@Component({
  selector: 'app-user-lots',
  templateUrl: './user-lots.component.html',
  styleUrls: ['./user-lots.component.less']
})
export class UserLotsComponent implements OnInit, OnDestroy {
  public lots: Lot[];
  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private localStorage: LocalStorageService,
    private confirmationDialogService: ConfirmationDialogService
  ) { }

  public getUserId(): string {
    //replace on function
    var payload = JSON.parse(window.atob(this.localStorage.get(CommonConstants.JWTToken).split('.')[1]));
    return payload.id;
  }

  public init() {
    const userId = this.getUserId();
    this.lotService.getLotsByUserId(userId)
      .subscribe(response => {
        this.lots = response;
        for (let lot of response) {
          this.initTimer(lot['id'], lot['startDateTime']);
        }
      });
  }

  public ngOnInit() {
    this.init();
  }

  public ngOnDestroy() {
    for (let lot of this.lots) {
      clearInterval(this.str[lot['id']]);
    }
  }

  public deleteLot(id: number) {
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to ... ?')
      .then((confirmed) => {
        if (confirmed) {
          this.lotService.deleteLotById(id)
            .subscribe(_ => {
              this.lots = this.lots.filter(x => x['id'] != id);
              clearInterval(this.str[id]);
              this.toastrService.success("Lot is deleted!");
            }, _ => {
              this.toastrService.error("Something went wrong!");
            });
        }
      })
      .catch();
  }

  public endBid(lotEnd: Lot) {
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to ... ?')
      .then((confirmed) => {
        if (confirmed) {
          lotEnd.IsSold = true;
          this.lotService.updateLot(lotEnd)
            .subscribe(_ => {
              lotEnd['isSold'] = true;
              clearInterval(this.str[lotEnd['id']]);
              document.getElementById('demo-' + lotEnd['id']).innerHTML = "Expired";
              this.toastrService.success("Lot is closed!");
            }, _ => {
              this.toastrService.error("Something went wrong!");
            });
        }
      })
      .catch();
  }

  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }

  public str = {};
  public initTimer(id: number, date: Date) {
    var dead = new Date(date);
    dead.setDate(dead.getDate() + 15);
    var deadline = new Date(dead).getTime();
    //deadline=new Date("Jul 2, 2021 09:44:00").getTime();
    this.str[id] = setInterval(() => {
      var now = new Date().getTime();
      var t = deadline - now;
      var days = Math.floor(t / (1000 * 60 * 60 * 24));
      var hours = Math.floor((t % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      var minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60));
      var seconds = Math.floor((t % (1000 * 60)) / 1000);

      document.getElementById('demo-' + id).innerHTML = days + "d " + hours + "h " + minutes + "m " + seconds + "s ";
      if (t < 0) {
        clearInterval(this.str[id]);
        this.checkLotIfTimerIsExpired(id);
        return;
      }
    }, 1000);
  }

  public checkLotIfTimerIsExpired(id: number) {
    var index = this.lots.findIndex(x => x['id'] == id);
    var lot = this.lots[index];

    if (parseFloat(lot['startPrice']) < parseFloat(lot['currentPrice'])) {
      this.lots = this.lots.filter(x => x['id'] != id);
      return;
    } else if (parseFloat(lot['startPrice']) === parseFloat(lot['currentPrice'])) {
      this.initTimer(lot['id'], lot['startDateTime']);
      return;
    }
  }
}
