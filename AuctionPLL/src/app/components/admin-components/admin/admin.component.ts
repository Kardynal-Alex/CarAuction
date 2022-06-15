import { Component, OnDestroy, OnInit } from '@angular/core';
import { User } from 'src/app/models/auth-models/user';
import { AdminService } from '../../../services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { ErrorMessages } from 'src/app/common/constants/error-messages';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.less']
})
export class AdminComponent implements OnInit, OnDestroy {
  public users$ = new BehaviorSubject<User[]>([]);
  constructor(
    private adminService: AdminService,
    private toastrService: ToastrService,
    private confirmationDialogService: ConfirmationDialogService
  ) { }

  public getUsers() {
    this.adminService.getUsersWithRoleUser()
      .subscribe((_) => this.users$.next(_));
  }

  public ngOnInit(): void {
    this.getUsers();
  }

  public ngOnDestroy(): void {
    this.users$.complete();
  }

  public deleteUser(userId: string) {
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to ... ?')
      .pipe(
        filter((confirmed) => confirmed),
        map(() => {
          this.adminService.deleteUser(userId)
            .subscribe((_) => {
              this.toastrService.success('User is deleted!');
              this.getUsers();
            }, (_) => {
              this.toastrService.error(ErrorMessages.Error);
            });
        })
      );
  }
}
