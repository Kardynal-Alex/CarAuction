import { Guid } from "guid-typescript";

export class Comment{
    Id:string;
    Author:string;
    Text:string;
    DateTime:Date;
    LotId:string;
    UserId:string;
    IsBid:boolean;
}