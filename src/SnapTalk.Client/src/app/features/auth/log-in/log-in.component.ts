import { Component } from '@angular/core';
import { InputComponent } from '../../../shared/components/input/input.component';

@Component({
  selector: 'app-log-in',
  standalone: true,
  imports: [InputComponent],
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.sass'
})
export class LogInComponent {
  value: string = '';
}
