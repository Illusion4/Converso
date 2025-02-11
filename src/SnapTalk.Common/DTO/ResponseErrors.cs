using System.Xml;

namespace SnapTalk.Common.DTO;

public static class ResponseErrors
{
    public static readonly Error UsernameAlreadyExists = new("USERNAME_ALREADY_EXISTS");
    
    public static readonly Error InvalidOtpCode = new("INVALID_OTP_CODE");
    
    public static readonly Error InvalidCredentials = new("INVALID_CREDENTIALS", "Invalid email or password");
    
    public static readonly Error UserNotFound = new("USER_NOT_FOUND", "User with provided id not found");
    
    public static readonly Error UserAlreadyExists = new("USER_ALREADY_EXISTS", "User with provided email already exists");
    
    public static readonly Error ImageIsNotProvided = new("IMAGE_IS_NOT_PROVIDED", "Image is not provided");
    
    public static readonly Error CannotDeleteChatNotOwner = new("CANNOT_DELETE_CHAT_NOT_OWNER", "Only the chat owner can delete this chat");
    
    public static readonly Error NotMemberOfChat = new("NOT_MEMBER_OF_CHAT", "You are not a member of this chat");
    
    public static readonly Error ChatNotFound = new("CHAT_NOT_FOUND", "Chat not found");
    
    public static readonly Error ChatAlreadyExists = new("CHAT_ALREADY_EXISTS", "Chat with provided name already exists");
    
    public static readonly Error UserAlreadyInChat = new("USER_ALREADY_IN_CHAT", "User is already a member of this chat");
    
    public static readonly Error MessageNotFound = new("MESSAGE_NOT_FOUND", "The requested message could not be found");

    public static readonly Error NoRightsForAction = new("NO_RIGHTS_FOR_ACTION", "You do not have the rights to perform this action");
}