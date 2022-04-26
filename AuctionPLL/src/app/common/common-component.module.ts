import { OverlayModule } from "@angular/cdk/overlay";
import { NgModule } from "@angular/core";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
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
        BrowserAnimationsModule,
        OverlayModule,
    ],
    exports: [
        HrComponent,
        EmptyViewComponent,
        ConfirmationDialogComponent,
        AwesomeTooltipComponent,
        AwesomeTooltipDirective
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
export class CommonComponentModule { }