import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { RegisterRequest } from '../../shared/models/registerRequest';
import { InputTokens } from '../../shared/models/InputTokens';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, tap } from 'rxjs';
import { Router } from '@angular/router';
import { Response } from '../../shared/models/response';
import { LoginRequest } from '../../shared/models/loginRequest';
import { ChatsService } from './chats.service';

export const ACCESS_TOKEN_KEY = 'access_token'
export const REFRESH_TOKEN_KEY = 'refresh_token'

export function tokenGetter() {
  return localStorage.getItem(ACCESS_TOKEN_KEY);
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  jwtHelper = inject(JwtHelperService);
  httpClient = inject(HttpClient);
  router = inject(Router);
  chatService = inject(ChatsService);
  private currentUserId = new BehaviorSubject<string | undefined>(undefined);
  currentUserId$ = this.currentUserId.asObservable();

  constructor(){
    const token = this.getToken()?.accessToken;
    if(token){
      let decodedJWT = JSON.parse(window.atob(token.split('.')[1]));
      this.currentUserId.next(decodedJWT.jti);
    }
  }

  sendVerifyEmailCode(email: string){
    return this.httpClient.post(environment.apiUrl + 'auth/' + 'send-verify-email-code', {email});
  }

  register(registerRequest: RegisterRequest){
    return this.httpClient.post<Response<InputTokens>>(environment.apiUrl + 'auth/' + 'register', registerRequest).pipe(
      tap(response => this.saveToken(response.data))
    );
  }

  login(loginRequest: LoginRequest){
    return this.httpClient.post<Response<InputTokens>>(environment.apiUrl + 'auth/' + 'login', loginRequest).pipe(
      tap(response => this.saveToken(response.data))
    );
  }

  refreshToken(refreshToken: string){
    return this.httpClient.post<Response<InputTokens>>(environment.apiUrl + 'auth/' + 'refresh-token', {refreshToken}).pipe(
      tap(response => this.saveToken(response.data))
    );;
  };
  

  private saveToken(token: InputTokens) {
    localStorage.setItem( ACCESS_TOKEN_KEY, JSON.stringify(token));

    let decodedJWT = JSON.parse(window.atob(token.accessToken.split('.')[1]));
    this.currentUserId.next(decodedJWT.jti);
  }

  getToken(): InputTokens{
    return JSON.parse(localStorage.getItem(ACCESS_TOKEN_KEY)!) as InputTokens;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token;
  }

  logout(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    this.router.navigate(['auth/log-in']);
    this.chatService.selectChat(undefined!);
  }
}
