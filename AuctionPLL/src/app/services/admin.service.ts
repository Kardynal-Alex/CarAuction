import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../services/local-storage.service';
import { User } from '../models/user';

@Injectable({providedIn: 'root'})
export class AdminService {
    apiUrl = 'https://localhost:44325/api/admin/';
    constructor(private httpClient:HttpClient,
                private localStorage:LocalStorageService) { }

    getUsersWithRoleUser(){
        return this.httpClient.get<User[]>(this.apiUrl+"users/");
    }

    deleteUser(userId:string){
        return this.httpClient.delete(this.apiUrl+userId);
    }
}