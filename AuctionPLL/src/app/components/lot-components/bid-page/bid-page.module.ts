import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { CommonComponentModule } from "src/app/common/common-component.module";
import { SortPipe } from "src/app/pipes/sort.pipe";
import { AskOwnerFormComponent } from "./ask-owner-form/ask-owner-form.component";
import { CommentsComponent } from "./comments/comments.component";
import { FreshLotsComponent } from "./fresh-lots/fresh-lots.component";
import { ShowLotImagesComponent } from "./show-lot-images/show-lot-images.component";
import { ShowLotToBidComponent } from "./show-lot-to-bid/show-lot-to-bid.component";

const appRoutes: Routes = [
    {
        path: 'lot/:id',
        component: ShowLotToBidComponent
    }
];

@NgModule({
    exports: [
        RouterModule,
        ShowLotToBidComponent,
        FreshLotsComponent,
        ShowLotImagesComponent,
        CommentsComponent,
        AskOwnerFormComponent,
        SortPipe
    ],
    imports: [
        RouterModule,
        RouterModule.forChild(appRoutes),
        CommonComponentModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule
    ],
    providers: [],
    declarations: [
        ShowLotToBidComponent,
        FreshLotsComponent,
        ShowLotImagesComponent,
        CommentsComponent,
        AskOwnerFormComponent,
        SortPipe
    ]
})
export class BidPageModule { }