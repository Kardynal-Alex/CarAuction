import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';
import { PageRequestViewModel } from '../generated-models/filter/page-request-view-model';
import { AskOwnerViewModel } from '../generated-models/lot-models/ask-owner-view-model';
import { LotViewModel } from '../generated-models/lot-models/lot-view-model';

@Injectable({ providedIn: 'root' })
export class LotService {
    private apiUrl = `${BaseUrl.ApiURL}/lot`;
    private uploadApiPhoto = `${BaseUrl.ApiURL}/upload`;

    constructor(private httpClient: HttpClient) { }
    numbersOfImages: number = 9;

    public createLot(lot: LotViewModel): Observable<Object> {
        return this.httpClient.post(`${this.apiUrl}`, lot);
    }

    public getAllLots(): Observable<LotViewModel[]> {
        return this.httpClient.get<LotViewModel[]>(`${this.apiUrl}`);
    }

    public getLotsByUserId(userId: string): Observable<LotViewModel[]> {
        return this.httpClient.get<LotViewModel[]>(`${this.apiUrl}/GetUserLots/${userId}`);
    }

    public deleteLotById(lotId: number): Observable<Object> {
        return this.httpClient.delete(`${this.apiUrl}/${lotId}`);
    }

    public getLotById(lotId: number): Observable<LotViewModel> {
        return this.httpClient.get<LotViewModel>(`${this.apiUrl}/${lotId}`);
    }

    public getFreshLots(): Observable<LotViewModel[]> {
        return this.httpClient.get<LotViewModel[]>(`${this.apiUrl}/GetFreshLots/`);
    }

    public getFavoriteUsersLots(userId: string): Observable<LotViewModel[]> {
        return this.httpClient.get<LotViewModel[]>(`${this.apiUrl}/Favorites/${userId}`);
    }

    public getUserBids(id: string): Observable<LotViewModel[]> {
        return this.httpClient.get<LotViewModel[]>(`${this.apiUrl}/UserBids/${id}`);
    }

    public getSoldLots(): Observable<LotViewModel[]> {
        return this.httpClient.get<LotViewModel[]>(`${this.apiUrl}/GetSoldLots/`);
    }

    public fetchFiltered(filter: PageRequestViewModel): Observable<LotViewModel[]> {
        return this.httpClient.post<LotViewModel[]>(`${this.apiUrl}/FetchFiltered`, filter);
    }

    public updateLot(lot: LotViewModel): Observable<Object> {
        return this.httpClient.put(`${this.apiUrl}/`, lot);
    }

    public updateLotAfterClosing(lot: LotViewModel): Observable<Object> {
        return this.httpClient.put(`${this.apiUrl}/CloseBid/`, lot);
    }

    public updateOnlyDateLot(lot: LotViewModel): Observable<Object> {
        return this.httpClient.put(`${this.apiUrl}/UpdateDateLot/`, lot);
    }

    public deletePhoto(path: string): Observable<Object> {
        return this.httpClient.delete(`${this.uploadApiPhoto}/?path=${path}`);
    }

    public askOwner(askOwner: AskOwnerViewModel): Observable<Object> {
        return this.httpClient.post(`${this.apiUrl}/AskOwner/`, askOwner);
    }

    public createImgPath(serverPath: string): string {
        return `https://localhost:44325/${serverPath}`;
    }
}