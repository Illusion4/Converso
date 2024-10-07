import { AfterContentInit, ContentChildren, Directive, ElementRef, HostListener, input, output, QueryList, ViewChildren } from '@angular/core';

export type OtpValueChangeEvent = {
  index: number,
  value: string
}

@Directive({
  selector: '[appInputNavigation]',
  standalone: true
})
export class InputNavigationDirective implements AfterContentInit {
  inputs: ElementRef<HTMLInputElement>[] = [];

  @ContentChildren('otpInputElement', { descendants: true })
  queryInputs!:QueryList<ElementRef<HTMLInputElement>>;

  valueChange = output<OtpValueChangeEvent>();
  regexp = input<RegExp>();

  ngAfterContentInit(): void {
    this.inputs = this.queryInputs.toArray();
  }

  getInputIndex(inputEl: HTMLInputElement){
    return this.inputs.findIndex(val => val.nativeElement === inputEl);
  }

  setFocus(index:number){
    if(index >= 0 && index < this.inputs.length){
      this.inputs[index].nativeElement.focus();
    }
  }

  @HostListener('keydown.arrowLeft', ['$event'])
  onArrowLeft($event: KeyboardEvent){
    const index = this.getInputIndex($event.target as HTMLInputElement);

    this.setFocus(index -1);
    
  }

  @HostListener('keydown.arrowRight', ['$event'])
  onArrowRight($event: KeyboardEvent){
    const index = this.getInputIndex($event.target as HTMLInputElement);

    this.setFocus(index + 1);
  }

  @HostListener('keydown.backspace', ['$event'])
  onBackspace($event: KeyboardEvent){
    const index = this.getInputIndex($event.target as HTMLInputElement);

    this.valueChange.emit({index, value: ''});
    this.setFocus(index - 1);
    $event.preventDefault();
  }

  @HostListener('input', ['$event'])
  onInput(event: InputEvent){
    const index = this.getInputIndex(event.target as HTMLInputElement);

    if(event.data?.match(this.regexp()!)){
      this.valueChange.emit({
        index,
        value: event.data
      })
      this.setFocus(index + 1)
    } else {
      this.inputs[index].nativeElement.value = '';
    }
  }

}
