import { CarBrand } from '../lot-models/car-brand';
import { ComplexFilter } from './complex-filter';

export class PageRequest {
    carBrand: CarBrand[];
    complexFilter: ComplexFilter[];
}