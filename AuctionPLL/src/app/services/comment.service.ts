import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Comment } from '../models/comment';

@Injectable({providedIn: 'root'})
export class CommentService {
    apiUrl = 'https://localhost:44325/api/comment/';
    constructor(private httpClient:HttpClient) { }

    addComment(comment:Comment){
        return this.httpClient.post(this.apiUrl,comment);
    }

    getCommentsByLotId(lotId:number){
        return this.httpClient.get<Comment[]>(this.apiUrl+lotId);
    }

    deleteVommentById(commentId:string){
        return this.httpClient.delete(this.apiUrl+commentId);
    }
}