export enum CarBrand {
    Tesla = 0,
    BMW,
    Ferrari,
    Ford,
    Porsche,
    Honda,
    Toyota,
    Audi,
    Jeep,
    Lexus,
    Chevrolet,
    Mercedes,
    Volkswagen,
    Peugeot,
    Kia
};

export const CarBrands = {
    [CarBrand.Tesla]: 'Tesla',
    [CarBrand.BMW]: 'BMV',
    [CarBrand.Ferrari]: 'Ferrari',
    [CarBrand.Ford]: 'Ford',
    [CarBrand.Porsche]: 'Porshe',
    [CarBrand.Honda]: 'Honda',
    [CarBrand.Toyota]: 'Toyota',
    [CarBrand.Audi]: 'Audi',
    [CarBrand.Jeep]: 'Jeep',
    [CarBrand.Lexus]: 'Lexus',
    [CarBrand.Chevrolet]: 'Chevrolet',
    [CarBrand.Mercedes]: 'Mercedes',
    [CarBrand.Volkswagen]: 'Volkswagen',
    [CarBrand.Peugeot]: 'Peugeot',
    [CarBrand.Kia]: 'KIA'
};

export const CarBrandArray = Object.values(CarBrand).filter((value) => typeof value === 'number');