import { NgModule } from "@angular/core";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { CdTimerModule } from "angular-cd-timer";

@NgModule({
    exports: [
        MatProgressSpinnerModule,
        MatTooltipModule,
        MatMenuModule,
        MatInputModule,
        MatSelectModule,
        CdTimerModule
    ],
    imports: [
        MatInputModule,
        MatSelectModule,
        CdTimerModule
    ]
})
export class AngularMaterialModule { }