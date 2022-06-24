import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';
import { CommonComponentModule } from 'src/app/common/common-component.module';
import { ExitAboutGuard } from 'src/app/guards/exit.about.guard';
import { CreateLotFormComponent } from './create-lot-form/create-lot-form.component';
import { UpdateLotFormComponent } from './update-lot-form/update-lot-form.component';

const appRoutes: Routes = [
    {
        path: 'createlot',
        component: CreateLotFormComponent,
        canDeactivate: [ExitAboutGuard]
    },
    {
        path: 'updatelot/:id',
        component: UpdateLotFormComponent,
        canDeactivate: [ExitAboutGuard]
    }
]

@NgModule({
    imports: [
        RouterModule,
        RouterModule.forChild(appRoutes),
        CommonComponentModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        TextareaAutosizeModule
    ],
    exports: [
        RouterModule,
        CreateLotFormComponent,
        UpdateLotFormComponent
    ],
    providers: [],
    declarations: [
        CreateLotFormComponent,
        UpdateLotFormComponent
    ]
})
export class LotFormModule { }