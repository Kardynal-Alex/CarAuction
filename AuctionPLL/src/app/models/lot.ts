import { User } from "./user";
import { LotState } from "./lot-state";
import { Images } from "./images";

export class Lot {
    Id: number;
    NameLot: string;
    StartPrice: number;
    IsSold: boolean;
    Image: string;
    Description: string;
    UserId: string;
    StartDateTime: Date;
    CurrentPrice: number;
    Year: number;
    User: User;
    LotState: LotState;
    Images: Images;
    constructor(id: number, nameLot: string, startPrice: number, isSold: boolean,
        image: string, description: string, userId: string, startDatetime: Date,
        currentPrice: number, year: number) {

    }
}