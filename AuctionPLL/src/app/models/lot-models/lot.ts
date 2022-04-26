import { User } from '../auth-models/user';
import { LotState } from './lot-state';
import { Images } from './images';

export class Lot {
    id: number;
    nameLot: string;
    startPrice: number;
    isSold: boolean;
    image: string;
    description: string;
    userId: string;
    startDateTime: Date;
    currentPrice: number;
    year: number;
    user: User;
    lotState: LotState;
    images: Images;
    constructor(
        id: number, nameLot: string, startPrice: number, isSold: boolean,
        image: string, description: string, userId: string, startDatetime: Date,
        currentPrice: number, year: number
    ) {

    }
}