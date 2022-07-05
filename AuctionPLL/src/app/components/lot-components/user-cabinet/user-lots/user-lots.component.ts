import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LotService } from 'src/app/services/lot.service';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { AuthService } from 'src/app/services/auth.service';
import { ErrorMessages } from 'src/app/common/constants/error-messages';
import { map } from 'rxjs/operators';
import { getTimerId } from 'src/app/utils/element-id.service';
import { LotViewModel } from 'src/app/generated-models/lot-models/lot-view-model';

@Component({
  selector: 'app-user-lots',
  templateUrl: './user-lots.component.html',
  styleUrls: ['./user-lots.component.less']
})
export class UserLotsComponent implements OnInit, OnDestroy {

  public str = {};
  public lots: LotViewModel[];
  public emptyText = 'List is empty =)';
  constructor(
    private toastrService: ToastrService,
    private lotService: LotService,
    private confirmationDialogService: ConfirmationDialogService,
    private authService: AuthService
  ) { }

  public ngOnInit() {
    this.init();
  }

  public ngOnDestroy() {
    this.lots.forEach((lot) => {
      clearInterval(this.str[lot.id]);
    });
  }

  public deleteLot(id: number) {
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to ... ?')
      .then((confirmed) => {
        if (confirmed) {
          this.lotService.deleteLotById(id)
            .subscribe(() => {
              this.lots = this.lots.filter(x => x.id != id);
              clearInterval(this.str[id]);
              this.toastrService.success('Lot is deleted!');
            }, () => {
              this.toastrService.error(ErrorMessages.SomethingWentWrong, ErrorMessages.TryAgain);
            });
        }
      }).catch();
  }

  public endBid(lotEnd: LotViewModel) {
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to ... ?')
      .then((confirmed) => {
        if (confirmed) {
          lotEnd.isSold = true;
          this.lotService.updateLot(lotEnd)
            .subscribe(() => {
              lotEnd.isSold = true;
              clearInterval(this.str[lotEnd.id]);
              document.getElementById(getTimerId(lotEnd.id)).innerHTML = 'Expired';
              this.toastrService.success('Lot is closed!');
            }, () => {
              this.toastrService.error(ErrorMessages.SomethingWentWrong, ErrorMessages.TryAgain);
            });
        }
      }).catch();
  }

  public createImgPath(serverPath: string) {
    return this.lotService.createImgPath(serverPath);
  }

  private initTimer(id: number, date: Date) {
    const dead = new Date(date);
    dead.setDate(dead.getDate() + 15);
    const deadline = new Date(dead).getTime();
    //deadline=new Date("Jul 2, 2021 09:44:00").getTime();
    this.str[id] = setInterval(() => {
      const now = new Date().getTime();
      const t = deadline - now;
      const days = Math.floor(t / (1000 * 60 * 60 * 24));
      const hours = Math.floor((t % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      const minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((t % (1000 * 60)) / 1000);

      document.getElementById(getTimerId(id)).innerHTML = `${days}d ${hours}h ${minutes}m ${seconds}s`;
      if (t < 0) {
        clearInterval(this.str[id]);
        this.checkLotIfTimerIsExpired(id);
        return;
      }
    }, 1000);
  }

  private checkLotIfTimerIsExpired(id: number) {
    const index = this.lots.findIndex(x => x.id == id);
    const lot = this.lots[index];

    if (lot.startPrice < lot.currentPrice) {
      this.lots = this.lots.filter(x => x.id != id);
      return;
    } else if (lot.startPrice === lot.currentPrice) {
      this.initTimer(lot.id, lot.startDateTime);
      return;
    }
  }

  private getUserId(): string {
    return this.authService.getUserIdFromToken();
  }

  private init() {
    const userId = this.getUserId();
    this.lotService.getLotsByUserId(userId)
      .pipe(map((lots) => lots.filter(lot => !lot?.isSold)))
      .subscribe((response) => {
        this.lots = response;
        response.forEach((lot) => {
          this.initTimer(lot.id, lot.startDateTime);
        });
      });
  }

}
