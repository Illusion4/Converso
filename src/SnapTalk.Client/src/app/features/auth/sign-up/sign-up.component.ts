import { Component } from '@angular/core';
import { InputComponent } from '../../../shared/components/input/input.component';
import { OtpVerificationComponent } from '../../../shared/components/otp-verification/otp-verification.component';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [InputComponent, OtpVerificationComponent],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.sass'
})
export class SignUpComponent {
  value: string = '';
}
