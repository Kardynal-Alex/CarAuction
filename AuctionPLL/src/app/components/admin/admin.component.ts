import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { AdminService } from '../../services/admin.service';
import { tap } from "rxjs/operators";
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  users:User[];
  constructor(private adminService:AdminService,
    private toastrService:ToastrService) { }

    getUsers(){
      this.adminService.getUsersWithRoleUser().pipe(tap(users=>this.users=users)).subscribe();
    }
  
  ngOnInit(): void {
   this.getUsers();
  }
  
  deleteUser(userId:string){
    this.adminService.deleteUser(userId).subscribe(response=>{
      this.toastrService.success("User is deleted!");
      this.getUsers();
    },
    error=>{
      this.toastrService.error("Error!");
    });
  }
}
