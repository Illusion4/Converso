import { DIALOG_DATA, DialogModule } from '@angular/cdk/dialog';
import { Component, inject, model } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Title } from '@angular/platform-browser';
import { createChatRequest } from '../../models/createChatRequest';

@Component({
  selector: 'app-chat-creation',
  standalone: true,
  imports: [DialogModule, ReactiveFormsModule],
  templateUrl: './chat-creation.component.html',
  styleUrl: './chat-creation.component.sass'
})

export class ChatCreationComponent {
  createChatForm = new FormGroup({
    title: new FormControl('', Validators.required),
    description: new FormControl('')
  });

  readonly dialogRef = inject(MatDialogRef<ChatCreationComponent>);
  data = inject(DIALOG_DATA);

  closeNewGroupDialog(): void {
    this.dialogRef.close();
  }

  createGroup() {
    const createChatRequest = this.createChatForm.value as createChatRequest;
    this.dialogRef.close(createChatRequest);
  }

}
