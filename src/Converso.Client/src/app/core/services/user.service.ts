import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { first, map, Observable, switchMap, take } from 'rxjs';
import { Response } from '../../shared/models/response';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';
import { User } from '../../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  httpClient = inject(HttpClient);
  authService = inject(AuthService);


  checkEmail(email: string): Observable<boolean> {
    return this.httpClient.get<Response<boolean>>(environment.apiUrl + `users/is-existing-email?email=${email}`).pipe(
      map(response => response.data)
  );
}

  getCurrentUser(): Observable<User> {
    return this.authService.currentUserId$.pipe(
      take(1),
      switchMap(userId => this.httpClient.get<Response<User>>(environment.apiUrl + 'users/'+ userId)),
      map(response => response.data),
  )}
}
