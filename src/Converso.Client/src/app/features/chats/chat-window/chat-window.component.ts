import { AfterViewChecked, ChangeDetectionStrategy, ChangeDetectorRef, Component, effect, ElementRef, HostListener, inject, input, signal, ViewChild } from '@angular/core';
import { ChatsService } from '../../../core/services/chats.service';
import { MessagesService } from '../../../core/services/messages.service';
import { AuthService } from '../../../core/services/auth.service';
import { Message } from '../../../shared/models/message';
import { Chat } from '../../../shared/models/chat';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { filter, map, switchMap, takeUntil, tap } from 'rxjs';
import { CommonModule, formatDate } from '@angular/common';

@Component({
  selector: 'app-chat-window',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './chat-window.component.html',
  styleUrl: './chat-window.component.sass',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatWindowComponent implements AfterViewChecked {
  @ViewChild('scrollContainer') scrollContainer!: ElementRef;
  changeDetectorRef = inject(ChangeDetectorRef)
  chatService = inject(ChatsService);
  messageService = inject(MessagesService);
  authService = inject(AuthService);

  showScrollButton = false;
  private scrollThreshold = 100;
  private shouldAutoScroll = true;

  selectedChat$ = this.chatService.selectedChat.pipe(
    filter((chat) => chat !== undefined),   
    switchMap(chat => this.messageService.getMessages(chat!.chatId).pipe(
      tap(_=> this.shouldAutoScroll = true),
      map(_ => chat)
    ))
  );

  replyingToMessage = signal<Message | null>(null);
  attachmentPreview = signal<string | null>(null);

  messageForm: FormGroup = new FormGroup({
    messageText: new FormControl('', [
        Validators.required,
    ]),
    attachment: new FormControl<File>(null!),
  });

  ngAfterViewChecked() {
    if(this.shouldAutoScroll){
      this.scrollToBottom();
    }
  }

  scrollToBottom() {
    if (this.scrollContainer) {
      const element = this.scrollContainer.nativeElement;
      element.scrollTo({
        top: element.scrollHeight,
        behavior: 'smooth'
      });

      // Re-enable auto-scroll after manual scroll
      this.shouldAutoScroll = true;
      this.showScrollButton = false;
    }
  }

  onScroll() {
    const element = this.scrollContainer.nativeElement;
    const isScrolledAboveThreshold = element.scrollHeight - element.scrollTop > element.clientHeight + this.scrollThreshold;
    const isAtBottom = element.scrollHeight - element.scrollTop === element.clientHeight;
    this.showScrollButton = isScrolledAboveThreshold;
    
    // If user manually scrolls, disable auto-scroll
    if (!isAtBottom) {
      this.shouldAutoScroll = false;
    }
  }

  onImagePicked(event: Event) {
    const file = (event.target as HTMLInputElement).files![0];

    const reader = new FileReader();
    reader.onload = () => {
      this.attachmentPreview.set(reader.result as string);
    };

    reader.readAsDataURL(file);

    this.messageForm.patchValue({ attachment: file});
  }

  getAttachmentName(){
    return this.messageForm.value.attachment.replace(/^.*[\\/]/, '');
  }


  sendMessage(selectedChat: Chat) {
    if(this.messageForm.invalid){
      return;
    }

    const newMessageForm = new FormData();
    newMessageForm.append('message', this.messageForm.value.messageText);
    newMessageForm.append('chatId', selectedChat?.chatId!);
    if(this.replyingToMessage()?.messageId){
      newMessageForm.append('replyToMessageId', this.replyingToMessage()?.messageId!);
    }

    if (this.messageForm.value.attachment) {
      newMessageForm.append('fileAttachment', this.messageForm.value.attachment);
    }

    this.scrollToBottom();
    this.messageService.sendMessage(newMessageForm).subscribe();
    this.messageForm.reset();
    this.cancelAttachment();
    this.cancelReply();
  }

  joinChat(selectedChat: Chat){
    this.chatService.joinChat(selectedChat?.chatId!).pipe(
      tap(chat => this.messageService.connectToGroup(selectedChat?.chatId!))
    ).subscribe();
  }
  
  replyToMessage(message: Message) {
    this.replyingToMessage.set(message);
  }
  
  cancelReply() {
    this.replyingToMessage.set(null);
  }

  cancelAttachment(){
    this.attachmentPreview.set(null);
    this.messageForm.patchValue({ attachment: null});
  }

  formatTime(date: string): string {
      const format = formatDate(date, 'hh:mm a', 'en-US');
  
      return format;
    }
}
