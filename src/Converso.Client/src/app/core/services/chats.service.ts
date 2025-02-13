import { effect, inject, Injectable, signal } from '@angular/core';
import { createChatRequest } from '../../shared/models/createChatRequest';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Chat } from '../../shared/models/chat';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, map, of, Subject, tap } from 'rxjs';
import { Response } from '../../shared/models/response';
import { Message } from '../../shared/models/message';
import { MessagesService } from './messages.service';

@Injectable({
  providedIn: 'root'
})
export class ChatsService {
  private httpClient = inject(HttpClient);
  private chats$ = new BehaviorSubject<Chat[]>([]);
  private selectedChat$ = new BehaviorSubject<Chat | undefined>(undefined);

  chats = this.chats$.asObservable();

  selectedChat = this.selectedChat$.asObservable();
  chatListOpened = signal<boolean>(true);

  createChat(createChatRequest: createChatRequest){
    return this.httpClient.post<Response<Chat>>(environment.apiUrl + 'chats', createChatRequest).pipe(
      map(response => response.data),
      tap(chat => this.chats$.next([chat, ...this.chats$.value]))
    );
  } 

  selectChat(chat: Chat | undefined){
    this.selectedChat$.next(chat);
    this.chatListOpened.set(chat === undefined);
  }

  openChatList(){
    this.chatListOpened.set(true);
  }

  joinChat(chatId:string){
    return this.httpClient.post<Response<any>>(environment.apiUrl + 'chats/'+ chatId, {}).pipe(
      tap(res => {
        const chat = this.chats$.value.find(chat => chat.chatId === chatId);
        const updatedChat = { ...chat!, isMember: true };

        this.chats$.next([updatedChat!, ...this.chats$.value.filter(c => c.chatId !== updatedChat?.chatId)]);
        this.selectChat(updatedChat);
      })
    );
  }

  getChats(){
    return this.httpClient.get<Response<Chat[]>>(environment.apiUrl + 'chats',).pipe(
      map(response => response.data),
      tap(chats => this.chats$.next(chats))
    );
  }

  searchChats(title: string){
    return this.httpClient.get<Response<Chat[]>>(environment.apiUrl + 'chats/search?title=' + title).pipe(
      map(response => response.data),
      tap(chats => this.chats$.next(chats))
    );
  }

  updateChatLastMessage(message: Message, chatId: string){
    const chat = this.chats$.value.find(chat => chat.chatId === chatId);
    if(chat){
      const updatedChat = {...chat!, lastMessage: message.message, lastMessageDate: message.createdAt, lastMessageSender: message.displayedName };
      this.chats$.next([updatedChat!, ...this.chats$.value.filter(c => c.chatId !== updatedChat?.chatId)]);
    }
  }
}
