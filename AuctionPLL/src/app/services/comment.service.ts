import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';
import { CommentViewModel } from '../generated-models/comment-view-model';

@Injectable({ providedIn: 'root' })
export class CommentService {
    private apiUrl = `${BaseUrl.ApiURL}/comment`;

    constructor(private httpClient: HttpClient) { }

    public addComment(comment: CommentViewModel): Observable<Object> {
        return this.httpClient.post(`${this.apiUrl}/`, comment);
    }

    public getCommentsByLotId(lotId: number): Observable<CommentViewModel[]> {
        return this.httpClient.get<CommentViewModel[]>(`${this.apiUrl}/${lotId}`);
    }

    public deleteCommentById(commentId: string): Observable<Object> {
        return this.httpClient.delete(`${this.apiUrl}/${commentId}`);
    }
}