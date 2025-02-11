import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { InputComponent } from '../../../shared/components/input/input.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { LoginRequest } from '../../../shared/models/loginRequest';
import { tap } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-log-in',
  standalone: true,
  imports: [InputComponent, ReactiveFormsModule],
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.sass',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LogInComponent {
  authService = inject(AuthService);
  router = inject(Router);
  loginError: string | null = null;

  loginFormGroup:FormGroup = new FormGroup({
      email: new FormControl(null, [
        Validators.required,
        Validators.email,
        Validators.maxLength(255),
      ]),
    
      password: new FormControl(null, [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/[A-Z]/),
        Validators.pattern(/[a-z]/),
      ]),
  }, {updateOn: 'submit'});


  login(){
    if(this.loginFormGroup.invalid){
      this.loginError = 'Invalid email or password';
      return;
    }
    const loginRequest = {...this.loginFormGroup.value} as LoginRequest
    this.authService.login(loginRequest).pipe(
      tap(response =>{
        if(response.errors){
          this.loginError = 'Invalid email or password';
        }else{
          this.router.navigate([''])
        }
      })
    ).subscribe();

  }
  
}
