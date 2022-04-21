import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from "@auth0/angular-jwt";
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';

import { LoginGuard } from './guards/login.guard';
import { AdminGuard } from './guards/admin.guard';
import { SortPipe } from '../app/pipes/sort.pipe';
import { TestComponent } from './components/test/test.component';
import { ExitAboutGuard } from './guards/exit.about.guard';

import { LoginComponent } from './components/auth-components/login/login.component';
import { RegisterComponent } from './components/auth-components/register/register.component';
import { CreateLotComponent } from './components/lot-components/create-lot/create-lot.component';
import { MenuComponent } from './components/menu/menu.component';
import { ShowLotsComponent } from './components/lot-components/show-lots/show-lots.component';
import { UserLotsComponent } from './components/lot-components/user-lots/user-lots.component';
import { ShowLotToBidComponent } from './components/lot-components/show-lot-to-bid/show-lot-to-bid.component';
import { UpdateLotComponent } from './components/lot-components/update-lot/update-lot.component';
import { UserBidsComponent } from './components/lot-components/user-bids/user-bids.component';
import { AdminComponent } from './components/admin/admin.component';
import { EmailConfirmationComponent } from './components/auth-components/email-confirmation/email-confirmation.component';
import { ForgotPasswordComponent } from './components/auth-components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/auth-components/reset-password/reset-password.component';
import { FavoriteLotComponent } from './components/lot-components/favorite-lot/favorite-lot.component';
import { SoldLotsComponent } from './components/lot-components/sold-lots/sold-lots.component';
import { ShowLotImagesComponent } from './components/lot-components/show-lot-images/show-lot-images.component';
import { AskOwnerFormComponent } from './components/ask-owner-form/ask-owner-form.component';
import { CommentsComponent } from './components/lot-components/comments/comments.component';
import { FreshLotsComponent } from './components/lot-components/fresh-lots/fresh-lots.component';
import { FacebookLoginComponent } from './components/auth-components/facebook-login/facebook-login.component';
import { GoogleLoginComponent } from './components/auth-components/google-login/google-login.component';
import { TwoStepVerificationComponent } from './components/auth-components/two-step-verification/two-step-verification.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonConstants } from './common/constants/common-constants';
import { CreateLotFormComponent } from './components/lot-components/create-lot-form/create-lot-form.component';
import { UpdateLotFormComponent } from './components/lot-components/update-lot-form/update-lot-form.component';
import { OverlayModule } from '@angular/cdk/overlay';
import { CommonModule } from './common/common.module';

const appRoutes: Routes = [
  { path: '', component: ShowLotsComponent },
  { path: 'createlot', component: CreateLotFormComponent, canActivate: [LoginGuard], canDeactivate: [ExitAboutGuard] },
  { path: 'createlotform', component: CreateLotComponent, canActivate: [LoginGuard], canDeactivate: [ExitAboutGuard] },
  { path: 'lot/:id', component: ShowLotToBidComponent },
  { path: 'userlots', component: UserLotsComponent, canActivate: [LoginGuard] },
  { path: 'userlots/updatelot/:id', component: UpdateLotFormComponent, canActivate: [LoginGuard], canDeactivate: [ExitAboutGuard] },
  { path: 'userlots/updatelotform/:id', component: UpdateLotComponent, canActivate: [LoginGuard], canDeactivate: [ExitAboutGuard] },
  { path: 'userbids', component: UserBidsComponent, canActivate: [LoginGuard] },
  { path: 'admin', component: AdminComponent, canActivate: [AdminGuard] },
  { path: 'emailconfirmation', component: EmailConfirmationComponent },
  { path: 'resetpassword', component: ResetPasswordComponent },
  { path: 'favorites', component: FavoriteLotComponent },
  { path: 'sold-lots', component: SoldLotsComponent },
  { path: 'test', component: TestComponent },
  { path: 'images', component: ShowLotImagesComponent },
  { path: 'twostepverification', component: TwoStepVerificationComponent },
  { path: '**', component: ShowLotsComponent }
];

export function tokenGetter() {
  return localStorage.getItem(CommonConstants.JWTToken);
}
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    CreateLotComponent,
    MenuComponent,
    ShowLotsComponent,
    UserLotsComponent,
    ShowLotToBidComponent,
    UpdateLotComponent,
    UserBidsComponent,
    SortPipe,
    AdminComponent,
    EmailConfirmationComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    FavoriteLotComponent,
    SoldLotsComponent,
    TestComponent,
    ShowLotImagesComponent,
    CommentsComponent,
    FreshLotsComponent,
    FacebookLoginComponent,
    GoogleLoginComponent,
    AskOwnerFormComponent,
    TwoStepVerificationComponent,
    CreateLotFormComponent,
    UpdateLotFormComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    MatTooltipModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    BrowserAnimationsModule,
    ReactiveFormsModule,
    ToastrModule.forRoot({ preventDuplicates: true }),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ["localhost:4200"],
        blacklistedRoutes: []
      }
    }),
    NgbModule,
    MatMenuModule,
    OverlayModule,
    TextareaAutosizeModule,
    CommonModule
  ],
  exports: [],
  providers: [
    SortPipe,
    ExitAboutGuard
  ],
  bootstrap: [AppComponent],
  entryComponents: []
})
export class AppModule { }
