import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseUrl } from "../common/constants/urls";
import { AuthorDescription } from "../models/author-description";

@Injectable({ providedIn: 'root' })
export class AuthorDescriptionService {
    private apiUrl = BaseUrl.ApiURL + 'authordescription/';
    constructor(
        private httpClient: HttpClient
    ) { }

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