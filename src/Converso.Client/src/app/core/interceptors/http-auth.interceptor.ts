import { HttpHeaders, HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const httpAuthInterceptor: HttpInterceptorFn = (req, next) => {

  const auth = inject(AuthService);
  const accessToken = auth.getToken()?.accessToken;

  const headerWithToken = req.clone({
    headers: new HttpHeaders({ Authorization: 'Bearer ' + accessToken })
  });


  return next(headerWithToken);
};
