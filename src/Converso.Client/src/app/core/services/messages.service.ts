import { effect, inject, Injectable, signal } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { BehaviorSubject, combineLatest, combineLatestWith, distinct, filter, map, merge, of, Subject, switchMap, take, tap, withLatestFrom } from 'rxjs';
import { Response } from '../../shared/models/response';
import { Chat } from '../../shared/models/chat';
import { environment } from '../../../environments/environment';
import { Message } from '../../shared/models/message';
import { HttpClient } from '@angular/common/http';
import { ChatsService } from './chats.service';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  private http = inject(HttpClient);
  private chatsService = inject(ChatsService);
  private hubConnection: HubConnection = new HubConnectionBuilder()
  .withUrl(environment.chatHubUrl)
  .configureLogging(LogLevel.Information)
  .build();

  private groupConnections: string[] = [];
  private connectedToHub$ = new BehaviorSubject<boolean>(false);
  connectedToHub = this.connectedToHub$.asObservable();
  private messages = new BehaviorSubject<Message[]>([]);
  private message = new Subject<Message>();

  messages$ = merge(
    this.messages,
    this.message.pipe(
      tap(message => this.chatsService.updateChatLastMessage(message, message.chatId)),
      withLatestFrom(this.chatsService.selectedChat),
      filter(([message, selectedChat]) => selectedChat?.chatId === message.chatId),
      map(([message]) => message),
      map(newMessage => [...this.messages.value, newMessage]),
      tap(updatedMessages => this.messages.next(updatedMessages))
    ),
  );

  constructor() {
    this.hubConnection.start().then(() => {
      this.connectedToHub$.next(true);
    }).catch(function (err) {
      console.error(err.toString());
    });

    this.hubConnection.on('SendMessage', (message) => {
      this.message.next(message);
    })
  }

  connectToGroup(groupId: string){
    if(!this.groupConnections.includes(groupId)){
      this.hubConnection.invoke('ConnectToGroup', groupId);
    }
  }

  getMessages(chatId: string){
    return this.http.get<Response<Message[]>>(environment.apiUrl + 'messages/'+ chatId).pipe(
      map(response => response.data),
      tap(messages => this.messages.next(messages))
    );
  }

  sendMessage(messageForm: FormData){
    return this.http.post(environment.apiUrl + 'messages', messageForm);
  }
}
