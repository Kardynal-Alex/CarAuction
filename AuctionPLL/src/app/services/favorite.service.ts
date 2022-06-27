import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Favorite } from '../models/favorite';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';

@Injectable({ providedIn: 'root' })
export class FavoriteService {
    private apiUrl = `${BaseUrl.ApiURL}/favorite`;

    constructor(private httpClient: HttpClient) { }

    public addFavorite(favorite: Favorite): Observable<Object> {
        return this.httpClient.post(`${this.apiUrl}/`, favorite);
    }

    public deleteFavoriteById(id: string): Observable<Object> {
        return this.httpClient.delete(`${this.apiUrl}/${id}`);
    }

    public getUserFavorite(userId: string): Observable<Favorite[]> {
        return this.httpClient.get<Favorite[]>(`${this.apiUrl}/${userId}`);
    }

    public getFavoriteByUserIdAndLotId(favorite: Favorite): Observable<Favorite> {
        return this.httpClient.post<Favorite>(`${this.apiUrl}/Favorite/`, favorite);
    }

    public deleteFavoriteByUserIdAndLotId(favorite: Favorite): Observable<Object> {
        return this.httpClient.post(`${this.apiUrl}/Delete`, favorite);
    }
}