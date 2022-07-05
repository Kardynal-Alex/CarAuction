import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';
import { FavoriteViewModel } from '../generated-models/favorite-view-model';

@Injectable({ providedIn: 'root' })
export class FavoriteService {
    private apiUrl = `${BaseUrl.ApiURL}/favorite`;

    constructor(private httpClient: HttpClient) { }

    public addFavorite(favorite: FavoriteViewModel): Observable<Object> {
        return this.httpClient.post(`${this.apiUrl}/`, favorite);
    }

    public deleteFavoriteById(id: string): Observable<Object> {
        return this.httpClient.delete(`${this.apiUrl}/${id}`);
    }

    public getUserFavorite(userId: string): Observable<FavoriteViewModel[]> {
        return this.httpClient.get<FavoriteViewModel[]>(`${this.apiUrl}/${userId}`);
    }

    public getFavoriteByUserIdAndLotId(favorite: FavoriteViewModel): Observable<FavoriteViewModel> {
        return this.httpClient.post<FavoriteViewModel>(`${this.apiUrl}/Favorite/`, favorite);
    }

    public deleteFavoriteByUserIdAndLotId(favorite: FavoriteViewModel): Observable<Object> {
        return this.httpClient.post(`${this.apiUrl}/Delete`, favorite);
    }
}