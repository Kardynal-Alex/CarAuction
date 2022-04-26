import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { CommonComponentModule } from "src/app/common/common-component.module";
import { LoginGuard } from "src/app/guards/login.guard";
import { FavoriteLotComponent } from "./favorite-lot/favorite-lot.component";
import { UserBidsComponent } from "./user-bids/user-bids.component";
import { UserLotsComponent } from "./user-lots/user-lots.component";

const appRoutes: Routes = [
    {
        path: 'favorites',
        component: FavoriteLotComponent
    },
    {
        path: 'userbids',
        component: UserBidsComponent,
        canActivate: [LoginGuard]
    },
    {
        path: 'userlots',
        component: UserLotsComponent,
        canActivate: [LoginGuard]
    },
];

@NgModule({
    imports: [
        RouterModule,
        RouterModule.forChild(appRoutes),
        CommonComponentModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule
    ],
    exports: [
        FavoriteLotComponent,
        RouterModule
    ],
    declarations: [
        FavoriteLotComponent,
        UserBidsComponent,
        UserLotsComponent
    ]
})
export class UserCabinetModule { }