import { NgModule } from "@angular/core";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';

@NgModule({
    exports: [
        MatProgressSpinnerModule,
        MatTooltipModule,
        MatMenuModule
    ]
})
export class AngularMaterialModule { }