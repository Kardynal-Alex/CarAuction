import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CommonComponentModule } from "src/app/common/common-component.module";
import { AdminComponent } from "./admin/admin.component";

const appRoutes: Routes = [
    {
        path: '',
        component: AdminComponent
    }
];

@NgModule({
    imports: [
        RouterModule,
        RouterModule.forChild(appRoutes),
        CommonComponentModule,
        CommonModule
    ],
    exports: [
        RouterModule,
        AdminComponent
    ],
    providers: [],
    declarations: [
        AdminComponent
    ]
})
export class AdminModule { }