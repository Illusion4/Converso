import { ContentChildren, Directive, ElementRef, HostListener, input, output, QueryList } from '@angular/core';
import { OtpValueChangeEvent } from './input-navigation.directive';

@Directive({
  selector: '[appPasteOtp]',
  standalone: true
})
export class PasteOtpDirective {

  @ContentChildren('otpInputElement', {descendants: true})
  inputs!: QueryList<ElementRef<HTMLInputElement>>;

  regexp = input<RegExp>();

  valueChange = output<OtpValueChangeEvent>();

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent){
    event.preventDefault();
    this.handlePasteEvent(event.clipboardData?.getData('text'));
  }

  private handlePasteEvent(otp: string | undefined){
    if(otp && this.regexp()?.test(otp)){
      otp.split('').forEach((value, index) => {
        this.valueChange.emit({index, value});
      });
    }
  }
}
