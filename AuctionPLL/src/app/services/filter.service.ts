import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ComplexFilterViewModel } from '../generated-models/filter/complex-filter-view-model';
import { PageRequestViewModel } from '../generated-models/filter/page-request-view-model';
import { SortOrderViewModel } from '../generated-models/filter/sort-order-view-model';
import { LotViewModel } from '../generated-models/lot-models/lot-view-model';
import { LotService } from './lot.service';

@Injectable({ providedIn: 'root' })
export class FilterService {
    private filter: PageRequestViewModel;
    constructor(private lotService: LotService) {
        this.filter = {
            carBrand: [], complexFilter: []
        };
    }

    public sortingData(sortOrder: SortOrderViewModel, sortfield: string): Observable<LotViewModel[]> {
        if (sortOrder == null) {
            this.removeFilter(sortfield);
        } else if (this.filter.complexFilter.filter((_) => _.field === sortfield).length === 0) {
            this.addFilter(sortOrder, sortfield);
        } else {
            this.changeFilterSortOrder(sortOrder, sortfield);
        }
        return this.lotService.fetchFiltered(this.filter);
    }

    public sortByCarBrand(carBrand: any[]): Observable<LotViewModel[]> {
        this.filter.carBrand = carBrand;
        return this.lotService.fetchFiltered(this.filter);
    }

    private addFilter(sortOrder: SortOrderViewModel, sortfield: string) {
        this.filter.complexFilter.push({
            field: sortfield,
            sortOrder: sortOrder
        } as ComplexFilterViewModel);
    }
    private removeFilter(sortfield: string) {
        this.filter.complexFilter = this.filter.complexFilter.filter(((_) => _.field !== sortfield));
    }
    private changeFilterSortOrder(sortOrder: SortOrderViewModel, sortfield: string) {
        var index = this.filter.complexFilter.findIndex((_) => _.field === sortfield);
        this.filter.complexFilter[index].sortOrder = sortOrder;
    }
}