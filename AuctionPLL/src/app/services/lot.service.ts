import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Lot } from '../models/lot';
import { AskOwner } from '../models/ask-owner';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';
import { AuthorDescription } from '../models/author-description';

@Injectable({ providedIn: 'root' })
export class LotService {
    private apiUrl = BaseUrl.ApiURL + 'lot/';
    private uploadApiPhoto = BaseUrl.ApiURL + 'upload/';
    constructor(private httpClient: HttpClient) { }
    numbersOfImages: number = 9;

    public createLot(lot: Lot): Observable<Object> {
        return this.httpClient.post(this.apiUrl, lot);
    }

    public getAllLots(): Observable<Lot[]> {
        return this.httpClient.get<Lot[]>(this.apiUrl);
    }

    public getLotsByUserId(userId: string): Observable<Lot[]> {
        return this.httpClient.get<Lot[]>(this.apiUrl + 'getuserlots/' + userId);
    }

    public deleteLotById(lotId: number): Observable<Object> {
        return this.httpClient.delete(this.apiUrl + lotId);
    }

    public getLotById(lotId: number): Observable<Lot> {
        return this.httpClient.get<Lot>(this.apiUrl + lotId);
    }

    public getFreshLots(): Observable<Lot[]> {
        return this.httpClient.get<Lot[]>(this.apiUrl + 'getfreshlots/');
    }

    public getFavoriteUsersLots(userId: string): Observable<Lot[]> {
        return this.httpClient.get<Lot[]>(this.apiUrl + 'favorites/' + userId);
    }

    public getUserBids(id: string): Observable<Lot[]> {
        return this.httpClient.get<Lot[]>(this.apiUrl + 'userbids/' + id);
    }

    public getSoldLots(): Observable<Lot[]> {
        return this.httpClient.get<Lot[]>(this.apiUrl + 'getsoldlots/');
    }

    public updateLot(lot: Lot): Observable<Object> {
        return this.httpClient.put(this.apiUrl, lot);
    }

    public updateLotAfterClosing(lot: Lot): Observable<Object> {
        return this.httpClient.put(this.apiUrl + 'closebid/', lot);
    }

    public updateOnlyDateLot(lot: Lot): Observable<Object> {
        return this.httpClient.put(this.apiUrl + 'onlydatelot/', lot);
    }

    public deletePhoto(path: string): Observable<Object> {
        return this.httpClient.delete(this.uploadApiPhoto + '?path=' + path);
    }

    public askOwner(askOwner: AskOwner): Observable<Object> {
        return this.httpClient.post(this.apiUrl + 'askowner/', askOwner);
    }

    public createImgPath(serverPath: string): string {
        return `https://localhost:44325/${serverPath}`;
    }

    public addAuthorDescription(authorDescription: AuthorDescription): Observable<Object> {
        return this.httpClient.post(this.apiUrl + 'addAuthorDescription/', authorDescription);
    }

    public updateAuthorDescription(authorDescription: AuthorDescription): Observable<Object> {
        return this.httpClient.put(this.apiUrl + 'updateAuthorDescription/', authorDescription);
    }

    public getAuthorDescriptionByLotId(id: number): Observable<AuthorDescription> {
        return this.httpClient.get<AuthorDescription>(this.apiUrl + 'getAuthorDescriptionByLotId/' + id);
    }
}