import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Lot } from '../models/lot';
import { AskOwner } from '../models/ask-owner';

@Injectable({providedIn: 'root'})
export class LotService {
    apiUrl = 'https://localhost:44325/api/lot/';
    uploadApiPhoto='https://localhost:44325/api/upload/';
    constructor(private httpClient:HttpClient) { }
    numbersOfImages:number=9;

    createLot(lot:Lot){
        return this.httpClient.post(this.apiUrl,lot);
    }

    getAllLots(){
        return this.httpClient.get<Lot[]>(this.apiUrl);
    }

    getLotsByUserId(userId:string){
        return this.httpClient.get<Lot[]>(this.apiUrl+"getuserlots/"+userId);
    }

    deleteLotById(lotId:number){
        return this.httpClient.delete(this.apiUrl+lotId);
    }

    getLotById(lotId:number){
        return this.httpClient.get<Lot>(this.apiUrl+lotId);
    }

    getFreshLots(){
        return this.httpClient.get<Lot[]>(this.apiUrl+"getfreshlots/");
    }

    getFavoriteUsersLots(userId:string){
        return this.httpClient.get<Lot[]>(this.apiUrl+"favorites/"+userId);
    }

    getUserBids(id:string){
        return this.httpClient.get<Lot[]>(this.apiUrl+"userbids/"+id);
    }

    getSoldLots(){
        return this.httpClient.get<Lot[]>(this.apiUrl+"getsoldlots/");
    }

    updateLot(lot:Lot){
        return this.httpClient.put(this.apiUrl,lot);
    }

    updateLotAfterClosing(lot:Lot){
        return this.httpClient.put(this.apiUrl+"closebid/",lot);
    }

    updateOnlyDateLot(lot:Lot){
        return this.httpClient.put(this.apiUrl+"onlydatelot/",lot);
    }

    deletePhoto(path:string){
        return this.httpClient.delete(this.uploadApiPhoto+"?path="+path);
    }

    askOwner(askOwner:AskOwner){
        return this.httpClient.post(this.apiUrl+"askowner/",askOwner);
    }

    createImgPath(serverPath: string){
        return `https://localhost:44325/${serverPath}`;
      }
}