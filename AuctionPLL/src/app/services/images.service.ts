import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseUrl } from '../common/constants/urls';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ImagesService {
    private uploadApiPhoto = `${BaseUrl.ApiURL}/upload`;

    constructor(private httpClient: HttpClient) { }

    public uploadImages(files: any): Observable<any> {
        let fileToUpload = files.target.files[0];
        let formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        return this.httpClient.post(this.uploadApiPhoto, formData, { reportProgress: true, observe: 'events' });
    }

    public uploadMainImage(files: any): Observable<any> {
        let fileToUpload = <File>files[0];
        let formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        return this.httpClient.post(this.uploadApiPhoto, formData, { reportProgress: true, observe: 'events' });
    }
}