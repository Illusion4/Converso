import { Component, input, model } from '@angular/core';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [],
  templateUrl: './input.component.html',
  styleUrl: './input.component.sass'
})
export class InputComponent {
  type = input<string>('text');

  identifier = input<string>('text');

  value = model<string>();

  placeholder = input<string>();

  disabled = input<boolean>(false);

  height = input<string>('54px');

  width = input<string>('360px');

  formControlName = input<string>();

  label = input<string>();
}
