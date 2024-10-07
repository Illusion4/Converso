import { ContentChildren, Directive, effect, ElementRef, input, QueryList } from '@angular/core';

@Directive({
  selector: '[appAutoBlur]',
  standalone: true
})
export class AutoBlurDirective {
  @ContentChildren('otpInputElement', { descendants: true })
  queryInputs!:QueryList<ElementRef<HTMLInputElement>>;

  isFormValid = input<boolean>();

  constructor() {
    effect(()=>{
      if(this.isFormValid()){
        this.queryInputs.forEach((value => value.nativeElement.blur()));
      }
    });
  }

}
