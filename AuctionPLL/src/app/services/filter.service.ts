import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ComplexFilter } from '../models/filter/complex-filter';
import { PageRequest } from '../models/filter/page-request';
import { SortOrder } from '../models/filter/sort-order';
import { Lot } from '../models/lot-models/lot';
import { LotService } from './lot.service';

@Injectable({ providedIn: 'root' })
export class FilterService {
    private filter: PageRequest;
    constructor(private lotService: LotService) {
        this.filter = {
            carBrand: [], complexFilter: []
        };
    }

    public sortingData(sortOrder: SortOrder, sortfield: string): Observable<Lot[]> {
        if (sortOrder == null) {
            this.removeFilter(sortfield);
        } else if (this.filter.complexFilter.filter((_) => _.field === sortfield).length === 0) {
            this.addFilter(sortOrder, sortfield);
        } else {
            this.changeFilterSortOrder(sortOrder, sortfield);
        }
        return this.lotService.fetchFiltered(this.filter);
    }

    public sortByCarBrand(carBrand: any[]): Observable<Lot[]> {
        this.filter.carBrand = carBrand;
        return this.lotService.fetchFiltered(this.filter);
    }

    private addFilter(sortOrder: SortOrder, sortfield: string) {
        this.filter.complexFilter.push({
            field: sortfield,
            sortOrder: sortOrder
        } as ComplexFilter);
    }
    private removeFilter(sortfield: string) {
        this.filter.complexFilter = this.filter.complexFilter.filter(((_) => _.field !== sortfield));
    }
    private changeFilterSortOrder(sortOrder: SortOrder, sortfield: string) {
        var index = this.filter.complexFilter.findIndex((_) => _.field === sortfield);
        this.filter.complexFilter[index].sortOrder = sortOrder;
    }
}