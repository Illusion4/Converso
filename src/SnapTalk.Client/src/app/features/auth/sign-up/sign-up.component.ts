import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { InputComponent } from '../../../shared/components/input/input.component';
import { OtpVerificationComponent } from '../../../shared/components/otp-verification/otp-verification.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { RegisterRequest } from '../../../shared/models/registerRequest';
import { routes } from '../../../app.routes';
import { Router } from '@angular/router';
import { patternWithMessage } from '../../../shared/validators/patternWithMessage';
import { KeyValuePipe } from '@angular/common';
import { emailExistsValidator } from '../../../shared/validators/emailExistsValidator';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [InputComponent, OtpVerificationComponent, ReactiveFormsModule],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.sass',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SignUpComponent {
  authService = inject(AuthService);
  userService = inject(UserService);
  router = inject(Router);
  registrationStep: 'initialForm' | 'confirmationCodeForm' = 'initialForm';

  registerFormGroup:FormGroup = new FormGroup({
    email: new FormControl(null, [
      Validators.required,
      Validators.email,
      Validators.maxLength(255),
    ], [emailExistsValidator(this.userService)]),
  
    firstName: new FormControl(null, [
      Validators.required,
      Validators.maxLength(50),
    ]),
  
    lastName: new FormControl(null, [
      Validators.maxLength(50),
    ]),
  
    password: new FormControl(null, [
      Validators.required,
      Validators.minLength(8),
      Validators.pattern(/[A-Z]/), // At least one uppercase letter
      Validators.pattern(/[a-z]/), // At least one lowercase letter
    ]),
  }, {updateOn: 'change'});

  onRegistrationFormSubmit(){
    this.authService.sendVerifyEmailCode(this.registerFormGroup.value.email).subscribe();
    this.registrationStep = 'confirmationCodeForm';
  }

  onOtpCodeComplete(otpCode:string){
    const registerRequest = this.registerFormGroup.value as RegisterRequest;
    registerRequest.code = otpCode;
    this.authService.register(registerRequest).subscribe(
      result => this.router.navigate([''])
    );
  }
  
}
