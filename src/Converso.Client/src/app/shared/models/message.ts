export interface Message{
    userId: string,
    chatId: string,
    messageId: string,
    displayedName: string,
    message: string,
    displayedNameColor: string,
    attachmentUrl: string | null,
    userAvatarUrl: string | null,
    
    replyToMessage: string | null,
    replyToUser: string | null,
    replyToUserColor: string | null,
    
    createdAt: string,
    updatedAt: string | null
}