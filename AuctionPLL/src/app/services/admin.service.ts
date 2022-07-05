import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';
import { UserViewModel } from '../generated-models/auth-models/user-view-model';

@Injectable({ providedIn: 'root' })
export class AdminService {
    private apiUrl = `${BaseUrl.ApiURL}/admin`;

    constructor(
        private httpClient: HttpClient
    ) { }

    public getUsersWithRoleUser(): Observable<UserViewModel[]> {
        return this.httpClient.get<UserViewModel[]>(`${this.apiUrl}/Users/`);
    }

    public deleteUser(userId: string): Observable<Object> {
        return this.httpClient.delete(`${this.apiUrl}/${userId}`);
    }
}