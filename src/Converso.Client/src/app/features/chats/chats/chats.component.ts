import { ChangeDetectionStrategy, Component, effect, ElementRef, inject, signal, Signal, ViewChild } from '@angular/core';
import { SidenavComponent } from "../../../shared/components/sidenav/sidenav.component";
import { Dialog, DialogModule } from '@angular/cdk/dialog';
import { ChatCreationComponent } from '../../../shared/components/chat-creation/chat-creation.component';
import { MatDialog } from '@angular/material/dialog';
import { ChatsService } from '../../../core/services/chats.service';
import { switchMap } from 'rxjs';
import { Chat } from '../../../shared/models/chat';
import { MessagesService } from '../../../core/services/messages.service';
import { Message } from '../../../shared/models/message';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChatListComponent } from "../chat-list/chat-list.component";
import { ChatWindowComponent } from "../chat-window/chat-window.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chats',
  standalone: true,
  imports: [SidenavComponent, DialogModule,CommonModule, ReactiveFormsModule, FormsModule, ChatListComponent, ChatWindowComponent],
  templateUrl: './chats.component.html',
  styleUrl: './chats.component.sass', 
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatsComponent {
  activePage: 'chats' | 'settings' = 'chats';

  chatService = inject(ChatsService);
  messageService = inject(MessagesService);
  authService = inject(AuthService);

  messages = signal<Message[]>([]);
  replyingToMessage = signal<Message | undefined>(undefined);

  newMessage = '';

  replyToMessage(message: Message) {
    this.replyingToMessage.set(message);
  }

  cancelReply() {
    this.replyingToMessage.set(undefined);
  }
}