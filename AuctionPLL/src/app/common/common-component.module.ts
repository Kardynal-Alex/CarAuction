import { OverlayModule } from "@angular/cdk/overlay";
import { NgModule } from "@angular/core";
import { ConfirmationDialogComponent } from "./confirmation-dialog/confirmation-dialog.component";
import { ConfirmationDialogService } from "./confirmation-dialog/confirmation-dialog.service";
import { EmptyViewComponent } from "./empty-view/empty-view.component";
import { HrComponent } from "./hr/hr.component";
import { AwesomeTooltipComponent } from "./tooltip/tooltip.component";
import { AwesomeTooltipDirective } from "./tooltip/tooltip.directive";
import { MatSpinnerComponent } from './mat-spinner/mat-spinner.component';
import { AngularMaterialModule } from "../material.module";
import { CommonModule } from "@angular/common";
import { CommonDirectiveModule } from "./directives/common-directives.module";

@NgModule({
    providers: [
        ConfirmationDialogService
    ],
    imports: [
        CommonModule,
        OverlayModule,
        AngularMaterialModule,
        CommonDirectiveModule
    ],
    exports: [
        HrComponent,
        EmptyViewComponent,
        ConfirmationDialogComponent,
        AwesomeTooltipComponent,
        AwesomeTooltipDirective,
        MatSpinnerComponent,
        CommonDirectiveModule
    ],
    declarations: [
        HrComponent,
        EmptyViewComponent,
        ConfirmationDialogComponent,
        AwesomeTooltipComponent,
        AwesomeTooltipDirective,
        MatSpinnerComponent,
        MatSpinnerComponent
    ]
})
export class CommonComponentModule { }