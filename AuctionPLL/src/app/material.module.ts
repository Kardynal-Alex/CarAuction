import { NgModule } from "@angular/core";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@NgModule({
    exports: [
        MatProgressSpinnerModule,
        MatTooltipModule,
        MatMenuModule,
        MatInputModule,
        MatSelectModule
    ],
    imports: [
        MatInputModule,
        MatSelectModule
    ]
})
export class AngularMaterialModule { }