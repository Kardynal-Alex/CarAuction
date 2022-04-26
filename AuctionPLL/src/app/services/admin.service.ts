import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../services/local-storage.service';
import { User } from '../models/auth-models/user';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';

@Injectable({ providedIn: 'root' })
export class AdminService {
    private apiUrl = BaseUrl.ApiURL + 'admin/';
    constructor(
        private httpClient: HttpClient,
        private localStorage: LocalStorageService
    ) { }

    public getUsersWithRoleUser(): Observable<User[]> {
        return this.httpClient.get<User[]>(this.apiUrl + 'users/');
    }

    public deleteUser(userId: string) {
        return this.httpClient.delete(this.apiUrl + userId);
    }
}