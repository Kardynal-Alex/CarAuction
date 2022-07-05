/* eslint-disable */
/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { CarBrandViewModel } from './car-brand-view-model';
import { UserViewModel } from '../auth-models/user-view-model';
import { LotStateViewModel } from './lot-state-view-model';
import { ImagesViewModel } from './images-view-model';

export class LotViewModel {
    id: number;
    nameLot: string;
    startPrice: number;
    isSold: boolean;
    image: string;
    description: string;
    carBrand: CarBrandViewModel;
    user: UserViewModel;
    userId: string;
    startDateTime: Date;
    currentPrice: number;
    year: number;
    lotState: LotStateViewModel;
    images: ImagesViewModel;
}
