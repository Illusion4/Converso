import { Component, output } from '@angular/core';
import { FormArray, FormControl, Validators } from '@angular/forms';
import { PasteOtpDirective } from '../../directives/paste-otp.directive';
import { InputNavigationDirective, OtpValueChangeEvent } from '../../directives/input-navigation.directive';
import { AutoFocusDirective } from '../../directives/auto-focus.directive';
import { AutoBlurDirective } from '../../directives/auto-blur.directive';

@Component({
  selector: 'app-otp-verification',
  standalone: true,
  imports: [PasteOtpDirective, InputNavigationDirective, AutoFocusDirective, AutoBlurDirective],
  templateUrl: './otp-verification.component.html',
  styleUrl: './otp-verification.component.sass'
})
export class OtpVerificationComponent {

  inputs = new FormArray(Array.from(
    { length:6 }, 
    () => new FormControl('', Validators.required)
  ));
  
  regexp = new RegExp('^[0-9]+$');

  otpComplete = output<string>();

  onValueChange($event: OtpValueChangeEvent){
    this.inputs.controls[$event.index].setValue($event.value);

    if(this.inputs.valid){
      const otpCode = this.inputs.value.join('');
      this.otpComplete.emit(otpCode);
    }
  }

}
