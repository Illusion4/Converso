import { Component, effect, forwardRef, inject, Injector, input, model, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NgControl } from '@angular/forms';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [],
  templateUrl: './input.component.html',
  styleUrl: './input.component.sass',
  providers:[{
    provide: NG_VALUE_ACCESSOR,
    multi: true,
    useExisting: forwardRef(() => InputComponent),
}]
})
export class InputComponent implements ControlValueAccessor {

  type = input<string>('text');

  identifier = input<string>('text');

  value = model<string>();

  placeholder = input<string>();

  disabled = input<boolean>(false);

  height = input<string>('54px');

  width = input<string>('360px');

  label = input<string>();

  onValueChange(event: Event){
    this.onChange((event.target as HTMLInputElement).value);
  }

  onChange!: (value: string) => void;

  onTouchedFn!: () => void;

  writeValue(value: any): void {
    this.value.set(value);
  }
  
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  
  registerOnTouched(fn: any): void {
    this.onTouchedFn = fn;
  }
  
  setDisabledState(isDisabled: boolean): void {
    // Disable or enable your custom control
  }
}
