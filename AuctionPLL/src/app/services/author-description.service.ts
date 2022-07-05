import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';
import { AuthorDescriptionViewModel } from '../generated-models/lot-models/author-description-view-model';

@Injectable({ providedIn: 'root' })
export class AuthorDescriptionService {
    private apiUrl = `${BaseUrl.ApiURL}/authordescription`;

    constructor(
        private httpClient: HttpClient
    ) { }

    public addAuthorDescription(authorDescription: AuthorDescriptionViewModel): Observable<Object> {
        return this.httpClient.post(`${this.apiUrl}/`, authorDescription);
    }

    public updateAuthorDescription(authorDescription: AuthorDescriptionViewModel): Observable<Object> {
        return this.httpClient.put(`${this.apiUrl}/`, authorDescription);
    }

    public getAuthorDescriptionByLotId(id: number): Observable<AuthorDescriptionViewModel> {
        return this.httpClient.get<AuthorDescriptionViewModel>(`${this.apiUrl}/GetAuthorDescriptionByLotId/${id}`);
    }
}