import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Favorite } from '../models/favorite';

@Injectable({providedIn: 'root'})
export class FavoriteService {
    apiUrl = 'https://localhost:44325/api/favorite/';
    constructor(private httpClient:HttpClient) { }

    addFavorite(favorite:Favorite){
        return this.httpClient.post(this.apiUrl,favorite);
    }

    deleteFavoriteById(id:string){
        return this.httpClient.delete(this.apiUrl+id);
    }

    getUserFavorite(userId:string){
        return this.httpClient.get<Favorite[]>(this.apiUrl+userId);
    }

    getFavoriteByUserIdAndLotId(favorite:Favorite){
        return this.httpClient.post<Favorite>(this.apiUrl+"favorite/",favorite);
    }

    deleteFavoriteByUserIdAndLotId(favorite:Favorite){
        return this.httpClient.post(this.apiUrl+"deletepost",favorite);
    }
}