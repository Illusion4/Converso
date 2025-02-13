import { ChangeDetectionStrategy, Component, effect, inject, Output, output, signal } from '@angular/core';
import { Chat } from '../../../shared/models/chat';
import { ChatsComponent } from '../chats/chats.component';
import { ChatsService } from '../../../core/services/chats.service';
import { MatDialog } from '@angular/material/dialog';
import { ChatCreationComponent } from '../../../shared/components/chat-creation/chat-creation.component';
import { BehaviorSubject, filter, map, Subject, switchMap, take, tap } from 'rxjs';
import { MessagesService } from '../../../core/services/messages.service';
import { CommonModule, formatDate } from '@angular/common';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.sass',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatListComponent {
  dialog = inject(MatDialog);
  chatsService = inject(ChatsService);
  messagesService = inject(MessagesService);

  searchQuerySubject = new Subject<string>();

  constructor(){
    this.chatsService.getChats().pipe(
      switchMap(chats => this.messagesService.connectedToHub.pipe(
        filter(connected => connected),
        take(1),
        map(()=>chats)
      )),
      tap(chats => chats.forEach(chat =>{
        this.messagesService.connectToGroup(chat.chatId);
      }))
    ).subscribe();

    this.searchQuerySubject.pipe(
      switchMap(title => title.trim() 
                ? this.chatsService.searchChats(title) 
                : this.chatsService.getChats()),
    ).subscribe();
  }

  searchValueChange(event:Event){
    this.searchQuerySubject.next((event.target as HTMLInputElement).value);
  }

  selectChat(chat: Chat){
    this.chatsService.selectChat(chat);
  }

  openNewGroupDialog() {
    this.dialog.open(ChatCreationComponent, {
      minWidth: '400px'
    }).afterClosed().pipe(
      filter(data => data),
      switchMap(newChatRequest => this.chatsService.createChat(newChatRequest)),
      tap(chat => this.messagesService.connectToGroup(chat.chatId))
    ).subscribe();
  }

  formatTime(date: string): string {
    const format = formatDate(date, 'hh:mm a', 'en-US');

    return format;
  }

}
