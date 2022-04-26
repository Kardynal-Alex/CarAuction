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
import { TestComponent } from './components/test/test.component';
import { ExitAboutGuard } from './guards/exit.about.guard';

import { CreateLotComponent } from './components/lot-components/manage-lot-form-components/create-lot/create-lot.component';
import { MenuComponent } from './components/menu/menu.component';
import { ShowLotsComponent } from './components/lot-components/show-lots/show-lots.component';
import { UserLotsComponent } from './components/lot-components/user-lots/user-lots.component';
import { UpdateLotComponent } from './components/lot-components/manage-lot-form-components/update-lot/update-lot.component';
import { UserBidsComponent } from './components/lot-components/user-bids/user-bids.component';
import { FavoriteLotComponent } from './components/lot-components/favorite-lot/favorite-lot.component';
import { SoldLotsComponent } from './components/lot-components/sold-lots/sold-lots.component';
import { ShowLotImagesComponent } from './components/lot-components/bid-page/show-lot-images/show-lot-images.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonConstants } from './common/constants/common-constants';
import { OverlayModule } from '@angular/cdk/overlay';
import { CommonComponentModule } from './common/common-component.module';
import { AdminModule } from './components/admin-components/admin.module';
import { LotFormModule } from './components/lot-components/manage-lot-form-components/lot-form.module';
import { BidPageModule } from './components/lot-components/bid-page/bid-page.module';
import { AuthComponentModule } from './components/auth-components/auth-component.module';

const appRoutes: Routes = [
  { path: '', component: ShowLotsComponent },
  // @deprecated
  // { path: 'createlotform', component: CreateLotComponent, canActivate: [LoginGuard], canDeactivate: [ExitAboutGuard] },
  {
    path: 'lotform',
    loadChildren: () => LotFormModule,
    canActivate: [LoginGuard]
  },
  {
    path: 'bid',
    loadChildren: () => BidPageModule
  },
  { path: 'userlots', component: UserLotsComponent, canActivate: [LoginGuard] },
  // @deprecated
  //{ path: 'userlots/updatelotform/:id', component: UpdateLotComponent, canActivate: [LoginGuard], canDeactivate: [ExitAboutGuard] },
  { path: 'userbids', component: UserBidsComponent, canActivate: [LoginGuard] },
  {
    path: 'admin',
    loadChildren: () => AdminModule, canActivate: [AdminGuard]
  },
  {
    path: 'auth',
    loadChildren: () => AuthComponentModule
  },
  { path: 'favorites', component: FavoriteLotComponent },
  { path: 'sold-lots', component: SoldLotsComponent },
  { path: 'test', component: TestComponent },
  { path: 'images', component: ShowLotImagesComponent },
  { path: '**', component: ShowLotsComponent }
];

export function tokenGetter() {
  return localStorage.getItem(CommonConstants.JWTToken);
}
@NgModule({
  declarations: [
    AppComponent,
    CreateLotComponent,
    MenuComponent,
    ShowLotsComponent,
    UserLotsComponent,
    UpdateLotComponent,
    UserBidsComponent,
    FavoriteLotComponent,
    SoldLotsComponent,
    TestComponent,
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
    CommonComponentModule,
    BidPageModule
  ],
  exports: [],
  providers: [
    ExitAboutGuard
  ],
  bootstrap: [AppComponent],
  entryComponents: []
})
export class AppModule { }
