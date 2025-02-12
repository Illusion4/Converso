import { Routes } from '@angular/router';
import { isUserLoggedInGuard } from './core/guards/is-user-logged-in.guard';

export const routes: Routes = [
    { 
        path: "",
        redirectTo:"chats",
        pathMatch:"full"
    },
    { 
        path: 'auth', loadChildren: () => import('./features/auth/auth.routes').then((m) => m.authRoutes )
    },
    {
        path: 'chats', loadComponent: () => import('./features/chats/chats/chats.component').then((m) => m.ChatsComponent),
        canActivate: [isUserLoggedInGuard]
    },
    {
        path: 'settings', loadComponent: () => import('./features/settings/settings.component').then((m) => m.SettingsComponent),
        canActivate: [isUserLoggedInGuard]
    }
];
