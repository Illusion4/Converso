import { Component, inject } from '@angular/core';
import { SidenavComponent } from "../../shared/components/sidenav/sidenav.component";
import { FormsModule } from '@angular/forms';
import { UserService } from '../../core/services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [SidenavComponent, FormsModule, CommonModule],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.sass'
})
export class SettingsComponent {
  userService = inject(UserService);
  user$ = this.userService.getCurrentUser();
  
  onEditField(arg0: string) {
    throw new Error('Method not implemented.');
  }
}
