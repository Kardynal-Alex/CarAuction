import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { BoldDirective } from "./bold.directive";

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        BoldDirective
    ],
    exports: [
        BoldDirective
    ]
})
export class CommonDirectiveModule { }