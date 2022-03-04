import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseUrl } from '../common/constants/urls';
import { Images } from '../models/images';

@Injectable({ providedIn: 'root' })
export class ImagesService {
    private apiUrl = BaseUrl.ApiURL + 'lot/';
    private uploadApiPhoto = BaseUrl.ApiURL + 'upload/';
    constructor(private httpClient: HttpClient) { }

    public uploadImages(files: any) {
        let uploadApiPhoto = BaseUrl.ApiURL + 'upload';
        let fileToUpload = files.target.files[0];
        let formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        return this.httpClient.post(uploadApiPhoto, formData, { reportProgress: true, observe: 'events' });
    }

    public uploadMainImage(files: any) {
        let uploadApiPhoto = BaseUrl.ApiURL + 'upload';
        let fileToUpload = <File>files[0];
        let formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        return this.httpClient.post(uploadApiPhoto, formData, { reportProgress: true, observe: 'events' });
    }
}