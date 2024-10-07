import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  httpClient = inject(HttpClient);

  sendVerifyEmailCode(email: string){
    return this.httpClient.post(environment.apiUrl, 'send-verify-email-code');
  }
}
