import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Comment } from '../models/comment';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';

@Injectable({ providedIn: 'root' })
export class CommentService {
    private apiUrl = `${BaseUrl.ApiURL}/comment`;

    constructor(private httpClient: HttpClient) { }

    public addComment(comment: Comment): Observable<Object> {
        return this.httpClient.post(`${this.apiUrl}/`, comment);
    }

    public getCommentsByLotId(lotId: number): Observable<Comment[]> {
        return this.httpClient.get<Comment[]>(`${this.apiUrl}/${lotId}`);
    }

    public deleteCommentById(commentId: string): Observable<Object> {
        return this.httpClient.delete(`${this.apiUrl}/${commentId}`);
    }
}