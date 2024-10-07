import { Route } from "@angular/router";
import { LogInComponent } from "./log-in/log-in.component";
import { SignUpComponent } from "./sign-up/sign-up.component";

export const authRoutes: Route[] = [
    {path: 'log-in', component: LogInComponent },
    {path: 'sign-up', component: SignUpComponent }
];