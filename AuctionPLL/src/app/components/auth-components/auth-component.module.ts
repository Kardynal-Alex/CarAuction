import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule, Routes } from "@angular/router";
import { EmailConfirmationComponent } from "./email-confirmation/email-confirmation.component";
import { FacebookLoginComponent } from "./facebook-login/facebook-login.component";
import { ForgotPasswordComponent } from "./forgot-password/forgot-password.component";
import { GoogleLoginComponent } from "./google-login/google-login.component";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { ResetPasswordComponent } from "./reset-password/reset-password.component";
import { TwoStepVerificationComponent } from "./two-step-verification/two-step-verification.component";

const appRoutes: Routes = [
    {
        path: 'resetpassword',
        component: ResetPasswordComponent
    },
    {
        path: 'emailconfirmation',
        component: EmailConfirmationComponent
    },
    {
        path: 'twostepverification',
        component: TwoStepVerificationComponent
    },
];

@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        RouterModule.forChild(appRoutes)
    ],
    exports: [
        LoginComponent
    ],
    providers: [
        RouterModule
    ],
    declarations: [
        LoginComponent,
        RegisterComponent,
        FacebookLoginComponent,
        GoogleLoginComponent,
        ResetPasswordComponent,
        ForgotPasswordComponent,
        TwoStepVerificationComponent,
        EmailConfirmationComponent
    ]
})
export class AuthComponentModule { }