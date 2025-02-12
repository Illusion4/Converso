import { AfterContentInit, ContentChild, contentChild, Directive, effect, ElementRef, input } from '@angular/core';

@Directive({
  selector: '[appAutoFocus]',
  standalone: true
})
export class AutoFocusDirective {

  @ContentChild('otpInputElement')
  child!: ElementRef<HTMLInputElement>;

  appAutoFocus = input<boolean>();

  constructor(){
    effect(() => {
      if(this.appAutoFocus() && this.child){
        this.child.nativeElement.focus();
      }
    });
  }
}
