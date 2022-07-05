import { CarBrandViewModel } from '../generated-models/lot-models/car-brand-view-model';

export const CarBrands = {
    [CarBrandViewModel.Tesla]: 'Tesla',
    [CarBrandViewModel.BMW]: 'BMV',
    [CarBrandViewModel.Ferrari]: 'Ferrari',
    [CarBrandViewModel.Ford]: 'Ford',
    [CarBrandViewModel.Porsche]: 'Porshe',
    [CarBrandViewModel.Honda]: 'Honda',
    [CarBrandViewModel.Toyota]: 'Toyota',
    [CarBrandViewModel.Audi]: 'Audi',
    [CarBrandViewModel.Jeep]: 'Jeep',
    [CarBrandViewModel.Lexus]: 'Lexus',
    [CarBrandViewModel.Chevrolet]: 'Chevrolet',
    [CarBrandViewModel.Mercedes]: 'Mercedes',
    [CarBrandViewModel.Volkswagen]: 'Volkswagen',
    [CarBrandViewModel.Peugeot]: 'Peugeot',
    [CarBrandViewModel.Kia]: 'KIA'
};

export const CarBrandArray = Object.values(CarBrandViewModel).filter((value) => typeof value === 'number');