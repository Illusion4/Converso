import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { BehaviorSubject, catchError, Observable, of, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

let isRefreshing = false;
const refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

export const refreshTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return next(req).pipe(
    catchError(err => {
      if(err instanceof HttpErrorResponse && err.status === 401){
        return handle401Error(err);
      }

      return throwError(err);
    })
  );

  function handle401Error(error: HttpErrorResponse): Observable<any>{
    const refreshToken = authService.getToken()?.refreshToken;

    return authService.refreshToken(refreshToken).pipe(
      switchMap((response) => {
        return next(
          req.clone({
            setHeaders: {
              Authorization: 'Bearer ' + response.data.accessToken
            },
            withCredentials: true
          })
        );
      }),
      catchError((_: HttpErrorResponse) => {
        authService.logout();
        return router.navigateByUrl('auth/log-in');
      })
    );

  }
};
