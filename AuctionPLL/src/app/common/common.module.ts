import { NgModule } from "@angular/core";
import { ConfirmationDialogComponent } from "./confirmation-dialog/confirmation-dialog.component";
import { ConfirmationDialogService } from "./confirmation-dialog/confirmation-dialog.service";
import { EmptyViewComponent } from "./empty-view/empty-view.component";
import { HrComponent } from "./hr/hr.component";
import { AwesomeTooltipComponent } from "./tooltip/tooltip.component";
import { AwesomeTooltipDirective } from "./tooltip/tooltip.directive";

@NgModule({
    providers: [
        ConfirmationDialogService
    ],
    imports: [
    ],
    exports: [
        HrComponent,
        EmptyViewComponent,
        ConfirmationDialogComponent
    ],
    declarations: [
        HrComponent,
        EmptyViewComponent,
        ConfirmationDialogComponent,
        AwesomeTooltipComponent,
        AwesomeTooltipDirective,
    ],
    entryComponents: [
        ConfirmationDialogComponent,
        AwesomeTooltipComponent
    ]
})
export class CommonModule { }