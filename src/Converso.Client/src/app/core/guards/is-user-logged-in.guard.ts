import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const isUserLoggedInGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if(auth.isAuthenticated()){
    return true;
  }

  return router.navigate(['auth/log-in']);
};
