import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/auth-models/user';
import { AdminService } from '../../../services/admin.service';
import { tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.less']
})
export class AdminComponent implements OnInit {
  public users: User[];
  constructor(
    private adminService: AdminService,
    private toastrService: ToastrService) { }

  public getUsers() {
    this.adminService.getUsersWithRoleUser()
      .pipe(tap(users => this.users = users))
      .subscribe();
  }

  public ngOnInit(): void {
    this.getUsers();
  }

  public deleteUser(userId: string) {
    this.adminService.deleteUser(userId)
      .subscribe(_ => {
        this.toastrService.success('User is deleted!');
        this.getUsers();
      }, _ => {
        this.toastrService.error('Error!');
      });
  }
}
